using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

    public GameObject player;
    // Use this for initialization
    private Vector3 position;
    public Vector3 relativepos;
    public int xRel, zRel, yRel;
 

	void Start () {

        
        

    }
	
	// Update is called once per frame
	void Update () {
        
        relativepos = new Vector3(xRel, zRel, yRel);
        this.transform.position = player.transform.position + relativepos;

    }
}
