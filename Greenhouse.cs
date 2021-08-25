using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Greenhouse : MonoBehaviour
{
    public GameObject[] flowers;
    public GameObject book;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < flowers.Length; i++)
        {
            if(PlayerPrefs.GetInt("Flower " + i) == 1) flowers[i].SetActive(true);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !book.activeSelf)
        {
            if (PlayerPrefs.GetInt("levelAt") != 6)
                SceneManager.LoadScene(PlayerPrefs.GetInt("levelAt", 1));
            else
                SceneManager.LoadScene("Main Menu");
        }
    }
}
