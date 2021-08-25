using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipPage : MonoBehaviour
{
    public GameObject prev;
    public GameObject next;

    public void PrevPage()
    {
        if (prev != null) prev.SetActive(true);
        gameObject.SetActive(false);
    }

    public void NextPage()
    {
        if (next != null) next.SetActive(true);
        gameObject.SetActive(false);
    }
}
