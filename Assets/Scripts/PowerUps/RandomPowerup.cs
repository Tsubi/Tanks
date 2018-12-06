using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPowerup : MonoBehaviour {
    public bool m_spawned;
    public GameObject m_Shield;
    public GameObject m_DoubleDamage;
    public GameObject m_Repair;
    public GameObject m_SpeedBoost;

    // Use this for initialization
    void Start () {
        m_spawned = false;
    }
	
	// Update is called once per frame
	void Update () {
		if (m_spawned == false)
        {
            m_spawned = true;
            StartCoroutine(spawn());
        }
        else if (m_Shield.gameObject.activeSelf == false && m_DoubleDamage.gameObject.activeSelf == false && m_Repair.gameObject.activeSelf == false && m_SpeedBoost.gameObject.activeSelf == false)
        {
            m_spawned = false;
        }
	}

    IEnumerator spawn()
    {
        //Pauses this funtion for this amount of time
        yield return new WaitForSeconds(1);
        int powerup = Random.Range(1, 4);
        switch (powerup)
        {
            case 1:
                GameObject m_powerup1 = Instantiate(m_Shield, transform.position, transform.rotation) as GameObject;
                //m_Shield.gameObject.SetActive(true);
                break;
            case 2:
                GameObject m_powerup2 = Instantiate(m_DoubleDamage, transform.position, transform.rotation) as GameObject;
                //m_DoubleDamage.gameObject.SetActive(true);
                break;
            case 3:
                GameObject m_powerup3 = Instantiate(m_Repair, transform.position, transform.rotation) as GameObject;
                //m_Repair.gameObject.SetActive(true);
                break;
            case 4:
                GameObject m_powerup4 = Instantiate(m_SpeedBoost, transform.position, transform.rotation) as GameObject;
                //m_SpeedBoost.gameObject.SetActive(true);
                break;
        }
    }
}
