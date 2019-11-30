using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class myImageCompomonent : MonoBehaviour
{
    Image myImageComponent;
    public Sprite firstImage;
    public Sprite secondImage; 
    LevelLoop level;
    
    void Awake()
    {
        myImageComponent = GetComponent<Image>();
        level = GameObject.Find("Level").GetComponent<LevelLoop>();

    }
    public void SetImage1() 
    {
        myImageComponent.sprite = firstImage;
    }

    public void SetImage2()
    {
        myImageComponent.sprite = secondImage;
    }

    void Update()
    {
        if (level.day) {

            SetImage1();

        }
        else {
            SetImage2();

        }




    }


}
