using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    GameManager gm;

    private void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            gm.lastCheckPointPos = transform.position;
            gm.touchedCheckpoint = true;
            transform.GetChild(0).GetComponent<Animator>().SetTrigger("Grow");
                
        }
    }
}
