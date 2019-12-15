using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Camera2Screen : MonoBehaviour
{
    LevelLoop level;
    bool arrived = false;
    bool spawn = false;
    public GameObject screen;
    public GameObject Point;

    public Text popularity;
    public Text liberated;
    public Text escaped;
    public Text money;
    public Text time;

    float timer = 0;

    public Sprite win;
    public Sprite lose;

    // Start is called before the first frame update
    void Start()
    {
        level = GameObject.Find("Level").GetComponent<LevelLoop>();
        Point = screen.transform.Find("Point").gameObject;
        arrived = false;
        spawn = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (level.stop_Game)
        {
            if (!arrived)
            {
                if (!spawn)
                {
                    screen = Instantiate(screen);

                    liberated = screen.transform.Find("Texts").Find("Liberated").GetComponent<Text>();
                    escaped = screen.transform.Find("Texts").Find("Escaped").GetComponent<Text>();
                    popularity = screen.transform.Find("Texts").Find("Popularity").GetComponent<Text>();
                    money = screen.transform.Find("Texts").Find("Money").GetComponent<Text>();
                    time = screen.transform.Find("Texts").Find("Time").GetComponent<Text>();
                    timer = Time.timeSinceLevelLoad;

                    spawn = true;
                    gameObject.transform.position = new Vector3(Point.transform.position.x, Point.transform.position.y, Point.transform.position.z - 20);
                }
                transform.LookAt(screen.transform);
                gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, new Vector3(Point.transform.position.x, Point.transform.position.y, Point.transform.position.z), 25 * Time.deltaTime);
                if (gameObject.transform.position == Point.transform.position)
                {
                    arrived = true;
                }
            }
            else if(arrived)
            {

                liberated.text = string.Format("{0}", level.num_liberated);
                escaped.text = string.Format("{0}", level.num_escaped);
                popularity.text = string.Format("{0}", level.popul.mCurrent);
                money.text = string.Format("{0}", level.money.mCurrent);
                time.text = string.Format("{0}", (int)timer) + " SEG";
            }
        }
    }
}
