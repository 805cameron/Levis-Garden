using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetBeetle : Creature
{
    // Start is called before the first frame update
    void Start()
    {
        isFacingLeft = true;
        StartCoroutine(WalkCycle());
    }

    // Update is called once per frame
    void Update()
    {
        if (RayForEnvironment(GetComponent<BoxCollider2D>().size.x / 2 + 0.1f))
            isFacingLeft = !isFacingLeft;

        // scan for edge to not walk off
        if (!RayCheckForEdge() && Grounded())
        {
            isFacingLeft = !isFacingLeft;
        }
    }

    IEnumerator Move()
    {
        isFacingLeft = (Random.value < 0.5);

        float walkTime = Random.Range(2, 4);
        float t = Time.time;

        while (Time.time - t <= walkTime)
        {
            MoveH(isFacingLeft);
            if (animator.GetBool("moving") == false) animator.SetBool("moving", true);
            yield return null;
        }
    }
    
    IEnumerator WalkCycle()
    {
        while(true)
        {
            if (animator.GetBool("moving") == true) animator.SetBool("moving", false);
            yield return new WaitForSeconds(Random.Range(2, 5));
            yield return StartCoroutine(Move());
        }
    }

    RaycastHit2D RayCheckForEdge()
    {
        Vector2 rayOrigin = transform.position;
        rayOrigin.x = isFacingLeft ? transform.position.x - (GetComponent<BoxCollider2D>().size.x / 2)
                                   : transform.position.x + (GetComponent<BoxCollider2D>().size.x / 2);

        return Physics2D.Raycast(rayOrigin, Vector2.down, 0.01f);

    }
}
