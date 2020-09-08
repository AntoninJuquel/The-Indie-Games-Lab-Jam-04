using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainBtn : MonoBehaviour
{
    public GameObject endLessBtn;
    private void Start()
    {
        if(PlayerPrefs.GetInt("levelReached") < 10)
        {
            endLessBtn.SetActive(false);
        }else
            endLessBtn.SetActive(true);
    }
    public void EndLess()
    {
        SceneManager.LoadScene(11);
    }
    public void Play()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
