using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseUI : MonoBehaviour
{
    GameManager gm;
    AudioManager audioManager;

    public GameObject PausePanel;
    bool isPaused = false;

    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) Resume(); else Pause();
        }
    }

    void Pause()
    {
        audioManager.Play("Menu Click");
        isPaused = true;
        PausePanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void Resume()
    {
        audioManager.Play("Menu Click");
        isPaused = false;
        PausePanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void Greenhouse()
    {
        audioManager.Play("Menu Click");
        Resume();
        Destroy(GameObject.Find("MusicPlayer"));
        gm.touchedCheckpoint = false;
        SceneManager.LoadScene("Greenhouse");
    }

    public void Restart()
    {
        audioManager.Play("Menu Click");
        Resume();
        Destroy(GameObject.Find("MusicPlayer"));
        gm.touchedCheckpoint = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Home()
    {
        audioManager.Play("Menu Click");
        Resume();
        Destroy(GameObject.Find("MusicPlayer"));
        gm.touchedCheckpoint = false;
        SceneManager.LoadScene("Main Menu");
    }
}
