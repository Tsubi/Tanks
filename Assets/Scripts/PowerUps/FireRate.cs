using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireRate : MonoBehaviour {

    public float speed = 20f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.down, speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider col) //resets the health on collide, dont forget to check set trigger in unity
    {
        Debug.Log("Collision!");

        GameObject player = col.gameObject;
        ShipHealth shipHealth = player.GetComponent<ShipHealth>();

        if (col.gameObject.CompareTag("Player"))
        {
            StartCoroutine(shipHealth.FireRate(col));

            GameObject spawned = GameObject.Find("Powerup_Spawner");
            RandomPowerup spawner = spawned.GetComponent<RandomPowerup>();
            spawner.m_spawned = false;

            this.gameObject.SetActive(false);
        }
    }
}
