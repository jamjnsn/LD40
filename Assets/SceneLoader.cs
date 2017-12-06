using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {
    public void TryAgain()
    {
        SceneManager.LoadScene("Room");
    }

    public void Lose()
    {
        SceneManager.LoadScene("Loss");
    }

    public void Win()
    {
        SceneManager.LoadScene("Win");
    }
}
