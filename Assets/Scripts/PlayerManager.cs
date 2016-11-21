using UnityEngine;
using System.Collections;
using System;

public class PlayerManager : MonoBehaviour {

    // This is used for activating the mode
    public bool isModeActive = false;
    public bool isAttackMode = false;

    public GameObject poisonBush;
    public GameObject fireTree;
    public GameObject rollingBall;
    public GameObject antiSpiderCannon;

    public GUIManager guiManager;

    private bool isOnFire;
    public int points;
    public int hp;
    

    public float burnTimer;
    public float stamina;

    public float minStamina;
    public float maxStamina;

    public float speed;
    public float gravity;

    Animation anim;

    public GameObject flame;

    CharacterController controller;
    private GameObject[] closestTorches;

    private const float STAM_DEC = 0.8f;
    private const float STAM_INC = 1f;

    void Start () {
        stamina = 1000;
        
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animation>();
        anim.Play("Idle");

    }
	
	// Update is called once per frame
	void Update () {
        if(stamina < minStamina)
        {
            stamina = minStamina;
        }
        if(stamina > maxStamina)
        {
            stamina = maxStamina;
        }

        closestTorches = GameObject.FindGameObjectsWithTag("Fire");

        if (!guiManager.gameOver) {
            if (isOnFire) {
                isBurning();
            }

            if (isModeActive) {
                if (Input.touchCount == 2) {
                    Touch touch1 = Input.GetTouch(0);
                    Touch touch2 = Input.GetTouch(1);
                    float x = (touch1.position.x - touch2.position.x) / 2;
                    float y = (touch1.position.y - touch2.position.y) / 2;
                    Vector2 midPoint = new Vector2(x, y);

                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(midPoint);

                    if (Physics.Raycast(ray, out hit)) {
                        if (points > 5) {
                            Vector3 point = hit.point;
                            point.y += 1;
                            GameObject.Instantiate(flame, point, new Quaternion());
                            points -= 5;
                        }
                    }


                }


                if (Input.GetMouseButton(0)) {

                    playerMovement();

                }
                else {
                    anim.Play("Idle");
                    stamina += STAM_INC;
                }

                if (Input.GetMouseButtonDown(1)) {
                    setFire();

                }
            }
            else {
                anim.Play("Idle");
                stamina += STAM_INC;
            }
        }

        

	
	}

    public void playerMovement()
    {
        float staminaLerp = Mathf.InverseLerp(0, 1000, stamina);
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit)) {
            Vector3 direction = new Vector3();
            direction = (hit.point - transform.position);
            direction.y = 0;

            if (!controller.isGrounded) {
                controller.Move(new Vector3(0, -1, 0));
            }

            if (direction.magnitude > 1) {
                controller.Move(direction.normalized * speed * Time.deltaTime*staminaLerp);
                transform.rotation = Quaternion.LookRotation(direction);

                anim.Play("Walk");
                stamina -= STAM_DEC;
            }
            else {
                anim.Play("Idle");
                stamina += STAM_INC;
            }

        }
    }


    public void setFire()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit)) {
            if (points > 5) {
                Vector3 point = hit.point;
                point.y += 1;
                GameObject.Instantiate(flame, point, new Quaternion());
                addScore(-5);
            }
        }
    }

    public void loseHP(int loss)
    {
        this.hp -= loss;
    }

    public bool addScore(int points)
    {
        if(this.points + points < 0) {
            return false;
        }else {
            this.points += points;
            return true;
        }
        
    }



    public void isBurning()
    {
        burnTimer -= Time.deltaTime;
        if(burnTimer > 0) {
            hp -= 2;
        }
        
    }

    void OnTriggerEnter(Collider hit)
    {
        burnTimer = 3;
        if(hit.transform.tag == "Fire") {
            isOnFire = true;

        }
        
    }

    void OnTriggerExit(Collider hit)
    {
        if (hit.transform.tag == "Fire") {
            isOnFire = false;

        }
    }

    public void buildPoisonBush()
    {
        if (this.addScore(-25)) {
            GameObject.Instantiate(poisonBush, this.transform.position, new Quaternion());
        }

    }
    public void buildFireTree()
    {
        if (this.addScore(-50)) {
            Vector3 offset = transform.forward * 5;
            GameObject.Instantiate(fireTree, this.transform.position + offset, new Quaternion());
        }
        
        
    }

    public void throwRollingBall()
    {
        if (this.addScore(-50)) {
            Vector3 offset = transform.forward + new Vector3(0, 2, 0);
            GameObject.Instantiate(rollingBall, this.transform.position + offset, new Quaternion());
        }

    }

    public void createASCannon()
    {
        if (this.addScore(-150)) {
            Vector3 offset = transform.forward * 2 + new Vector3(0, 2, 0);
            GameObject.Instantiate(antiSpiderCannon, this.transform.position + offset, new Quaternion());
        }
    }

    public void heartPickup()
    {

        this.hp += 500;
    }

    public GameObject[] getClosestTorches()
    {
        return closestTorches;
    }

    public void staminaPickup()
    {
        this.minStamina += 50;
    }



}




