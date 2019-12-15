﻿using System.Collections;
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
    public Image logo;

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
            if (!spawn)
            {
                screen = Instantiate(screen);

                liberated = screen.transform.Find("Texts").Find("Liberated").GetComponent<Text>();
                escaped = screen.transform.Find("Texts").Find("Escaped").GetComponent<Text>();
                popularity = screen.transform.Find("Texts").Find("Popularity").GetComponent<Text>();
                money = screen.transform.Find("Texts").Find("Money").GetComponent<Text>();
                time = screen.transform.Find("Texts").Find("Time").GetComponent<Text>();
                logo = screen.transform.Find("Logos").Find("You").GetComponent<Image>();
                timer = Time.timeSinceLevelLoad;

                liberated.text = string.Format("{0}", level.num_liberated);
                escaped.text = string.Format("{0}", level.num_escaped);
                popularity.text = string.Format("{0}", level.popul.mCurrent);
                money.text = string.Format("{0}", level.money.mCurrent);

                int c = (int)(timer - (timer % 60));
                if ( c < 10)
                    time.text = "0" + string.Format("{0}", c);
                else
                    time.text = string.Format("{0}", c);

                c = (int)(timer % 60);
                if (c < 10)
                    time.text = time.text + ".0" + string.Format("{0}", c);
                else
                    time.text = time.text + string.Format("{0}", c);


                if(level.num_liberated >= 5)
                {
                    logo.sprite = win;
                }else if(level.num_escaped >= 3)
                {
                    logo.sprite = lose;
                }


                GetComponent<CameraMovement>().enabled = false;

                spawn = true;
                gameObject.transform.position = new Vector3(Point.transform.position.x, Point.transform.position.y, Point.transform.position.z - 20);
            }
            transform.LookAt(screen.transform);
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, new Vector3(Point.transform.position.x, Point.transform.position.y, Point.transform.position.z), 25 * Time.deltaTime);
        }
    }
}