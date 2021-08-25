using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Creature : MonoBehaviour
{
    [HideInInspector]
    public Rigidbody2D rb2d;
    [HideInInspector]
    public SpriteRenderer sprite;
    [HideInInspector]
    public Animator animator;
    [HideInInspector]
    public AudioManager audioManager;


    [HideInInspector]
    public bool moveReady = true;
    [HideInInspector]
    public bool attackReady = true;
    [HideInInspector]
    public bool isFacingLeft = false;

    [Header("Stats")]
    public int maxHealth = 3;
    [HideInInspector]
    public int health;
    public int attackDamage = 1;

    [Header("Movement Values")]
    public float moveSpeed = 1;
    public float attackRange = 0.2f;

    [HideInInspector]
    public bool moving;

    [HideInInspector]
    public bool dying = false;

    // Start is called before the first frame update
    public virtual void Awake()
    {
        dying = false;
        rb2d = GetComponent<Rigidbody2D>();
        animator = transform.GetChild(0).GetComponent<Animator>();
        sprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
        health = maxHealth;
        audioManager = FindObjectOfType<AudioManager>();
    }

    public virtual void MoveH(bool facingLeft)
    {
        Vector3 rayOrigin = transform.position;
        rayOrigin.y += 0.2f;

        transform.position += (facingLeft ? Vector3.left : Vector3.right) * Time.deltaTime * moveSpeed;
        moving = true;

        sprite.flipX = facingLeft;
    }

    public RaycastHit2D RayForEnvironment(float length)
    {
        Vector3 rayOrigin = transform.position;
        rayOrigin.y += 0.1f;
        Debug.DrawRay(rayOrigin, (isFacingLeft ? Vector3.left : Vector3.right) * length, Color.red, 0.01f);
        return Physics2D.Raycast(rayOrigin, isFacingLeft ? Vector3.left : Vector3.right, length, 1 << 9);
    }

    public bool Grounded()
    {
        Vector2 rayOrigin = transform.position;
        
        RaycastHit2D leftRay = Physics2D.Raycast(rayOrigin - new Vector2(GetComponent<BoxCollider2D>().size.x / 2 + 0.015f, 0),
                                                    Vector2.down, 
                                                    0.01f,
                                                    1 << 9);
                                                                         
        RaycastHit2D rightRay = Physics2D.Raycast(rayOrigin + new Vector2(GetComponent<BoxCollider2D>().size.x / 2 + 0.015f, 0),
                                                    Vector2.down,
                                                    0.01f,
                                                    1 << 9);
                                                    
        RaycastHit2D centerRay = Physics2D.Raycast(rayOrigin,
                                                    Vector2.down, 
                                                    0.01f,
                                                    1 << 9);

        Debug.DrawRay(rayOrigin - new Vector2(GetComponent<BoxCollider2D>().size.x / 2 + 0.015f, 0),
                        Vector2.down * 0.1f,
                        Color.yellow,
                        0.01f);

        Debug.DrawRay(rayOrigin + new Vector2(GetComponent<BoxCollider2D>().size.x / 2 + 0.015f, 0),
                        Vector2.down * 0.1f,
                        Color.red,
                        0.01f);

        Debug.DrawRay(rayOrigin,
                        Vector2.down * 0.1f,
                        Color.cyan,
                        0.01f);


        return (leftRay || rightRay || centerRay);
    }


    public void Attack()
    {
        animator.SetTrigger("Attack");

        audioManager.Play("Swing");

        Vector2 pos = transform.position;
        pos.y += 0.1f;

        RaycastHit2D ray = Physics2D.Raycast(pos, isFacingLeft ? Vector2.left : Vector2.right, attackRange, 1 << 10);
        Debug.DrawRay(pos, (isFacingLeft ? Vector2.left : Vector2.right) * attackRange, Color.red, 0.7f);

        if (ray.collider)
        {
            if (ray.collider.tag == "Enemy")
            {
                ray.collider.GetComponent<Enemy>().TakeDamage(attackDamage, this.transform);
            }
        }
    }

    public virtual void TakeDamage(int dmg, Transform dmgSource)
    {
        health -= dmg;
        if (health <= 0 && !dying) StartCoroutine(Die());
        else
        {
            StartCoroutine(HurtEffect());
            Vector3 force = new Vector3(((this.transform.position.x > dmgSource.position.x) ? 30 : -30), 50, 0);
            rb2d.AddForce(force);
        }
            //camera shake
            //damage animation
    }


    public IEnumerator HurtEffect()
    {
        Color originalColor = sprite.color;
        Color newColor = sprite.color;

        newColor.g -= 0.4f;
        newColor.b -= 0.4f;

        sprite.color = newColor;

        yield return new WaitForSeconds(0.1f); // make fancy n shit later

        sprite.color = originalColor;
       
        yield return null; 
    }

    public IEnumerator Die()
    {
        if (this.tag != "Player")
        {
            audioManager.Play("BeetleDeath");
            if (PlayerPrefs.GetInt("PetBeetle") != 1) PlayerPrefs.SetInt("Pacifist", 0);
            Destroy(this.gameObject);
            yield return null;
        }
        else
        {
            dying = true;
            moveReady = false;
            GameObject ps = Instantiate(GetComponent<PlayerMovement>().DeathParticles.gameObject, transform);
            ps.GetComponent<ParticleSystem>().Play();
            audioManager.Play("LeviDeath");
            sprite.enabled = false;
            yield return new WaitForSeconds(1.5f);

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        //reset scene
    }


}
