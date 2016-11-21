using UnityEngine;
using System.Collections;


/// <summary>
/// Handles movement of the enemy
/// and also animation/attacking/ etc
/// </summary>
public class EnemyMovement : MonoBehaviour {

    public GameObject player;
    public PlayerManager playerScript;
    public GUIManager stateManagerScript;
    public GameObject stateManager;
    CharacterController controller;
   

    Animation anim;
    Vector3 direction;
    Vector3 gravity = new Vector3(0, -.5f, 0);
    
    // TODO: Add jumping (Later)
    //int jumpCounter = 30;
    //float jumpSpeed = 10;
    public float speed;
    public int hp;
    public int attackDist = 4;
    private float timer;

    private bool isOnFire;
    private float fireTimer;
    public bool dead;
    private bool isPoisened;

    



    void Start () {
        fireTimer = 5;
        dead = false;
        player = GameObject.Find("Player");
        stateManager = GameObject.Find("StateManager");
        stateManagerScript = stateManager.GetComponent<GUIManager>();
        playerScript = player.GetComponent<PlayerManager>();
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animation>();

        timer = 5;
	    
	}
	
	
	void Update () {

        //damage calculators
        if (isOnFire && fireTimer > 0) {
            isBeingHurt(2);
            fireTimer -= Time.deltaTime;
        }
        if (isPoisened)
        {
            isBeingHurt(1);
        }

        if (hp < 0 && !dead) {
            dead = true;
            Die();
            stateManagerScript.enemyDeath();

        }

        if (!dead) {

            direction = new Vector3(0, 0, 0);
            if (Vector3.Distance(player.transform.position, transform.position) > attackDist) {

                if (controller.isGrounded)
                    moveToPlayer();
                else
                    controller.Move(gravity);

            }
            else {
                attackPlayer();
            }

            direction = transform.position - player.transform.position;
            transform.rotation = Quaternion.LookRotation(direction * -1);

        }
        else {
            timer -= Time.deltaTime;
            transform.Translate(new Vector3(0, -.08f, 0));
            Destroy(controller);
            if (timer < 0) {
                playerScript.points += 10;
                Destroy(this.gameObject);

            }
        }
        

	}



    public void moveToPlayer()
    {
        direction = transform.position - player.transform.position;
        anim.Play("walk");
        
        controller.Move(-1 * direction.normalized * speed * Time.deltaTime);

        
    }

    public void attackPlayer()
    {
        anim.Play("attack");
        playerScript.loseHP(1);
    }

    public void Die()
    {
        //it cant find this anim - FIX
        //anim.Play("die");
    }

    public void isBeingHurt(int amount)
    {
        hp -= amount;
    }



    void OnCollisionEnter(Collision col)
    {
        if (col.transform.tag == "RollingRock") {
            isBeingHurt(200);
            stateManagerScript.gameScore += 20;
        }
        

    }


    void OnTriggerEnter(Collider hit)
    {

        if (hit.transform.tag == "Fire") {
            isOnFire = true;
            fireTimer = 5;
            stateManagerScript.gameScore += 5;
        }
        if (hit.transform.tag == "Poison")
        {
            isPoisened = true;
            stateManagerScript.gameScore += 2;


        }

    }

    void OnTriggerExit(Collider hit)
    {
        if (hit.transform.tag == "Fire") {
            isOnFire = false;

        }
        if (hit.transform.tag == "Poison")
        {
            isPoisened = false;

        }
        

    }

}
