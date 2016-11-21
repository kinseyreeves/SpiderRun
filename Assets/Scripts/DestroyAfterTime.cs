using UnityEngine;
using System.Collections;

public class DestroyAfterTime : MonoBehaviour {

    public float secondsTillDestory;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        secondsTillDestory -= Time.deltaTime;
        if (secondsTillDestory < 0)
        {
            Destroy(this.gameObject);
        }
	
	}
}
