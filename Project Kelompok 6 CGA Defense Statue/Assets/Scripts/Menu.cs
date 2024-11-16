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

}
