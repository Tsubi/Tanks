using System.Collections;
using UnityEngine;

public class ShipMovement : MonoBehaviour
{
    public int m_PlayerNumber = 1;              // Used to identify which tank belongs to which player.  This is set by this tank's manager.
    public float m_Speed = 12f;                 // How fast the tank moves forward and back.
    public float m_TurnSpeed = 180f;            // How fast the tank turns in degrees per second.
    public AudioSource m_MovementAudio;         // Reference to the audio source used to play engine sounds. NB: different to the shooting audio source.
    public AudioClip m_EngineIdling;            // Audio to play when the tank isn't moving.
    public AudioClip m_EngineDriving;           // Audio to play when the tank is moving.
    public float m_PitchRange = 0.2f;           // The amount by which the pitch of the engine noises can vary.


    private string m_MovementAxisName;          // The name of the input axis for moving forward and back.
    private string m_TurnAxisName;              // The name of the input axis for turning.
    private Rigidbody m_Rigidbody;              // Reference used to move the tank.
    private float m_MovementInputValue;         // The current value of the movement input.
    private float m_TurnInputValue;             // The current value of the turn input.
    private float m_OriginalPitch;              // The pitch of the audio source at the start of the scene.

    public int m_powerTime;
    public bool m_speedBoost;
    public float m_boost;

    public bool m_hasCow;
    public bool m_coolDown;
    public int m_moonCows = 11;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }


    private void OnEnable()
    {
        // When the tank is turned on, make sure it's not kinematic.
        m_Rigidbody.isKinematic = false;

        // Also reset the input values.
        m_MovementInputValue = 0f;
        m_TurnInputValue = 0f;
        
        m_powerTime = 3;
        m_speedBoost = false;
        m_boost = 1.5f;

        m_hasCow = false;
        m_coolDown = false;
    }


    private void OnDisable()
    {
        // When the tank is turned off, set it to kinematic so it stops moving.
        m_Rigidbody.isKinematic = true;
    }


    private void Start()
    {
        // The axes names are based on player number.
        m_MovementAxisName = "Vertical" + m_PlayerNumber;
        m_TurnAxisName = "Horizontal" + m_PlayerNumber;

        // Store the original pitch of the audio source.
        m_OriginalPitch = m_MovementAudio.pitch;
    }


    private void Update()
    {
        // Store the value of both input axes.
        m_MovementInputValue = Input.GetAxis(m_MovementAxisName);
        m_TurnInputValue = Input.GetAxis(m_TurnAxisName);

        //distance between this object and the planet
        //float dist = Vector3.Distance(this.position, )
        //if the distance is < 30

        /*
         GameObject player = GameObject.FindWithTag("Player");
         ShipHealth damage = spawned.GetComponent<ShipHealth>();
         player.m_CanDamage = false;
         */

        EngineAudio();
    }

    void OnTriggerEnter(Collider col) //resets the health on collide, dont forget to check set trigger in unity
    {
        //if (col.gameObject.CompareTag("SpeedBoost"))
        //{
        //    if (m_speedBoost == false)
        //    {
        //        StartCoroutine(SpeedBoost(col));

        //        GameObject spawned = GameObject.Find("Powerup_Spawner");
        //        RandomPowerup spawner = spawned.GetComponent<RandomPowerup>();
        //        spawner.m_spawned = false;
        //        //GameObject.Find("Powerup_Spawner").m_spawned = false;
        //        col.gameObject.SetActive(false);
        //    }
        //}
        /*else if (col.gameObject.CompareTag("Cow"))
        {
            //if the player doesnt have a cow then pick it up
            if (m_hasCow == false && m_coolDown == false)
            {
                //move the cow to the right place before parenting
                Vector3 temp = this.transform.position;
                //col.gameObject.transform.position = temp;
                //col.transform.Translate(0, 2, 0);
                //col.transform.parent = this.transform;
                GameObject.FindWithTag("Cow").transform.position = temp;
                GameObject.FindWithTag("Cow").transform.Translate(0, 2, 0);
                GameObject.FindWithTag("Cow").transform.parent = this.transform;
                Debug.Log("Has Cow");
                m_hasCow = true;
            }
        }
        else if (col.gameObject.CompareTag("Moon"))
        {
            //if the player doesnt have a cow then pick it up
            if (m_hasCow == false && m_coolDown == false && m_moonCows > 0)
            {
                //move the cow to the right place before parenting
                m_moonCows--;

                Vector3 temp = this.transform.position;
                GameObject.FindWithTag("Cow").transform.position = temp;
                GameObject.FindWithTag("Cow").transform.Translate(0, 2, 0);
                GameObject.FindWithTag("Cow").transform.parent = this.transform;
                Debug.Log("Has Cow");
                m_hasCow = true;
            }
        }*/
        if (col.gameObject.CompareTag("Bullet"))
        {
            if (m_hasCow)
            {
                Debug.Log("Drop Cow");
                GameObject.FindWithTag("Cow").transform.parent = null;
                GameObject.FindWithTag("Cow").transform.Translate(0, -2, 0);
                m_hasCow = false;
                Debug.Log("Can't Cows");
                m_coolDown = true;
                StartCoroutine(CoolDown(col));
            }
        }
    }

    public IEnumerator SpeedBoost(Collider player)
    {
        m_speedBoost = true;
        Debug.Log("More Speed");

        float tempSpeed = m_Speed;
        float tempTurnSpeed = m_TurnSpeed;

        m_Speed *= m_boost;
        m_TurnSpeed *= m_boost;
        Transform[] ts = gameObject.GetComponentsInChildren<Transform>();
        Transform childTransform = null;
        foreach (Transform child in ts)
        {
            if(child.tag == "SpeedBoost_Feedback")
            {
                childTransform = child;
                print("found it");
            }
            //child is your child transform
        }

        childTransform.localScale = new Vector3(1, 1, 1);
        //GameObject.FindChildWithTag("SpeedBoost_Feedback").transform.localScale = new Vector3(1, 1, 1);

        //Pauses this funtion for this amount of time
        yield return new WaitForSeconds(m_powerTime);
        m_Speed = tempSpeed;
        m_TurnSpeed = tempTurnSpeed;
        childTransform.localScale = new Vector3(100, 100, 100);
        //GameObject.FindChildWithTag("SpeedBoost_Feedback").transform.localScale = new Vector3(100, 100, 100);
        m_speedBoost = false;
        Debug.Log("End of Speed Boost");
        //remove the shield model
    }

    public IEnumerator CoolDown(Collider player)
    {
        //Pauses this funtion for this amount of time
        yield return new WaitForSeconds(m_powerTime);
        m_coolDown = false;
        Debug.Log("Can Cow");
    }

    private void EngineAudio()
    {
        // If there is no input (the tank is stationary)...
        if (Mathf.Abs(m_MovementInputValue) < 0.1f && Mathf.Abs(m_TurnInputValue) < 0.1f)
        {
            // ... and if the audio source is currently playing the driving clip...
            if (m_MovementAudio.clip == m_EngineDriving)
            {
                // ... change the clip to idling and play it.
                m_MovementAudio.clip = m_EngineIdling;
                m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                m_MovementAudio.Play();
            }
        }
        else
        {
            // Otherwise if the tank is moving and if the idling clip is currently playing...
            if (m_MovementAudio.clip == m_EngineIdling)
            {
                // ... change the clip to driving and play.
                m_MovementAudio.clip = m_EngineDriving;
                m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                m_MovementAudio.Play();
            }
        }
    }


    private void FixedUpdate()
    {
        // Adjust the rigidbodies position and orientation in FixedUpdate.
        Move();
        Turn();
    }


    private void Move()
    {
        // Create a vector in the direction the tank is facing with a magnitude based on the input, speed and the time between frames.
        Vector3 movement = transform.forward * m_MovementInputValue * m_Speed * Time.deltaTime;

        // Apply this movement to the rigidbody's position.
        m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
    }


    private void Turn()
    {
        // Determine the number of degrees to be turned based on the input, speed and time between frames.
        float turn = m_TurnInputValue * m_TurnSpeed * Time.deltaTime;

        // Make this into a rotation in the y axis.
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);

        // Apply this rotation to the rigidbody's rotation.
        m_Rigidbody.MoveRotation(m_Rigidbody.rotation * turnRotation);
    }
}