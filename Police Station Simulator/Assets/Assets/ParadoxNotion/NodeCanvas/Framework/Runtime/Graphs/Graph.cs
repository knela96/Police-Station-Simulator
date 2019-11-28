using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NodeCanvas.Framework.Internal;
using ParadoxNotion;
using ParadoxNotion.Serialization;
using ParadoxNotion.Serialization.FullSerializer;
using ParadoxNotion.Services;
using UnityEngine;
using Logger = ParadoxNotion.Services.Logger;

namespace NodeCanvas.Framework{

	///This is the base and main class of NodeCanvas and graphs. All graph System are deriving from this.
	[System.Serializable]
	abstract public partial class Graph : ScriptableObject, ITaskSystem, ISerializationCallbackReceiver {

		[SerializeField]
		private string _serializedGraph;
		[SerializeField]
		private List<Object> _objectReferences;
		[SerializeField]
		private bool _deserializationFailed = false;

		[System.NonSerialized]
		private bool hasDeserialized = false;

		#if UNITY_EDITOR
		///Editor Only. Invoked after graph serialization.
		public static event System.Action<Graph> onGraphSerialized;
		#endif


		//These are the data that are serialized and deserialized into/from a 'GraphSerializationData' object
		///----------------------------------------------------------------------------------------------
		private string _category                  = string.Empty;
		private string _comments                  = string.Empty;
		private Vector2 _translation              = new Vector2(-5000, -5000);
		private float _zoomFactor                 = 1f;
		private List<Node> _nodes                 = new List<Node>();
		private Node _primeNode                   = null;
		private List<CanvasGroup> _canvasGroups   = null;
		private BlackboardSource _localBlackboard = null;
		///----------------------------------------------------------------------------------------------

		///----------------------------------------------------------------------------------------------
		void ISerializationCallbackReceiver.OnBeforeSerialize(){ Serialize(); }
		void ISerializationCallbackReceiver.OnAfterDeserialize(){ Deserialize(); }
		///----------------------------------------------------------------------------------------------

		///----------------------------------------------------------------------------------------------
		protected void OnEnable(){ Validate(); }
		protected void OnDisable(){}
		protected void OnDestroy(){}
		protected void OnValidate(){}
		///----------------------------------------------------------------------------------------------

		///Serialize the Graph
		public void Serialize(){
			#if UNITY_EDITOR //we only serialize in the editor

			if (JSONSerializer.applicationPlaying){
				return;
			}

			if (_objectReferences != null && _objectReferences.Count > 0 && _objectReferences.Any(o => o != null)){
				hasDeserialized = false;
			}
			
			_serializedGraph = this.Serialize(false, _objectReferences);

			//notify owner. This is used for bound graphs
			var owner = agent != null && agent is GraphOwner? (GraphOwner)agent : null;
			if (owner != null && owner.graph == this){
				owner.OnAfterGraphSerialized(this);
			}

			//raise event.
			if (onGraphSerialized != null){
				try { onGraphSerialized(this); }
				catch (System.Exception e){ Logger.LogException(e, "Serialization", this); }
			}

			#endif
		}

		///Deserialize the Graph
		public void Deserialize(){
			if (hasDeserialized && JSONSerializer.applicationPlaying){
				return;
			}
			hasDeserialized = true;
			this.Deserialize(_serializedGraph, false, _objectReferences);
		}

		///----------------------------------------------------------------------------------------------

		///Serialize the graph and returns the serialized json string
		public string Serialize(bool pretyJson, List<UnityEngine.Object> objectReferences){
			//if something went wrong on deserialization, dont serialize back, but rather keep what we had
			if (_deserializationFailed){
				_deserializationFailed = false;
				return _serializedGraph;
			}
			
			//the list to save the references in. If not provided explicitely we save into the local list
			if (objectReferences == null){
				objectReferences = this._objectReferences = new List<Object>();
			}

			UpdateNodeIDs(true);
			return JSONSerializer.Serialize(typeof(GraphSerializationData), new GraphSerializationData(this), pretyJson, objectReferences);
		}

		///Deserialize the json serialized graph provided. Returns the data or null if failed.
		public GraphSerializationData Deserialize(string serializedGraph, bool validate, List<UnityEngine.Object> objectReferences){
			if (string.IsNullOrEmpty(serializedGraph)){
				return null;
			}

			//the list to load the references from. If not provided explicitely we load from the local list (which is the case most times)
			if (objectReferences == null){
				objectReferences = this._objectReferences;
			}

			try
			{
				//deserialize provided serialized graph into a new GraphSerializationData object and load it
				var data = JSONSerializer.Deserialize<GraphSerializationData>(serializedGraph, objectReferences);
				if (LoadGraphData(data, validate) == true){
					this._deserializationFailed = false;
					this._serializedGraph = serializedGraph;
					this._objectReferences = objectReferences;
					return data;
				}

				_deserializationFailed = true;
				return null;
			}
			catch (System.Exception e)
			{
                Logger.LogException(e, "NodeCanvas", this);
				_deserializationFailed = true;
				return null;
			}
		}

		///Loads the GraphSerializationData
		bool LoadGraphData(GraphSerializationData data, bool validate){

			if (data == null){
                Logger.LogError("Can't Load graph, cause of null GraphSerializationData provided", "Serialization", this);
				return false;
			}

			if (data.type != this.GetType()){
                Logger.LogError("Can't Load graph, cause of different Graph type serialized and required", "Serialization", this);
				return false;
			}

			data.Reconstruct(this);

			//grab the final data and set fields directly
			this._category        = data.category;
			this._comments        = data.comments;
			this._translation     = data.translation;
			this._zoomFactor      = data.zoomFactor;
			this._nodes           = data.nodes;
			this._primeNode       = data.primeNode;
			this._canvasGroups    = data.canvasGroups;
			this._localBlackboard = data.localBlackboard;

			//IMPORTANT: Validate should be called in all deserialize cases outside of Unity's 'OnAfterDeserialize',
			//like for example when loading from json, or manualy calling this outside of OnAfterDeserialize.
			if (validate){
				Validate();
			}

			return true;
		}

		///Graph can override this for derived data serialization if needed
		virtual public object OnDerivedDataSerialization(){return null;}
		///Graph can override this for derived data deserialization if needed
		virtual public void OnDerivedDataDeserialization(object data){}

		///Gets the json string along with the list of UnityObject references used for serialization by this graph.
		public void GetSerializationData(out string json, out List<UnityEngine.Object> references){
			json = _serializedGraph;
			references = _objectReferences != null? new List<UnityEngine.Object>(_objectReferences) : null;
		}

		///Sets the target list of UnityObject references. This is for internal use and highly not recomended to do.
		public void SetSerializationObjectReferences(List<UnityEngine.Object> references){
			this._objectReferences = references;
		}

		///Clones the graph and returns the new one. Currently exactly the same as Instantiate, but could change in the future
		public static T Clone<T>(T graph) where T:Graph{
			var newGraph = (T)Instantiate(graph);
			newGraph.name = newGraph.name.Replace("(Clone)", "");
			return (T)newGraph;
		}

		///Serialize the local blackboard of the graph alone
		public string SerializeLocalBlackboard(){
			return JSONSerializer.Serialize(typeof(BlackboardSource), _localBlackboard, false, _objectReferences);
		}

		///Deserialize the local blackboard of the graph alone
		public bool DeserializeLocalBlackboard(string json){
			try
			{
				_localBlackboard = JSONSerializer.Deserialize<BlackboardSource>(json, _objectReferences);
				if (_localBlackboard == null) _localBlackboard = new BlackboardSource();
				return true;
			}
			catch (System.Exception e)
			{
                Logger.LogException(e, "Serialization", this);
				return false;
			}
		}
		
		///Non-editor CopySerialized
		public void CopySerialized(Graph target){
			var json = this.Serialize(false, target._objectReferences);
			target.Deserialize( json, true, this._objectReferences );
		}

		///Validate the graph and it's nodes. Also called from OnEnable callback.
		public void Validate(){

			#if UNITY_EDITOR
			if (!Application.isPlaying && !UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode){
				initialReferencesUpdated = false; //force update references
				UpdateReferences(this.agent, this.blackboard);
			}
			#endif

			for (var i = 0; i < allNodes.Count; i++){
				try { allNodes[i].OnValidate(this); } //validation could be critical. we always continue
				catch (System.Exception e) { Logger.LogException(e, "Validation", allNodes[i]); continue; }
			}

			var allTasks = GetAllTasksOfType<Task>();
			for (var i = 0; i < allTasks.Count; i++){
				try { allTasks[i].OnValidate(this); } //validation could be critical. we always continue
				catch (System.Exception e) { Logger.LogException(e, "Validation", allTasks[i]); continue; }
			}
			
			OnGraphValidate();

			//in runtime and if graph uses local blackboard, initialize property/field binding.
			//'null' target gameObject (since localBlackboard can only have static bindings).
			//'false' so that setter is not called.
			if (Application.isPlaying && useLocalBlackboard){
				localBlackboard.InitializePropertiesBinding(null, false);
			}
		}

		///Use this for derived graph Validation
		virtual protected void OnGraphValidate(){}

		///----------------------------------------------------------------------------------------------

		///Raised when the graph is Stoped/Finished if it was Started at all.
		///Important: After the graph is stopped, the OnFinished event is cleared from all subscribers!
		public event System.Action<bool> onFinish;

		[System.NonSerialized]
		private Component _agent;
		[System.NonSerialized]
		private IBlackboard _blackboard;

		[System.NonSerialized]
		private static List<Graph> runningGraphs = new List<Graph>();
		[System.NonSerialized]
		private float timeStarted;
		[System.NonSerialized]
		private bool initialReferencesUpdated;
		[System.NonSerialized]
		private bool _isAutoUpdated;
		[System.NonSerialized]
		private bool _isRunning;
		[System.NonSerialized]
		private bool _isPaused;

		///----------------------------------------------------------------------------------------------

		///The base type of all nodes that can live in this system
		abstract public System.Type baseNodeType{get;}
		///Is this system allowed to start with a null agent?
		abstract public bool requiresAgent{get;}
		///Does the system needs a prime Node to be set for it to start?
		abstract public bool requiresPrimeNode{get;}
		///Should the the nodes be auto sorted on position x?
		abstract public bool autoSort{get;}
		///Force use the local blackboard vs propagated one?
		abstract public bool useLocalBlackboard{get;}
		//Whether the graph can accept variables Drag&Drop
		abstract public bool canAcceptVariableDrops{get;}

		///----------------------------------------------------------------------------------------------

		///The friendly title name of the graph
		new public string name{
			get {return base.name;}
			set {base.name = value;}
		}

		///Graph category
		public string category{
			get {return _category;}
			set {_category = value;}
		}

		///Graph Comments
		public string comments{
			get {return _comments;}
			set {_comments = value;}
		}
		
		///The time in seconds this graph is running
		public float elapsedTime{
			get {return isRunning || isPaused? Time.time - timeStarted : 0;}
		}

		///Is the graph running?
		public bool isRunning {
			get {return _isRunning;}
			private set {_isRunning = value;}
		}

		///Is the graph paused?
		public bool isPaused {
			get {return _isPaused;}
			private set {_isPaused = value;}
		}

		///Is the graph started with autoUpdate?
		public bool isAutoUpdated{
			get {return _isAutoUpdated;}
			private set {_isAutoUpdated = value;}
		}

		///All nodes assigned to this system
		public List<Node> allNodes{
			get {return _nodes;}
			private set {_nodes = value;}
		}

		///The node to execute first. aka 'START'
		public Node primeNode{
			get {return _primeNode;}
			set
			{
				if (_primeNode != value){

					if (value != null && value.allowAsPrime == false){
						return;
					}
					
					if (isRunning){
						if (_primeNode != null)	_primeNode.Reset();
						if (value != null) value.Reset();
					}

					RecordUndo("Set Start");
					_primeNode = value;
					UpdateNodeIDs(true);
				}
			}
		}

		///The canvas groups of the graph (Editor purposes)
		public List<CanvasGroup> canvasGroups{
			get {return _canvasGroups;}
			set {_canvasGroups = value;}
		}

		///The translation of the graph in the total canvas (Editor purposes)
		public Vector2 translation{
			get {return _translation;}
			set {_translation = value;}
		}

		///The zoom of the graph (Editor purposes)
		public float zoomFactor{
			get {return _zoomFactor;}
			set {_zoomFactor = value;}
		}

		///The agent currently assigned to the graph
		public Component agent{
			get {return _agent;}
			private set {_agent = value;}
		}

		///The blackboard currently assigned to the graph
		public IBlackboard blackboard{
			get
			{
				if (useLocalBlackboard){ return localBlackboard; }
				#if UNITY_EDITOR
				//done for when user removes bb component in editor.
				if (_blackboard == null || _blackboard.Equals(null)){
					return null;
				}
				#endif

				return _blackboard;
			}
			private set
			{
				if (_blackboard != value){
					if (isRunning){ return; }
					if (useLocalBlackboard){ return; }
					_blackboard = value;
				}
			}
		}

		///The local blackboard of the graph
		public BlackboardSource localBlackboard{
			get
			{
				if (ReferenceEquals(_localBlackboard, null)){
					_localBlackboard = new BlackboardSource();
					_localBlackboard.name = "Local Blackboard";
				}
				return _localBlackboard;
			}
		}

		///The UnityObject of the ITaskSystem. In this case the graph itself
		UnityEngine.Object ITaskSystem.contextObject{
			get {return this;}
		}

		///----------------------------------------------------------------------------------------------

		///Makes a copy of provided nodes and if targetGraph is provided, pastes the new nodes in that graph.
		public static List<Node> CloneNodes(List<Node> originalNodes, Graph targetGraph = null, Vector2 originPosition = default(Vector2)){

			if (targetGraph != null){
				if (originalNodes.Any(n => n.GetType().IsSubclassOf(targetGraph.baseNodeType) == false )){
					return null;
				}
			}

			var newNodes = new List<Node>();
			var linkInfo = new Dictionary<Connection, KeyValuePair<int, int>>();

			//duplicate all nodes first
			foreach (var original in originalNodes){
				Node newNode = null;
				if (targetGraph != null){
					newNode = original.Duplicate(targetGraph);
					if (targetGraph != original.graph && original.graph != null && original == original.graph.primeNode){
						targetGraph.primeNode = newNode;
					}
				} else {
					newNode = JSONSerializer.Clone<Node>(original);
				}
				newNodes.Add(newNode);					

				//store the out connections that need dulpicate along with the indeces of source and target
				foreach (var c in original.outConnections){
					var sourceIndex = originalNodes.IndexOf(c.sourceNode);
					var targetIndex = originalNodes.IndexOf(c.targetNode);
					linkInfo[c] = new KeyValuePair<int, int>(sourceIndex, targetIndex);
				}
			}

			//duplicate all connections that we stored as 'need duplicating' providing new source and target
			foreach (var linkPair in linkInfo){
				if (linkPair.Value.Value != -1){ //we check this to see if the target node is part of the duplicated nodes since IndexOf returns -1 if element is not part of the list
					var newSource = newNodes[ linkPair.Value.Key ];
					var newTarget = newNodes[ linkPair.Value.Value ];
					if (targetGraph != null){
						linkPair.Key.Duplicate(newSource, newTarget);
					} else {
						var newConnection = JSONSerializer.Clone<Connection>(linkPair.Key);
						newConnection.SetSource(newSource, false);
						newConnection.SetTarget(newTarget, false);
					}
				}
			}

			if (originPosition != default(Vector2) && newNodes.Count > 0){
				if (newNodes.Count == 1){
					newNodes[0].position = originPosition;
				} else {
					var diff = newNodes[0].position - originPosition;
					newNodes[0].position = originPosition;
					for (var i = 1; i < newNodes.Count; i++){
						newNodes[i].position -= diff;
					}					
				}
			}

			if (targetGraph != null){
				//revalidate all new nodes in their new graph
				for (var i = 0; i < newNodes.Count; i++){
					newNodes[i].OnValidate(targetGraph);
				}
			}

			return newNodes;
		}

		///Update the Agent/Component references: Setting the system to the tasks and blackboard to BBParameters.
		///This is done when the graph starts and in the editor for convenience.
		public void UpdateReferences(Component newAgent, IBlackboard newBlackboard){
			//initial references is used so that at least once even if agent and bb are null, calls are made.
			if (this.agent != newAgent || this.blackboard != newBlackboard || !initialReferencesUpdated){
				initialReferencesUpdated = true;
				this.agent = newAgent;
				this.blackboard = newBlackboard;
				UpdateNodeBBFields();
				SendTaskOwnerDefaults();
			}
		}
		
		///See UpdateReferences
		public void UpdateReferencesFromOwner(GraphOwner owner){
			UpdateReferences(owner, owner != null? owner.blackboard : null);
		}

		///Update all graph node's BBFields for current Blackboard.
		void UpdateNodeBBFields(){
			for (var i = 0; i < allNodes.Count; i++){
				BBParameter.SetBBFields(allNodes[i], blackboard);
			}
		}

		///Sets all graph Tasks' owner system (which is this graph).
		public void SendTaskOwnerDefaults(){
			var tasks = GetAllTasksOfType<Task>();
			for (var i = 0; i < tasks.Count; i++){
				tasks[i].SetOwnerSystem(this);
			}
		}

		///Update the IDs of the nodes in the graph. Is automatically called whenever a change happens in the graph by the adding, removing, connecting etc.
		public void UpdateNodeIDs(bool alsoReorderList){

			var lastID = -1;

			//start from prime
			if (primeNode != null){
				lastID = primeNode.AssignIDToGraph(lastID);
			}

			//set the rest starting from nodes without parent(s)
			var tempList = allNodes.OrderBy(n => n.inConnections.Count != 0).OrderBy(n => n.priority * -1).ToList();
			for (var i = 0; i < tempList.Count; i++){
				lastID = tempList[i].AssignIDToGraph(lastID);
			}

			//reset the check
			for (var i = 0; i < allNodes.Count; i++){
				allNodes[i].ResetRecursion();
			}

			if (alsoReorderList){
				allNodes = allNodes.OrderBy(node => node.ID).ToList();
			}
		}


		///Start the graph for the agent and blackboard provided.
		///Optionally provide a callback for when the graph stops or ends
		public void StartGraph(Component agent, IBlackboard blackboard, bool autoUpdate, System.Action<bool> callback = null){

			#if UNITY_EDITOR //prevent the user to accidentaly start the graph while its an asset. At least in the editor
			if (UnityEditor.EditorUtility.IsPersistent(this)){
                Logger.LogError("You have tried to start a graph which is an asset, not an instance! You should Instantiate the graph first.", "NodeCanvas", this);
				return;
			}
			#endif

			if (isRunning){
				if (callback != null){
					onFinish += callback;
				}
                Logger.LogWarning("Graph is already Active.", "NodeCanvas", this);
				return;
			}

			if (agent == null && requiresAgent){
                Logger.LogWarning("You've tried to start a graph with null Agent.", "NodeCanvas", this);
				return;
			}

			if (primeNode == null && requiresPrimeNode){
                Logger.LogWarning("You've tried to start graph without a 'Start' node.", "NodeCanvas", this);
				return;
			}
			
			if (blackboard == null){
				if (agent != null){
                    Logger.Log("Graph started without blackboard. Looking for blackboard on agent '" + agent.gameObject + "'...", "NodeCanvas", this);
					blackboard = agent.GetComponent(typeof(IBlackboard)) as IBlackboard;
				}
				if (blackboard == null){
                    Logger.LogWarning("Started with null Blackboard. Using Local Blackboard instead.", "NodeCanvas", this);
					_blackboard = localBlackboard;
				}
			}

			UpdateReferences(agent, blackboard);

			if (callback != null){
				this.onFinish = callback;
			}

			isRunning = true;

			runningGraphs.Add(this);

			if (!isPaused){
				timeStarted = Time.time;
				OnGraphStarted();
			} else {
				OnGraphUnpaused();
			}

			for (var i = 0; i < allNodes.Count; i++){
				if (!isPaused){
					allNodes[i].OnGraphStarted();
				} else {
					allNodes[i].OnGraphUnpaused();
				}
			}

			isPaused = false;

			isAutoUpdated = autoUpdate;
			if (autoUpdate){
				MonoManager.current.onUpdate += UpdateGraph;
				UpdateGraph();
			}
		}

		///Stops the graph completely and resets all nodes.
		public void Stop(bool success = true){

			if (!isRunning && !isPaused){
				return;
			}

			runningGraphs.Remove(this);
			if (isAutoUpdated){
				MonoManager.current.onUpdate -= UpdateGraph;
			}

			isRunning = false;
			isPaused = false;

			for (var i = 0; i < allNodes.Count; i++){
				allNodes[i].Reset(false);
				allNodes[i].OnGraphStoped();
			}

			OnGraphStoped();

			if (onFinish != null){
				onFinish(success);
				onFinish = null;
			}
		}

		///Pauses the graph from updating as well as notifying all nodes.
		public void Pause(){

			if (!isRunning){
				return;
			}

			runningGraphs.Remove(this);
			if (isAutoUpdated){
				MonoManager.current.onUpdate -= UpdateGraph;
			}
			
			isRunning = false;
			isPaused = true;

			for (var i = 0; i < allNodes.Count; i++){
				allNodes[i].OnGraphPaused();
			}
			
			OnGraphPaused();
		}

		///Updates the graph. Normaly this is updated by MonoManager since at StartGraph, this method is registered for updating.
		public void UpdateGraph(){
			// UnityEngine.Profiling.Profiler.BeginSample(string.Format("Graph ({0})", agent != null? agent.name : this.name) );
			if (isRunning){
				OnGraphUpdate();
			}
			// UnityEngine.Profiling.Profiler.EndSample();
		}

		///Override for graph specific stuff to run when the graph is started
		virtual protected void OnGraphStarted(){}
		///Override for graph specific per frame logic. Called every frame if the graph is running
		virtual protected void OnGraphUpdate(){}
		///Override for graph specific stuff to run when the graph is stoped
		virtual protected void OnGraphStoped(){}
		///Override this for when the graph is paused
		virtual protected void OnGraphPaused(){}
		///Override for graph stuff to run when the graph is unpause
		virtual protected void OnGraphUnpaused(){}


		///Sends an OnCustomEvent message to the tasks that needs them. Tasks subscribe to events using [EventReceiver] attribute.
		public void SendEvent(string name){SendEvent(new EventData(name));}
		public void SendEvent<T>(string name, T value){SendEvent(new EventData<T>(name, value));}
		public void SendEvent(EventData eventData){
			if (!isRunning || eventData == null || agent == null){
				return;
			}

			#if UNITY_EDITOR
			if (NodeCanvas.Editor.NCPrefs.logEvents){
				Logger.Log(string.Format("Event '{0}' Send to '{1}'", eventData.name, agent.gameObject.name), "Event", agent);
			}
			#endif

			var router = agent.GetComponent<MessageRouter>();
			if (router != null){
				router.Dispatch("OnCustomEvent", eventData);
				router.Dispatch(eventData.name, eventData.value);
			}
			//if router is null, it means that nothing has subscribed to any event, thus we dont care.
		}

		///Sends an event to all Running graphs
		public static void SendGlobalEvent(string name){ SendGlobalEvent(new EventData(name)); }
		public static void SendGlobalEvent<T>(string name, T value){ SendGlobalEvent(new EventData<T>(name, value)); }
		public static void SendGlobalEvent(EventData eventData){
			var send = new List<GameObject>();
			foreach(var graph in runningGraphs.ToArray()){ //ToArray because an event may result in a graph stopping and thus messing with the enumeration.
				if (graph.agent != null && !send.Contains(graph.agent.gameObject)){
					send.Add(graph.agent.gameObject);
					graph.SendEvent(eventData);
				}
			}
		}

		///----------------------------------------------------------------------------------------------

		///Get a node by it's ID, null if not found. The ID should always be the same as the node's index in allNodes list.
		public Node GetNodeWithID(int searchID){
			if (searchID < allNodes.Count && searchID >= 0){
				return allNodes.Find(n => n.ID == searchID);
			}
			return null;
		}

		///Get all nodes of a specific type
		public List<T> GetAllNodesOfType<T>() where T:Node{
			return allNodes.OfType<T>().ToList();
		}

		///Get a node by it's tag name
		public T GetNodeWithTag<T>(string tagName) where T:Node{
			foreach (var node in allNodes.OfType<T>()){
				if (node.tag == tagName){
					return node;
				}
			}
			return null;
		}

		///Get all nodes taged with such tag name
		public List<T> GetNodesWithTag<T>(string tagName) where T:Node{
			var nodes = new List<T>();
			foreach (var node in allNodes.OfType<T>()){
				if (node.tag == tagName){
					nodes.Add(node);
				}
			}
			return nodes;
		}

		///Get all taged nodes regardless tag name
		public List<T> GetAllTagedNodes<T>() where T:Node{
			var nodes = new List<T>();
			foreach (var node in allNodes.OfType<T>()){
				if (!string.IsNullOrEmpty(node.tag)){
					nodes.Add(node);
				}
			}
			return nodes;
		}

		///Get all nodes of the graph that have no incomming connections
		public List<Node> GetRootNodes(){
			return allNodes.Where(n => n.inConnections.Count == 0).ToList();
		}

		///Get all nodes of the graph that have no outgoing connections
		public List<Node> GetLeafNodes(){
			return allNodes.Where(n => n.outConnections.Count == 0).ToList();
		}

		///Get all Nested graphs of this graph
		public List<T> GetAllNestedGraphs<T>(bool recursive) where T:Graph{
			var graphs = new List<T>();
			foreach (var node in allNodes.OfType<IGraphAssignable>()){
				if (node.nestedGraph is T){
					if (!graphs.Contains((T)node.nestedGraph)){
						graphs.Add( (T)node.nestedGraph );
					}
					if (recursive){
						graphs.AddRange( node.nestedGraph.GetAllNestedGraphs<T>(recursive) );
					}
				}
			}
			return graphs;
		}

		///Get all runtime instanced Nested graphs of this graph and it's sub-graphs
		public List<Graph> GetAllInstancedNestedGraphs(){
			var instances = new List<Graph>();
			foreach (var node in allNodes.OfType<IGraphAssignable>()){
				var subInstances = node.GetInstances();
				instances.AddRange( subInstances );
				foreach(var subInstance in subInstances){
					instances.AddRange( subInstance.GetAllInstancedNestedGraphs() );
				}
			}
			return instances;
		}

		///----------------------------------------------------------------------------------------------

		///Given an object returns the relevant graph if any.
		public static Graph GetElementGraph(object obj){
			if (obj is GraphOwner){ return (obj as GraphOwner).graph; }
			if (obj is Graph){ return (Graph)obj; }
			if (obj is Node){ return (obj as Node).graph; }
			if (obj is Connection){ return (obj as Connection).graph; }
			if (obj is Task){ return (obj as Task).ownerSystem as Graph; }
			return null;			
		}

		///Get ALL Tasks of type T in the graph (including tasks assigned on Nodes and on Connections and other Tasks that implement ISubTasksContainer).
		///TODO: Refactor to use VirtualGraphStructure, but that first needs to be made fast and/or cached.
		public List<T> GetAllTasksOfType<T>() where T:Task{
			var tempTasks = new List<Task>();
			var resultTasks = new List<T>();
			for (var i = 0; i < allNodes.Count; i++){
				GetObjectTasks<Task>(allNodes[i], ref tempTasks);
				for (var j = 0; j < allNodes[i].outConnections.Count; j++){
					GetObjectTasks<Task>(allNodes[i].outConnections[j], ref tempTasks);
				}
			}

			for (var i = 0; i < tempTasks.Count; i++){
				if (tempTasks[i] is T){ resultTasks.Add( (T)tempTasks[i] ); }
				GetObjectTasks<T>(tempTasks[i], ref resultTasks);
			}

			return resultTasks;
		}

		///Utility to get tasks from an object. Fills ref list provided.
		void GetObjectTasks<T>(object obj, ref List<T> tasks) where T:Task{
			if (obj is ITaskAssignable && (obj as ITaskAssignable).task is T){
				tasks.Add( (T)(obj as ITaskAssignable).task );
			}
			if (obj is ISubTasksContainer){
				tasks.AddRange( (obj as ISubTasksContainer).GetSubTasks().OfType<T>() );
			}
		}

		///----------------------------------------------------------------------------------------------

		///Returns a structure of the graphs that includes Nodes, Connections, Tasks and BBParameters,
		///but with nodes elements all being root to the graph (instead of respective parent connections).
		virtual public Hierarchy.Element GetFlatGraphHierarchy(){
			var root = new Hierarchy.Element(this);
			int lastID = 0;
			for (var i = 0; i < allNodes.Count; i++){
				root.AddChild( GetTreeNodeElement(allNodes[i], false, ref lastID) );
			}
			return root;
		}
		
		///Returns a structure of the graphs that includes Nodes, Connections, Tasks and BBParameters,
		///but where node elements are parent to their respetive connections. Only possible for tree-like graphs.
		public Hierarchy.Element GetFullGraphHierarchy(){
			var root = new Hierarchy.Element(this);
			int lastID = 0;
			if (primeNode != null){
				root.AddChild( GetTreeNodeElement(primeNode, true, ref lastID) );
			}
			for (var i = 0; i < allNodes.Count; i++){
				var node = allNodes[i];
				if (node.ID > lastID && node.inConnections.Count == 0){
					root.AddChild( GetTreeNodeElement(node, true, ref lastID) );
				}
			}
			return root;
		}

		///Used above. Returns a node hierarchy element optionaly along with all it's children recursively
		Hierarchy.Element GetTreeNodeElement(Node node, bool recurse, ref int lastID){
			var nodeElement = GetTaskAndParametersStructureInTarget( node );
			for (var i = 0; i < node.outConnections.Count; i++){
				var connectionElement = GetTaskAndParametersStructureInTarget( node.outConnections[i] );
				nodeElement.AddChild( connectionElement );
				if (recurse){
					var targetNode = node.outConnections[i].targetNode;
					if (targetNode.ID > node.ID){ //ensure no recursion loop
						connectionElement.AddChild( GetTreeNodeElement(targetNode, recurse, ref lastID) );
					}
				}
			}
			lastID = node.ID;
			return nodeElement;
		}

		///Returns an element that includes tasks and parameters for target object recursively
		public static Hierarchy.Element GetTaskAndParametersStructureInTarget(object obj){
			var parentElement = new Hierarchy.Element(obj);
			var resultObjects = new List<object>();
			if (obj is ITaskAssignable && (obj as ITaskAssignable).task != null){
				resultObjects.Add( (obj as ITaskAssignable).task );
			}
			if (obj is ISubTasksContainer){
				var subs = (obj as ISubTasksContainer).GetSubTasks();
				if (subs != null){ resultObjects.AddRange( subs ); }
			}

			//this also handles ISubParametersContainer
			resultObjects.AddRange( BBParameter.GetObjectBBParameters(obj).ToArray() );

			//recurse
			for (var i = 0; i < resultObjects.Count; i++){
				parentElement.AddChild( GetTaskAndParametersStructureInTarget( resultObjects[i]) );
			}

			return parentElement;
		}

		///----------------------------------------------------------------------------------------------

		///Get the parent graph element (node/connection) from target Task.
		///SLOW
		public IGraphElement GetTaskParentElement(Task targetTask){
			var rootElement = GetFlatGraphHierarchy();
			var targetElement = rootElement.FindReferenceElement(targetTask);
			return targetElement != null? targetElement.GetFirstParentReferenceOfType<IGraphElement>() : null;
		}

		///Get the parent graph element (node/connection) from target BBParameter
		///SLOW
		public IGraphElement GetParameterParentElement(BBParameter targetParameter){
			var rootElement = GetFlatGraphHierarchy();
			var targetElement = rootElement.FindReferenceElement(targetParameter);
			return targetElement != null? targetElement.GetFirstParentReferenceOfType<IGraphElement>() : null;
		}

		///Get all Tasks found in target
		///SLOW
		public static Task[] GetTasksInElement(IGraphElement target){
			var rootElement = GetTaskAndParametersStructureInTarget(target);
			return rootElement.GetAllChildrenReferencesOfType<Task>().ToArray();
		}

		///Get all BBParameters found in target
		///SLOW
		public static BBParameter[] GetParametersInElement(IGraphElement target){
			var rootElement = GetTaskAndParametersStructureInTarget(target);
			return rootElement.GetAllChildrenReferencesOfType<BBParameter>().ToArray();
		}

		///Returns all defined BBParameter names found in graph
		///SLOW
		public BBParameter[] GetDefinedParameters(){
			var rootElement = GetFlatGraphHierarchy();
			return rootElement.GetAllChildrenReferencesOfType<BBParameter>()
			.Where(p => p != null && p.isDefined)
			.ToArray();
		}

		///Utility function to create all defined parameters of this graph as variables into the provided blackboard.
		///SLOW
		public void PromoteDefinedParametersToVariables(IBlackboard bb){
			foreach (var bbParam in GetDefinedParameters()){
				bbParam.PromoteToVariable(bb);
			}
		}

		///----------------------------------------------------------------------------------------------

		///Add a new node to this graph
		public T AddNode<T>() where T : Node{
			return (T)AddNode(typeof(T));
		}

		public T AddNode<T>(Vector2 pos) where T : Node{
			return (T)AddNode(typeof(T), pos);
		}

		public Node AddNode(System.Type nodeType){
			return AddNode(nodeType, new Vector2(-translation.x + 100, -translation.y + 100));
		}

		///Add a new node to this graph
		public Node AddNode(System.Type nodeType, Vector2 pos){
			
			if (nodeType.IsGenericTypeDefinition){
				nodeType = nodeType.RTMakeGenericType(nodeType.GetFirstGenericParameterConstraintType());
			}

			if (!nodeType.RTIsSubclassOf(baseNodeType)){
                Logger.LogWarning(nodeType + " can't be added to " + this.GetType().FriendlyName() + " graph.", "NodeCanvas", this);
				return null;
			}

			var newNode = Node.Create(this, nodeType, pos);

			RecordUndo("New Node");

			allNodes.Add(newNode);

			if (primeNode == null){
				primeNode = newNode;
			}
		
			UpdateNodeIDs(false);
			return newNode;
		}

		///Disconnects and then removes a node from this graph
		public void RemoveNode(Node node, bool recordUndo = true, bool force = false){
 
			if (!force && node.GetType().RTIsDefined<ParadoxNotion.Design.ProtectedSingletonAttribute>(true)){
				if (allNodes.Where(n => n.GetType() == node.GetType()).ToArray().Length == 1){
					return;
				}
			}

			if (!allNodes.Contains(node)){
                Logger.LogWarning("Node is not part of this graph.", "NodeCanvas", this);
				return;
			}

			#if UNITY_EDITOR
			if (!Application.isPlaying){
				//auto reconnect parent & child of deleted node. Just a workflow convenience
				if (autoSort && node.inConnections.Count == 1 && node.outConnections.Count == 1){
					var relinkNode = node.outConnections[0].targetNode;
					if (relinkNode != node.inConnections[0].sourceNode){
						RemoveConnection(node.outConnections[0]);
						node.inConnections[0].SetTarget(relinkNode);
					}
				}
			}

			NodeCanvas.Editor.GraphEditorUtility.activeElement = null;
			#endif

			//callback
			node.OnDestroy();

			//disconnect parents
			foreach (var inConnection in node.inConnections.ToArray()){
				RemoveConnection(inConnection);
			}

			//disconnect children
			foreach (var outConnection in node.outConnections.ToArray()){
				RemoveConnection(outConnection);
			}

			if (recordUndo){
				RecordUndo("Delete Node");
			}

			allNodes.Remove(node);

			if (node == primeNode){
				primeNode = GetNodeWithID( primeNode.ID );
			}

			UpdateNodeIDs(false);
		}
		
		///Connect two nodes together to the next available port of the source node
		public Connection ConnectNodes(Node sourceNode, Node targetNode){
			return ConnectNodes(sourceNode, targetNode, sourceNode.outConnections.Count);
		}

		///Connect two nodes together to a specific port index of the source node
		public Connection ConnectNodes(Node sourceNode, Node targetNode, int indexToInsert){

			if (targetNode.IsNewConnectionAllowed(sourceNode) == false){
				return null;
			}

			RecordUndo("New Connection");

			var newConnection = Connection.Create(sourceNode, targetNode, indexToInsert);
			sourceNode.OnChildConnected(indexToInsert);
			targetNode.OnParentConnected(targetNode.inConnections.IndexOf(newConnection));

			UpdateNodeIDs(false);
			return newConnection;
		}

		///Removes a connection
		public void RemoveConnection(Connection connection, bool recordUndo = true){

			//for live editing
			if (Application.isPlaying){
				connection.Reset();
			}

			if (recordUndo){
				RecordUndo("Delete Connection");
			}

			//callbacks
			connection.OnDestroy();
			connection.sourceNode.OnChildDisconnected(connection.sourceNode.outConnections.IndexOf(connection));
			connection.targetNode.OnParentDisconnected(connection.targetNode.inConnections.IndexOf(connection));
			
			connection.sourceNode.outConnections.Remove(connection);
			connection.targetNode.inConnections.Remove(connection);

			#if UNITY_EDITOR
			NodeCanvas.Editor.GraphEditorUtility.activeElement = null;
			#endif

			UpdateNodeIDs(false);
		}

		//Helper function
		public void RecordUndo(string name){
			#if UNITY_EDITOR
			if (!Application.isPlaying){
				UnityEditor.Undo.RecordObject(this, name);
			}
			#endif
		}

		///Clears the whole graph
		public void ClearGraph(){
			canvasGroups = null;
			foreach (var node in allNodes.ToArray()){
				RemoveNode(node);
			}
		}
		
	}
}