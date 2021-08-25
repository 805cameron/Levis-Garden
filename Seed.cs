using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Seed : MonoBehaviour
{
    [HideInInspector]
    public Sprite sprite;
    [HideInInspector]
    public float distanceToPlayer = 1000f;
    [HideInInspector]
    public bool planted = false;
    [HideInInspector]
    public Vector3 plantedPos;

    // Start is called before the first frame update
    public virtual void Awake()
    {
        if (transform.GetChild(0).GetComponent<SpriteRenderer>() != null)
            sprite = transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (planted) distanceToPlayer = (GameObject.FindGameObjectWithTag("Player").transform.position - plantedPos).magnitude;
    }

    public virtual IEnumerator Grow()
    {
        yield return null;
    }
}
