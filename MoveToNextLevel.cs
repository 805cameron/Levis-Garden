using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveToNextLevel : MonoBehaviour
{
    GameManager gm;
    [HideInInspector]
    public int nextSceneLoad;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameManager>();
        nextSceneLoad = SceneManager.GetActiveScene().buildIndex + 1;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            gm.touchedCheckpoint = false;
            StartCoroutine(GoToNext());
        }
    }

    IEnumerator GoToNext()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(GameObject.Find("MusicPlayer"));

        SceneManager.LoadScene(nextSceneLoad);
        if(nextSceneLoad > PlayerPrefs.GetInt("levelAt"))
        {
            PlayerPrefs.SetInt("levelAt", nextSceneLoad);
        }
    }
}
