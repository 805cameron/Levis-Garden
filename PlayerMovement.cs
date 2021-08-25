using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : Creature
{
    GameManager gm;

    float timeSinceLastAttack = 0;
    bool startedLanding = false;

    [Header("Player Variables")]
    public int maxWater = 3;
    public int jumpStrength = 100;
    public float attackDelay = 0.5f;
    public float invincibilityLength = 0.5f;

    int inventorySize = 3;
    bool isColliding;
    bool touchingVine = false;
    bool isClimbing = false;
    bool isInvincible = false;

    bool touchingBook = false;

    Seed[] seedInventory;
    int selectedSlot = 0;

    //ui
    [Header("UI")]
    public Sprite selectedSprite;
    public Sprite unselectedSprite;

    public Image[] heartContainers = new Image[3];
    public Sprite fullHeart;
    public Sprite emptyHeart;

    public ParticleSystem DeathParticles;

    Image[] inventorySlots = new Image[3];
    List<Seed> plantedSeeds = new List<Seed>();

    public Seed defaultSeed;


    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameManager>();
        if (gm.touchedCheckpoint) transform.position = gm.lastCheckPointPos;

        seedInventory = new Seed[inventorySize];
        InitArrays();
        selectedSlot = 0;
        inventorySlots[selectedSlot].GetComponent<Image>().sprite = selectedSprite;
    }
    

    // Update is called once per frame
    public void Update()
    {
        animator.SetBool("Falling", rb2d.velocity.y < 0);

        if (moveReady) HandleInput();
    }


    void InitArrays()
    {
        for (int i = 0; i < heartContainers.Length; i++)
        {
            heartContainers[i] = GameObject.Find("Heart " + i).GetComponent<Image>();
        }

        for (int i = 0; i < inventorySlots.Length; i++)
        {
            inventorySlots[i] = GameObject.Find("Inventory " + i).GetComponent<Image>();
        }
    }


    /*****************************************************************************************************************/


    void HandleInput()
    {
        if (isClimbing == false)
        {
            if (Input.GetKey(KeyCode.A))
            {
                isFacingLeft = true;
                if (!RayForEnvironment(0.1f)) MoveH(isFacingLeft);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                isFacingLeft = false;
                if (!RayForEnvironment(0.1f)) MoveH(isFacingLeft);
            }



            if (Input.GetKeyDown(KeyCode.Space) && Grounded() && !touchingVine)
            {
                Jump();
            }

            attackReady = (Time.time - timeSinceLastAttack > attackDelay);

            if (Input.GetMouseButtonDown(0) && attackReady && Grounded())
            {
                Attack();
                timeSinceLastAttack = Time.time;
            }


            //book
            if (touchingBook && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.E)))
            {
                Time.timeScale = 0;
                GameObject.FindWithTag("book").GetComponent<Book>().book.SetActive(true);
            }



            if (Input.GetKeyDown(KeyCode.E) && Grounded())
            {
                StartCoroutine(Water(ClosestSeed() ));
            }

            if (Input.GetKeyDown(KeyCode.Q) && Grounded() && seedInventory[selectedSlot] != null)
            {
                StartCoroutine(PlantSeed(seedInventory[selectedSlot]) );
            }

            if (touchingVine && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space)))
            {
                if (animator.GetBool("Climbing") == false ) animator.SetBool("Climbing", true);
                isClimbing = true;
                rb2d.gravityScale = 0;
                rb2d.velocity = Vector3.zero;
            }
        }



        if (Input.GetKey(KeyCode.Alpha1))
        {
            UpdateSlot(0);
        }

        if (Input.GetKey(KeyCode.Alpha2))
        {
            UpdateSlot(1);
        }

        if (Input.GetKey(KeyCode.Alpha3))
        {
            UpdateSlot(2);
        }



        //Climbing Controls
        if (isClimbing == true)
        {
            if (Input.GetKey(KeyCode.W))
            {
                transform.position += Vector3.up * Time.deltaTime * (moveSpeed);
                moving = true;
                animator.SetFloat("ChargedAsylumIsMeanie", 1); // ChargedAsylumIsMeanie is just Speed
            }

            if (Input.GetKey(KeyCode.S))
            {
                transform.position += Vector3.down * Time.deltaTime * (moveSpeed);
                moving = true;
                animator.SetFloat("ChargedAsylumIsMeanie", -1); // ChargedAsylumIsMeanie is just Speed
            }

            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
            {
                rb2d.gravityScale = 0.7f;
                isClimbing = false;
                animator.SetBool("Climbing", false);
                rb2d.velocity = Vector3.zero;
            }
        }

        if (moving && Grounded())
        {
            if (!GetComponent<AudioSource>().isPlaying) GetComponent<AudioSource>().Play();
        }

        animator.SetBool("Running", moving);
        moving = false;
        if (!touchingVine)
        {
            isClimbing = false;
            animator.SetBool("Climbing", false);
            rb2d.gravityScale = 0.7f;
        }
    }


    void Jump()
    {
        rb2d.AddForce(Vector2.up * jumpStrength);

        animator.SetTrigger("Jump");
        animator.SetBool("Grounded", false);
    }
    

    void UpdateSlot(int k)
    {
        inventorySlots[selectedSlot].sprite = unselectedSprite;

        selectedSlot = k;

        inventorySlots[selectedSlot].sprite = selectedSprite;
    }

    void UpdateHealth()
    {
        for (int i = 0; i < heartContainers.Length; i++)
        {
            heartContainers[i].sprite = (i < health) ? fullHeart : emptyHeart;
        }
    }

    /*****************************************************************************************************************/

    void PickUpSeed(Seed seed)
    {
        for (int i = 0; i < seedInventory.Length; i++)
        {
            if (seedInventory[i] == null)
            {
                seedInventory[i] = seed;
                
                inventorySlots[i].transform.GetChild(0).GetComponent<Image>().sprite = seed.sprite;
                inventorySlots[i].transform.GetChild(0).GetComponent<Image>().preserveAspect = true;

                Color c = inventorySlots[i].transform.GetChild(0).GetComponent<Image>().color;
                c.a = 1;
                inventorySlots[i].transform.GetChild(0).GetComponent<Image>().color = c;

                UpdateSlot(i);

                seed.transform.GetChild(0).gameObject.SetActive(false);
                break;
            }
        }

        audioManager.Play("Pickup");

    }


    IEnumerator PlantSeed(Seed seed)
    {
        moveReady = false;
        attackReady = false;
        animator.SetTrigger("Plant");

        audioManager.Play("Plant");

        seedInventory[selectedSlot] = null;

        inventorySlots[selectedSlot].transform.GetChild(0).GetComponent<Image>().sprite = null;

        Color c = inventorySlots[selectedSlot].transform.GetChild(0).GetComponent<Image>().color;
        c.a = 0;
        inventorySlots[selectedSlot].transform.GetChild(0).GetComponent<Image>().color = c;

        for (int i = seedInventory.Length - 1; i > 0; i--)
        {
            if (seedInventory[i] != null)
            {
                selectedSlot = i;
                break;
            }

            UpdateSlot(0);
        }

        yield return new WaitForSeconds(1.5f); // 1.5 = length of planting animation

        //plant seed
        seed.plantedPos = transform.position;
        plantedSeeds.Add(seed);
        seed.planted = true;

        moveReady = true;
    }

    public override void TakeDamage(int dmg, Transform dmgSource)
    {
        base.TakeDamage(dmg, dmgSource);
        if (health > 0) audioManager.Play("LeviHit");
    }


    IEnumerator Water(Seed seed)
    {
        moveReady = false;
        attackReady = false;
        animator.SetTrigger("Water");
        audioManager.Play("Water");

        yield return new WaitForSeconds(1.5f); // 1.5 = length of watering animation


        if (seed.distanceToPlayer < 0.5f)
        {
            StartCoroutine(seed.Grow());
            plantedSeeds.Remove(seed);
        }

        moveReady = true;
    }

    Seed ClosestSeed()
    {
        Seed closest = defaultSeed;

        for (int i = 0; i < plantedSeeds.Count; i++)
        {
            if (plantedSeeds[i].distanceToPlayer < closest.distanceToPlayer) closest = plantedSeeds[i];
        }

        return closest;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            startedLanding = false;
            animator.SetTrigger("Land");
            animator.SetBool("Grounded", true);
            animator.SetBool("Climbing", false);

            rb2d.gravityScale = 0.7f;
            isClimbing = false;
        }

        if (collision.collider.tag == "Enemy")
        {
            Enemy enemy = collision.collider.GetComponent<Enemy>();

            if (!isInvincible)
            {
                TakeDamage(enemy.attackDamage, collision.transform);
                UpdateHealth();
                StartCoroutine(Invincibility());
            }
        }

        if (collision.collider.tag == "Thorns" && !dying)
        {
            StartCoroutine(Die());
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Enemy")
        {
            Enemy enemy = collision.collider.GetComponent<Enemy>();

            if (!isInvincible)
            {
                TakeDamage(enemy.attackDamage, collision.transform);
                UpdateHealth();
                StartCoroutine(Invincibility());
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isColliding) return;
        isColliding = true;

        if (collision.tag == "Seed")
        {
            PickUpSeed(collision.transform.GetComponentInParent<Seed>());
        }

        if (collision.tag == "Mushroom" && health != maxHealth)
        {
            audioManager.Play("Mushroom");
            health = maxHealth;
            UpdateHealth();

            Destroy(collision.gameObject);
        }

        if (collision.tag == "book")
        {
            touchingBook = true;
        }

        StartCoroutine(Reset());
    }

    public IEnumerator Invincibility()
    {
        isInvincible = true;
        yield return new WaitForSeconds(0.5f);
        isInvincible = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Vines") touchingVine = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Vines") touchingVine = false;

        if (collision.tag == "book")
        {
            touchingBook = false;
        }
    }

    IEnumerator Reset()
    {
        yield return new WaitForEndOfFrame();
        isColliding = false;
    }

}
