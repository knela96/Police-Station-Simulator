using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{

    public GameObject cameraOne;
    public GameObject cameraTwo;
    LevelLoop level;
    int cameranumb = 0;
    timeonstage time;
    AudioListener cameraOneAudioLis;
    AudioListener cameraTwoAudioLis;
    // Start is called before the first frame update
    void Start()
    {
        //Get Camera Listeners
        cameraOneAudioLis = cameraOne.GetComponent<AudioListener>();
        cameraTwoAudioLis = cameraTwo.GetComponent<AudioListener>();
        level = GameObject.Find("Level").GetComponent<LevelLoop>();
        time = GameObject.Find("Time").GetComponent<timeonstage>();
    }

    // Update is called once per frame
    public void START()
    {
        //Change Camera Keyboard
        cameranumb = 1;
        cameraPositionChange();
        level.enabled = true;
        time.enabled = true;
    }


    //Camera change Logic
    void cameraPositionChange()
    {
        

        //Set camera position 1
        if (cameranumb == 0)
        {
            cameraOne.SetActive(true);
            cameraOneAudioLis.enabled = true;

            cameraTwoAudioLis.enabled = false;
            cameraTwo.SetActive(false);
        }

        //Set camera position 2
        if (cameranumb == 1)
        {
            cameraTwo.SetActive(true);
            cameraTwoAudioLis.enabled = true;

            cameraOneAudioLis.enabled = false;
            cameraOne.SetActive(false);
        }

    }
}
