using UnityEngine;
using System.Collections;

public class RollingBallScript : MonoBehaviour {

    Rigidbody rigid;
    public float speed;
    public float initSpeed;


	void Start () {
        rigid = gameObject.GetComponent<Rigidbody>();
        rigid.AddForce(Vector3.forward*initSpeed);
	
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 acceleration = Vector3.zero;

        foreach (AccelerationEvent accEvent in Input.accelerationEvents) {
            acceleration += accEvent.acceleration * accEvent.deltaTime;
        }

        Vector3 direction = new Vector3(acceleration.x*200, 0.0f, acceleration.y*200);
        rigid.AddForce(direction*speed*Time.deltaTime);
	
	}
}
