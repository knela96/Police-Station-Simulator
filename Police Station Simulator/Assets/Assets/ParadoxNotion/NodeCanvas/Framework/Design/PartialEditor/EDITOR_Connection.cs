#if UNITY_EDITOR

using System.Collections;
using NodeCanvas.Editor;
using ParadoxNotion.Design;
using ParadoxNotion.Services;
using UnityEditor;
using UnityEngine;
using Logger = ParadoxNotion.Services.Logger;


namespace NodeCanvas.Framework{

	partial class Connection{

		protected enum TipConnectionStyle
		{
			None,
			Circle,
			Arrow
		}

		[SerializeField]
		private bool _infoCollapsed;
		
		const float RELINK_DISTANCE_SNAP      = 20f;
		const float STATUS_BLINK_DURATION     = 0.25f;
		const float STATUS_BLINK_SIZE_ADD     = 2f;
		const float STATUS_BLINK_PACKET_SPEED = 0.8f;
		const float STATUS_BLINK_PACKET_SIZE  = 10f;
		const float STATUS_BLINK_PACKET_COUNT = 4f;

		private Rect centerRect;
		private Rect startRect;
		private Rect endRect;

		private Status lastStatus = Status.Resting;
		private Color color = Node.statusColors[Status.Resting];
		private float size = 3;
		private bool isRelinking;
		private Vector2 relinkClickPos;

		private float statusChangeTime;
		private Vector2 lineFromTangent;
		private Vector2 lineToTangent;
		private float hor;

		///----------------------------------------------------------------------------------------------

		private bool infoExpanded{
			get {return !_infoCollapsed;}
			set {_infoCollapsed = !value;}
		}

		virtual protected Color defaultColor{
			get {return Node.statusColors[Status.Resting];}
		}

		virtual protected float defaultSize{
			get {return 3f;}
		}

		virtual protected TipConnectionStyle tipConnectionStyle{
			get {return TipConnectionStyle.Circle;}
		}

		virtual protected bool canRelink{
			get {return true;}
		}

		///----------------------------------------------------------------------------------------------

		//Draw connection from-to
		public void DrawConnectionGUI(Vector2 lineFrom, Vector2 lineTo){
			
			startRect = new Rect(0,0,12,12);
			startRect.center = lineFrom;

			endRect = new Rect(0, 0, 16, 16);
			endRect.center = lineTo;

			ResolveTangents(lineFrom, lineTo);

			if (Application.isPlaying && isActive){
				UpdateBlinkStatus(lineFrom, lineTo);
			}

			HandleEvents(lineFrom, lineTo);
			if (!isRelinking || Vector2.Distance(relinkClickPos, Event.current.mousePosition) < RELINK_DISTANCE_SNAP ){
				DrawConnection(lineFrom, lineTo);
				DrawInfoRect(lineFrom, lineTo);
			}
		}

		//Resolve tangency
		void ResolveTangents(Vector2 lineFrom, Vector2 lineTo){
			var rigid = 0f;
			if (NCPrefs.connectionStyle == NCPrefs.ConnectionStyle.Curved){ rigid = 0.8f; }
			if (NCPrefs.connectionStyle == NCPrefs.ConnectionStyle.Stepped){ rigid = 1f; }
			var tangentX = Mathf.Abs(lineFrom.x - lineTo.x) * rigid;
			var tangentY = Mathf.Abs(lineFrom.y - lineTo.y) * rigid;

			GUI.color = color;
			hor = 0;

			if (lineFrom.x <= sourceNode.rect.x){
				lineFromTangent = new Vector2(-tangentX, 0);
				hor--;
			}

			if (lineFrom.x >= sourceNode.rect.xMax){
				lineFromTangent = new Vector2(tangentX, 0);
				hor++;
			}

			if (lineFrom.y <= sourceNode.rect.y){
				lineFromTangent = new Vector2(0, -tangentY);
			}

			if (lineFrom.y >= sourceNode.rect.yMax){
				lineFromTangent = new Vector2(0, tangentY);
			}


			if (lineTo.x <= targetNode.rect.x){
				lineToTangent = new Vector2(-tangentX, 0);
				hor--;
				if (tipConnectionStyle == TipConnectionStyle.Arrow){
					GUI.Box(endRect, string.Empty, CanvasStyles.arrowRight);
				}
			}

			if (lineTo.x >= targetNode.rect.xMax){
				lineToTangent = new Vector2(tangentX, 0);
				hor++;
				if (tipConnectionStyle == TipConnectionStyle.Arrow){
					GUI.Box(endRect, string.Empty, CanvasStyles.arrowLeft);
				}
			}

			if (lineTo.y <= targetNode.rect.y){
				lineToTangent = new Vector2(0, -tangentY);
				if (tipConnectionStyle == TipConnectionStyle.Arrow){
					GUI.Box(endRect, string.Empty, CanvasStyles.arrowBottom);
				}
			}

			if (lineTo.y >= targetNode.rect.yMax){
				lineToTangent = new Vector2(0, tangentY);
				if (tipConnectionStyle == TipConnectionStyle.Arrow){
					GUI.Box(endRect, string.Empty, CanvasStyles.arrowTop);
				}
			}

			if (tipConnectionStyle == TipConnectionStyle.Circle){
				GUI.Box(endRect, string.Empty, CanvasStyles.circle);
			}

			hor = hor == 0? 0.5f : 1;
			GUI.color = Color.white;
		}

		//The actual connection graphic
		void DrawConnection(Vector2 lineFrom, Vector2 lineTo){
			
			color = isActive? color : new Color(0.3f, 0.3f, 0.3f);
			if (!Application.isPlaying){
				color = isActive? defaultColor : new Color(0.3f, 0.3f, 0.3f);
				var highlight = GraphEditorUtility.activeElement == this || GraphEditorUtility.activeElement == sourceNode || GraphEditorUtility.activeElement == targetNode;
				if (startRect.Contains(Event.current.mousePosition) || endRect.Contains(Event.current.mousePosition)){
					highlight = true;
				}
				color.a = highlight? 1 : color.a;
				size = highlight? defaultSize + 2 : defaultSize;
			}

			Handles.color = color;
			if (NCPrefs.connectionStyle == NCPrefs.ConnectionStyle.Curved){
				var shadow = new Vector2(3.5f, 3.5f);
				Handles.DrawBezier(lineFrom, lineTo + shadow, lineFrom + shadow + lineFromTangent + shadow, lineTo + shadow + lineToTangent, new Color(0,0,0,0.1f), null, size + 10f);
				Handles.DrawBezier(lineFrom, lineTo, lineFrom + lineFromTangent, lineTo + lineToTangent, color, null, size);
			} else if (NCPrefs.connectionStyle == NCPrefs.ConnectionStyle.Stepped){
				var shadow = new Vector2(1,1);
				Handles.DrawPolyLine(lineFrom, lineFrom + lineFromTangent * hor, lineTo + lineToTangent * hor, lineTo);
				Handles.DrawPolyLine(lineFrom + shadow, (lineFrom + lineFromTangent * hor) + shadow, (lineTo + lineToTangent * hor) + shadow, lineTo + shadow);
			} else {
				Handles.DrawBezier(lineFrom, lineTo, lineFrom, lineTo, color, null, size);
			}
			Handles.color = Color.white;
		}

		//Information showing in the middle
		void DrawInfoRect(Vector2 lineFrom, Vector2 lineTo){

			centerRect.center = GetPosAlongConnectionCurve(lineFrom, lineTo, lineFromTangent, lineToTangent, 0.5f);
			var isExpanded = infoExpanded || GraphEditorUtility.activeElement == this || GraphEditorUtility.activeElement == sourceNode;
			var alpha = isExpanded? 0.8f : 0.25f;
			var info = GetConnectionInfo();
			var extraInfo = sourceNode.GetConnectionInfo( sourceNode.outConnections.IndexOf(this) );
			if (!string.IsNullOrEmpty(info) || !string.IsNullOrEmpty(extraInfo)){
				
				if (!string.IsNullOrEmpty(extraInfo) && !string.IsNullOrEmpty(info)){
					extraInfo = "\n" + extraInfo;
				}

				var textToShow = isExpanded? string.Format("<size=9>{0}{1}</size>", info, extraInfo) :  "<size=9>...</size>";
				var finalSize = CanvasStyles.box.CalcSize(new GUIContent(textToShow));

				centerRect.width = finalSize.x;
				centerRect.height = finalSize.y;

				GUI.color = new Color(1f,1f,1f,alpha);
				GUI.Box(centerRect, textToShow, CanvasStyles.box);
				GUI.color = Color.white;

			} else {
			
				centerRect.width = 0;
				centerRect.height = 0;
			}
		}

		///Returns the mid position rect of the connection
		public Rect GetMidRect(){
			return centerRect;
		}

		///Get position on curve from, to, by t
		static Vector2 GetPosAlongConnectionCurve(Vector2 from, Vector2 to, Vector2 fromTangent, Vector2 toTangent, float t){
			float u = 1.0f - t;
		    float tt = t * t;
		    float uu = u * u;
		    float uuu = uu * u;
		    float ttt = tt * t;
		    Vector2 result = uuu * from;
		    result += 3 * uu * t * (from + fromTangent);
		    result += 3 * u * tt * (to + toTangent);
		    result += ttt * to;
		    return result;
		}

		///Is target position along from, to curve
		static bool IsPositionAlongConnection(Vector2 from, Vector2 to, Vector2 fromTangent, Vector2 toTangent, Vector2 targetPosition){
			if ( ParadoxNotion.RectUtils.GetBoundRect(from, to).Contains(targetPosition) ){
				var CLICK_CHECK_RES = 50f;
				var CLICK_CHECK_DISTANCE = 10f;
				for (var i = 0f; i <= CLICK_CHECK_RES; i++){
					var checkPos = GetPosAlongConnectionCurve(from, to, fromTangent, toTangent, i/CLICK_CHECK_RES );
					if ( Vector2.Distance( targetPosition, checkPos ) < CLICK_CHECK_DISTANCE ){
						return true;
					}
				}
			}
			return false;
		}


		//The connection's inspector
		public static void ShowConnectionInspectorGUI(Connection c){

			UndoManager.CheckUndo(c.graph, "Connection Inspector");

			GUILayout.BeginHorizontal();
			GUI.color = new Color(1,1,1,0.5f);

			if (GUILayout.Button("◄", GUILayout.Height(14), GUILayout.Width(20))){
				GraphEditorUtility.activeElement = c.sourceNode;
			}

			if (GUILayout.Button("►", GUILayout.Height(14), GUILayout.Width(20))){
				GraphEditorUtility.activeElement = c.targetNode;
			}

			c.isActive = EditorGUILayout.ToggleLeft("ACTIVE", c.isActive, GUILayout.Width(150));

			GUILayout.FlexibleSpace();

			if (GUILayout.Button("X", GUILayout.Height(14), GUILayout.Width(20))){
				GraphEditorUtility.PostGUI += delegate { c.graph.RemoveConnection(c); };
				return;
			}

			GUI.color = Color.white;
			GUILayout.EndHorizontal();

			EditorUtils.BoldSeparator();
			c.OnConnectionInspectorGUI();
			c.sourceNode.OnConnectionInspectorGUI(c.sourceNode.outConnections.IndexOf(c));

			UndoManager.CheckDirty(c.graph);
		}

		//The information to show in the middle area of the connection
		virtual protected string GetConnectionInfo(){ return null; }
		//Editor.Override to show controls in the editor panel when connection is selected
		virtual protected void OnConnectionInspectorGUI(){}


		///Handle UI events
		void HandleEvents(Vector2 lineFrom, Vector2 lineTo){

			var e = Event.current;

			//On click select this connection
			if ( GraphEditorUtility.allowClick && e.type == EventType.MouseDown && e.button == 0 ){
				if ( IsPositionAlongConnection(lineFrom, lineTo, lineFromTangent, lineToTangent, e.mousePosition) || centerRect.Contains(e.mousePosition) || startRect.Contains(e.mousePosition) || endRect.Contains(e.mousePosition) ){
					if (canRelink){
						isRelinking = true;
						relinkClickPos = e.mousePosition;
					}
					GraphEditorUtility.activeElement = this;
					e.Use();
					return;
				}
			}

			if (canRelink && isRelinking){
				if (Vector2.Distance(relinkClickPos, e.mousePosition) > RELINK_DISTANCE_SNAP){
					Handles.DrawBezier(startRect.center, e.mousePosition, startRect.center, e.mousePosition, defaultColor, null, defaultSize);
				}
				if (e.rawType == EventType.MouseUp && e.button == 0){					
					foreach(var node in graph.allNodes){
						if (node != targetNode && node != sourceNode && node.rect.Contains(e.mousePosition) && node.IsNewConnectionAllowed() ){
							SetTarget(node);
							break;
						}
					}
					isRelinking = false;
					e.Use();
				}
			}

			if (GraphEditorUtility.allowClick && e.type == EventType.MouseDown && e.button == 1 && centerRect.Contains(e.mousePosition)){
				var menu = new GenericMenu();
				menu.AddItem(new GUIContent(infoExpanded? "Collapse Info" : "Expand Info"), false, ()=> { infoExpanded = !infoExpanded; });
				menu.AddItem(new GUIContent(isActive? "Disable" : "Enable"), false, ()=> { isActive = !isActive; });
				
				var assignable = this as ITaskAssignable;
				if (assignable != null){
					
					if (assignable.task != null){
						menu.AddItem(new GUIContent("Copy Assigned Condition"), false, ()=> { CopyBuffer.Set<Task>(assignable.task); });
					} else {
						menu.AddDisabledItem(new GUIContent("Copy Assigned Condition"));
					}

					if (CopyBuffer.Has<Task>()){
						menu.AddItem(new GUIContent(string.Format("Paste Assigned Condition ({0})", CopyBuffer.Peek<Task>().name)), false, ()=>
						{
							if (assignable.task == CopyBuffer.Peek<Task>()){
								return;
							}

							if (assignable.task != null){
								if (!EditorUtility.DisplayDialog("Paste Condition", string.Format("Connection already has a Condition assigned '{0}'. Replace assigned condition with pasted condition '{1}'?", assignable.task.name, CopyBuffer.Peek<Task>().name), "YES", "NO")){
									return;
								}
							}

							try {assignable.task = CopyBuffer.Get<Task>().Duplicate(graph);}
							catch {Logger.LogWarning("Can't paste Condition here. Incombatible Types.", "Editor", this);}
						});

					} else {
						menu.AddDisabledItem(new GUIContent("Paste Assigned Condition"));
					}

				}

				menu.AddSeparator("/");
				menu.AddItem(new GUIContent("Delete"), false, ()=> { graph.RemoveConnection(this); });

				GraphEditorUtility.PostGUI += ()=> { menu.ShowAsContext(); };
				e.Use();
			}
		}




		void UpdateBlinkStatus(Vector2 lineFrom, Vector2 lineTo){
			if (status != lastStatus){
				lastStatus = status;
				statusChangeTime = Time.time;
			}

			var deltaTimeSinceChange = (Time.time - statusChangeTime);
			if (status != Status.Resting || size != defaultSize){
				size = Mathf.Lerp(defaultSize + STATUS_BLINK_SIZE_ADD, defaultSize, deltaTimeSinceChange / STATUS_BLINK_DURATION);
			}

			if (status != Status.Resting || size == defaultSize){
				color = status == Status.Resting? defaultColor : Node.statusColors[status];
			}

			if (NCPrefs.connectionStyle != NCPrefs.ConnectionStyle.Stepped && status == Status.Running){
				var packetTraversal = deltaTimeSinceChange * STATUS_BLINK_PACKET_SPEED;
				for (var i = 0f; i < STATUS_BLINK_PACKET_COUNT; i++){
					var progression = packetTraversal + (i/STATUS_BLINK_PACKET_COUNT);
					var normPos = Mathf.Repeat(progression, 1f);

					var packetColor = this.color;
					var pingPong = Mathf.PingPong(normPos, 0.5f);
					var norm = (pingPong * 2) / 0.5f;
					var pSize = Mathf.Lerp(0.5f, 1f, norm) * STATUS_BLINK_PACKET_SIZE;
					packetColor.a = norm * ( deltaTimeSinceChange / (STATUS_BLINK_DURATION + 0.25f) );

					var rect = new Rect(0, 0, pSize, pSize);
					rect.center = GetPosAlongConnectionCurve(lineFrom, lineTo, lineFromTangent, lineToTangent, normPos);;
					GUI.color = packetColor;
					GUI.Box(rect, string.Empty, CanvasStyles.circle);
					GUI.color = Color.white;
				}
			}
		}
	}
}

#endif