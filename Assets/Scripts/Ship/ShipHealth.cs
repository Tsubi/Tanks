using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ShipHealth : MonoBehaviour
{
    public float m_StartingHealth = 100f;
    public Slider m_Slider;
    public Image m_FillImage;
    public Color m_FullHealthColor = Color.green;
    public Color m_ZeroHealthColor = Color.red;
    public GameObject m_ExplosionPrefab;

    private AudioSource m_ExplosionAudio;
    private ParticleSystem m_ExplosionParticles;
    private float m_CurrentHealth;
    private bool m_Dead;

    private int m_powerTime;
    public bool m_CanDamage;
    private bool m_DoubleDamage;

    Vector3 spawnPos;

    private void Awake()
    {
        m_ExplosionParticles = Instantiate(m_ExplosionPrefab).GetComponent<ParticleSystem>();
        m_ExplosionAudio = m_ExplosionParticles.GetComponent<AudioSource>();

        m_ExplosionParticles.gameObject.SetActive(false);

        spawnPos = transform.position;
    }


    private void OnEnable()
    {
        m_CurrentHealth = m_StartingHealth;
        m_Dead = false;

        m_CanDamage = true;
        m_DoubleDamage = false;
        m_powerTime = 3;

        SetHealthUI();
    }

    void OnTriggerEnter(Collider col) //resets the health on collide, dont forget to check set trigger in unity
    {
        Debug.Log("Collision!");
        if (col.gameObject.CompareTag("Repair"))
        {
            m_CurrentHealth = m_StartingHealth;
            SetHealthUI();

            GameObject spawned = GameObject.Find("Powerup_Spawner");
            RandomPowerup spawner = spawned.GetComponent<RandomPowerup>();
            spawner.m_spawned = false;

            //GameObject.Find("Powerup_Spawner").m_spawned = false;
            col.gameObject.SetActive(false);
        }
        else if (col.gameObject.CompareTag("Shield"))
        {
            StartCoroutine(Shield(col));

            GameObject spawned = GameObject.Find("Powerup_Spawner");
            RandomPowerup spawner = spawned.GetComponent<RandomPowerup>();
            spawner.m_spawned = false;

            //GameObject.Find("Powerup_Spawner").m_spawned = false;
            col.gameObject.SetActive(false);
        }
        else if (col.gameObject.CompareTag("FireRate"))
        {
            StartCoroutine(FireRate(col));

            GameObject spawned = GameObject.Find("Powerup_Spawner");
            RandomPowerup spawner = spawned.GetComponent<RandomPowerup>();
            spawner.m_spawned = false;

            //GameObject.Find("Powerup_Spawner").m_spawned = false;
            col.gameObject.SetActive(false);
        }
    }

    IEnumerator Shield(Collider player)
    {
        Debug.Log("No Damage");
        m_CanDamage = false;

        GameObject.Find("shield_feedback").transform.localScale = new Vector3(1, 1, 1);

        //Pauses this funtion for this amount of time
        yield return new WaitForSeconds(m_powerTime);
        Debug.Log("End of Shield");
        m_CanDamage = true;
        GameObject.Find("shield_feedback").transform.localScale = new Vector3(100, 100, 100);
        Debug.Log("Yes Damage");
        //remove the shield model
    }

    IEnumerator FireRate(Collider player)
    {
        Debug.Log("Double Damage");
        m_DoubleDamage = true;

        GameObject.Find("doubleDamage_feedback").transform.localScale = new Vector3(1, 1, 1);

        //Pauses this funtion for this amount of time
        yield return new WaitForSeconds(m_powerTime);

        m_DoubleDamage = false;
        GameObject.Find("doubleDamage_feedback").transform.localScale = new Vector3(100, 100, 100);
    }

    public void TakeDamage(float amount)
    {
        Debug.Log("Hit");
        if (m_CanDamage == true)
        {
            Debug.Log("Damaged");
            // Reduce current health by the amount of damage done.
            if (m_DoubleDamage == false)
            {
                m_CurrentHealth -= amount;
            }
            else
            {
                m_CurrentHealth -= 2*amount;
            }

            //drop the cow if they have a cow

            // Change the UI elements appropriately.
            SetHealthUI();

            // If the current health is at or below zero and it has not yet been registered, call OnDeath.
            if (m_CurrentHealth <= 0f && !m_Dead)
            {
                OnDeath();
            }
        }
    }

    private void SetHealthUI()
    {
        // Set the slider's value appropriately.
        m_Slider.value = m_CurrentHealth;

        // Interpolate the color of the bar between the choosen colours based on the current percentage of the starting health.
        m_FillImage.color = Color.Lerp(m_ZeroHealthColor, m_FullHealthColor, m_CurrentHealth / m_StartingHealth);
    }

    private void OnDeath()
    {
        // Set the flag so that this function is only called once.
        //m_Dead = true;

        // Move the instantiated explosion prefab to the tank's position and turn it on.
        m_ExplosionParticles.transform.position = transform.position;
        m_ExplosionParticles.gameObject.SetActive(true);

        // Play the particle system of the tank exploding.
        m_ExplosionParticles.Play();

        // Play the tank explosion sound effect.
        m_ExplosionAudio.Play();

        //Instead move the tank back to their corner
        m_CurrentHealth = m_StartingHealth;
        SetHealthUI();
        gameObject.transform.position = spawnPos;
    }
}