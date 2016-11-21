using UnityEngine;
using System.Collections;
using System;

public class CannonBallEmitter : MonoBehaviour {
    public float delay;
    private float resetDelay;

    public GameObject cannonBall;
    public float forwardForce;

    

    public bool isInRange;

    // Use this for initialization
    void Start () {
        resetDelay = delay;
	}
	
	// Update is called once per frame
	void Update () {
        if(delay < 0 && isInRange) {
            delay = resetDelay;
            fireShot();
        }
        else {
            delay -= Time.deltaTime;
        }
	
	}

    private void fireShot()
    {
        GameObject tempBall;
        tempBall = GameObject.Instantiate(cannonBall, transform.position, transform.rotation) as GameObject;

        Rigidbody tempRigid;
        tempRigid = tempBall.GetComponent<Rigidbody>();

        tempRigid.AddForce(transform.forward * forwardForce);

        Destroy(tempBall, 5);
    }
}
