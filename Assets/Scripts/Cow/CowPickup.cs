using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowPickup : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider col) //resets the health on collide, dont forget to check set trigger in unity
    {
        GameObject player = col.gameObject;
        ShipMovement cowPick = player.GetComponent<ShipMovement>();

        if (col.gameObject.CompareTag("Player"))
        {
            //if the player doesnt have a cow then pick it up
            if (cowPick.m_hasCow == false && cowPick.m_coolDown == false)
            {
                //move the cow to the right place before parenting
                Vector3 temp = player.transform.position;
                //col.gameObject.transform.position = temp;
                //col.transform.Translate(0, 2, 0);
                //col.transform.parent = this.transform;
                this.transform.position = temp;
                this.transform.Translate(0, 2, 0);
                this.transform.parent = player.transform;
                Debug.Log("Has Cow");
                cowPick.m_hasCow = true;
            }
        }
    }
}
