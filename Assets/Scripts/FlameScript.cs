using UnityEngine;
using System.Collections;

public class FlameScript : MonoBehaviour {

    public float burnTimer;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if(burnTimer < 0) {
            Destroy(this.gameObject);
        }
        else {
            burnTimer -= Time.deltaTime;
        }
	
	}
}
