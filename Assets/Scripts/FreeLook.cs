using UnityEngine;
using System.Collections;


/// <summary>
/// Handles camera movement. MoveCam variable will move the camera 
/// based on the 
/// </summary>
public class FreeLook : MonoBehaviour {

    // Use this for initialization

    
    public GameObject yPivot;
    public GameObject camera;


    public bool moveCam;


    public float rotSpeed, moveSpeed;


    private float xRot, yRot;
    private Vector3 movement, rotate;

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {


      

        movement = new Vector3(0, 0, 0);

        //handles input for movement forward and back and roll,
        //each direction has collision detection using a ray cast
        if (Input.GetKey(KeyCode.W)) {
            movement  +=  camera.transform.forward * moveSpeed*Time.deltaTime;
        }if (Input.GetKey(KeyCode.S)) {
            movement -= camera.transform.forward * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A)) {
            movement -= camera.transform.right * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D)) {
            movement += camera.transform.right*moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.Q)) {
            camera.transform.Rotate(0, 0, 1);
        }
        if (Input.GetKey(KeyCode.E)) {
            camera.transform.Rotate(0, 0, -1);
        }


        //increases move speed
        if (Input.GetKey(KeyCode.LeftShift))
        {
            movement = movement*5;
        }
            

        transform.position += movement * Time.deltaTime*moveSpeed;




        //handles rotation due to mouse movement
        if (moveCam)
        {
            xRot = Input.GetAxisRaw("Mouse X") * Time.deltaTime * rotSpeed;
            yRot = -Input.GetAxisRaw("Mouse Y") * Time.deltaTime * rotSpeed;
        }
        

        transform.Rotate(new Vector3(0, xRot, 0));
        yPivot.transform.Rotate(new Vector3(yRot, 0 , 0));
	
	}












}
