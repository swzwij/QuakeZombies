using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("MapMenu");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void Map_01()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
