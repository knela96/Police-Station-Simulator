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
    
    public void START()
    {
        SceneManager.LoadScene("PoliceScene");
    }

    public void Resume(GameObject go)
    {
        go.GetComponent<MenuOptions>().show = false;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Credits(GameObject go)
    {
        go.SetActive(!go.active);
    }

    public void OpenURL()
    {
        Application.OpenURL("http://unity3d.com/");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
