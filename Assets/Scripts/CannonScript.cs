using UnityEngine;
using System.Collections;

public class CannonScript : MonoBehaviour {

    
    public float maxDist;

    public CannonBallEmitter cannonEmitter;

    void Start () {
	
	}
	
	
	void Update () {
        GameObject closestSpider = FindClosestEnemy();
        if (closestSpider != null) {
            Vector3 lookDirection = transform.position - closestSpider.transform.position;
            transform.rotation = Quaternion.LookRotation(lookDirection * -1);
        }
	
	}


    GameObject FindClosestEnemy()
    {
        GameObject[] spiders;
        spiders = GameObject.FindGameObjectsWithTag("Spider");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject spider in spiders) {
            Vector3 diff = spider.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if(curDistance < distance) {
                closest = spider;
                distance = curDistance;
            }
        }
        if(distance < maxDist) {
            cannonEmitter.isInRange = true;
            return closest;
            
        }
        cannonEmitter.isInRange = false;
        return null;
    }
}
