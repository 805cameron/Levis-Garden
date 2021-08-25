using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom_Seed : Seed
{
    public GameObject mushroom;

    // Start is called before the first frame update
    public override void Awake()
    {
        base.Awake();
    }

    public override void Update()
    {
        base.Update();
    }

    public override IEnumerator Grow()
    {
        Instantiate(mushroom, plantedPos, Quaternion.identity);
        yield return null;
    }
}
