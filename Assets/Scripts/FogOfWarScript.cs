using UnityEngine;
using System.Collections;

public class FogOfWarScript : MonoBehaviour {

    public Transform fogOfWarPlane;
    public Renderer FOWRenderer;
    public GameObject player;
    public PlayerManager playermanager;
    public float fogRadius;
    public Vector3 offset;
    public float y;


	// Use this for initialization
	void Start () {
        FOWRenderer = fogOfWarPlane.GetComponent<Renderer>();
        offset = Vector3.zero;
    }
	
	// Update is called once per frame
	void Update () {

        float radiusController = Mathf.InverseLerp(0, 1000, playermanager.hp);
        Vector3 camPos = Camera.main.WorldToScreenPoint(transform.position);
        Ray rayToPlayer = Camera.main.ScreenPointToRay(camPos);


        
            offset.x = Screen.width / 2;
            offset.z = Screen.height / 2;
            offset = offset - player.transform.position;
        

        FOWRenderer.material.SetVector("_Player_Pos", offset + player.transform.position);

        this.transform.position = new Vector3(this.transform.position.x, player.transform.position.y + y, this.transform.position.z);

        //player.transform.position + offset
        FOWRenderer.material.SetFloat("_FogRadius", fogRadius*radiusController);
	
	}
}
