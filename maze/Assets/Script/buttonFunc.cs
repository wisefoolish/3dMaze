using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonFunc : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchScenes(string scenename)
    {
        SceneManager.LoadScene(scenename);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
