using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject start;
    [SerializeField] private GameObject settings;

    private void Start()
    {
        if (settings != null)
        {
            settings.SetActive(false);
        }
            
    }

    public void Play()
    {
        SceneManager.LoadScene("Game");
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void OpenSettings()
    {
        if (!settings.activeSelf)
        {
            settings.SetActive(true);
            start.SetActive(false);
        }
        else
        {
            settings.SetActive(false);
            start.SetActive(true);
        }
    }

}
