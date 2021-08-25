using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject levelSelectPanel;
    public Button[] lvlButtons;
    AudioManager audioManager;

    int levelAt;

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();

        levelAt = PlayerPrefs.GetInt("levelAt", 1);

        for (int i = 0; i < lvlButtons.Length; i++)
        {
            if (i + 1 > levelAt)
            {
                lvlButtons[i].gameObject.SetActive(false);
            }
        }
    }

    public void StartButton()
    {
        levelAt = 1;
        PlayerPrefs.SetInt("levelAt", 1);
        PlayerPrefs.SetInt("Flower 0", 0);
        PlayerPrefs.SetInt("Flower 1", 0);
        PlayerPrefs.SetInt("Flower 2", 0);
        PlayerPrefs.SetInt("Flower 3", 0);
        PlayerPrefs.SetInt("Flower 4", 0);
        PlayerPrefs.SetInt("Flower 5", 0);
        PlayerPrefs.SetInt("Flower 6", 0);
        PlayerPrefs.SetInt("Flower 7", 0);

        PlayerPrefs.SetInt("Pacifist", 1);
        PlayerPrefs.SetInt("PetBeetle", 0);

        audioManager.Play("Menu Click");
        SceneManager.LoadScene(levelAt);
    }

    public void Levels()
    {
        audioManager.Play("Menu Click");
        levelSelectPanel.SetActive(true);
    }

    public void Exit()
    {
        audioManager.Play("Menu Click");
        Application.Quit();
    }

    public void LevelSelect(int lvl)
    {
        audioManager.Play("Menu Click");
        SceneManager.LoadScene(lvl);
    }

    public void Back()
    {
        audioManager.Play("Menu Click");
        levelSelectPanel.SetActive(false);
    }
}
