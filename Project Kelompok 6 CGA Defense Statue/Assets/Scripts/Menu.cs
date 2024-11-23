using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void Stage1Start() {
        SceneManager.LoadSceneAsync("Stage1");
    }

    public void Stage2Start() {
        SceneManager.LoadSceneAsync("Stage2");
    }

    public void Stage3Start() {
        SceneManager.LoadSceneAsync("Stage3");
    }

    public void Stage4Start() {
        SceneManager.LoadSceneAsync("Stage4");
    }

}
