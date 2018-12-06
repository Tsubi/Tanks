using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartManager : MonoBehaviour {

    public Transform m_SpawnPoint;                          // The position and direction the heart will have when it spawns.
    [HideInInspector] public int m_HeartNumber;            // This specifies which player this the manager for.
    [HideInInspector] public GameObject m_Instance;         // A reference to the instance of the heart when it is created.
    [HideInInspector] public int m_Wins;                    // The number of wins this player has so far.

    private heart m_heart;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // Used at the start of each round to put the hearts into it's default state.
    public void Reset()
    {
        m_Instance.transform.position = m_SpawnPoint.position;
        m_Instance.transform.rotation = m_SpawnPoint.rotation;

        m_Instance.SetActive(false);
        m_Instance.SetActive(true);
    }
}
