using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : MonoBehaviour
{
    public int flowerNumber;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GetComponent<SpriteRenderer>().enabled = false;
        PlayerPrefs.SetInt("Flower " + flowerNumber, 1);

        if (flowerNumber == 7)
        {
            if (PlayerPrefs.GetInt("Pacifist") == 1)
                PlayerPrefs.SetInt("PetBeetle", 1);
        }
    }
}
