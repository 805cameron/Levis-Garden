using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beetle : Enemy
{
    // Start is called before the first frame update
    void Start()
    {
        isFacingLeft = true;
    }

    // Update is called once per frame
    public void Update()
    {
        MoveH(isFacingLeft);

        if (RayForEnvironment(GetComponent<BoxCollider2D>().size.x / 2 + 0.1f))
            isFacingLeft = !isFacingLeft;

        // scan for edge to not walk off
        if (!RayCheckForEdge() && Grounded())
        {
            isFacingLeft = !isFacingLeft;
        }
    }

    RaycastHit2D RayCheckForEdge()
    {
        Vector2 rayOrigin = transform.position;
        rayOrigin.x = isFacingLeft ? transform.position.x - (GetComponent<BoxCollider2D>().size.x / 2)
                                   : transform.position.x + (GetComponent<BoxCollider2D>().size.x / 2);
        
        return Physics2D.Raycast(rayOrigin, Vector2.down, 0.01f);
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Enemy") isFacingLeft = !isFacingLeft;
    }

    public override void TakeDamage(int dmg, Transform dmgSource)
    {
        base.TakeDamage(dmg, dmgSource);
        if (health > 0) audioManager.Play("BeetleHit");
    }
}
