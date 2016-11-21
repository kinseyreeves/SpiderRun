using UnityEngine;
using System.Collections;

public class StaminaScript : MonoBehaviour {

    GameObject player;
    PlayerManager playerManager;
    void Start()
    {

        player = GameObject.Find("Player");
        playerManager = player.GetComponent<PlayerManager>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(player.transform.position, transform.position) < 3)
        {
            playerManager.staminaPickup();
            Destroy(this.gameObject);
        }


    }
}
