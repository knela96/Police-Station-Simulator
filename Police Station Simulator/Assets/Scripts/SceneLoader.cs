using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    public void START()
    {
        SceneManager.LoadScene("PoliceScene");
    }

    public void Credits()
    {
        Application.OpenURL("http://unity3d.com/");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
