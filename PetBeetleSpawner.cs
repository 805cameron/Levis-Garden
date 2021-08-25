using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetBeetleSpawner : MonoBehaviour
{
    public GameObject petBeetle;

    void Start()
    {
        if (PlayerPrefs.GetInt("PetBeetle") == 1) Instantiate(petBeetle, this.transform);
    }
}
