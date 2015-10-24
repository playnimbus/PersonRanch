using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody))]
public class Car : MonoBehaviour 
{
    public static Car Instance
    {
        get { return instance; }
    }

    private static Car instance;

    public float forwardAcceleration;
    public float maxForwardSpeed;
    public float reverseAcceleration;
    public float maxReverseSpeed;
    public float turnRate;

    public AudioClip initialRev;
    public AudioClip loopRev;
    public AudioClip stopRev;

    private float targetSpeed = 0;
    private float forward;
    private Rigidbody rigidbody;
    private AudioSource audioSource;

    void Awake()
    {
        instance = this;
        rigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = initialRev;
        audioSource.loop = false;
    }

	// Update is called once per frame
	void Update () 
    {
        bool wasStill = Mathf.Abs(targetSpeed) < 0.1f;

        if (Input.GetKey(KeyCode.W))
        {
            targetSpeed += forwardAcceleration * Time.deltaTime;
            targetSpeed = Mathf.Clamp(targetSpeed, -maxReverseSpeed, maxForwardSpeed);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            targetSpeed -= reverseAcceleration * Time.deltaTime;
            targetSpeed = Mathf.Clamp(targetSpeed, -maxReverseSpeed, maxForwardSpeed);
        }
        else
        {
            
            targetSpeed = Mathf.Lerp(targetSpeed, 0, 0.1f);
        }

        if (Input.GetKey(KeyCode.A))
        {
            float factor = targetSpeed > 0 ? targetSpeed / maxForwardSpeed : -targetSpeed / maxReverseSpeed;
            factor = Mathf.Max(factor, 0.5f);
            forward += turnRate * factor * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            float factor = targetSpeed > 0 ? targetSpeed / maxForwardSpeed : -targetSpeed / maxReverseSpeed;
            factor = Mathf.Max(factor*2f, 0.5f);
            forward -= turnRate * factor * Time.deltaTime;
        }

        bool moving = Mathf.Abs(targetSpeed) > 0.1f;

        // Started moving
        if (wasStill && moving)
        {
            if(audioSource.clip != initialRev || !audioSource.isPlaying)
            {
                audioSource.Stop();
                audioSource.clip = initialRev;
                audioSource.Play();
            }
        }

        // Stopped moving
        if (!wasStill && !moving)
        {
            if (audioSource.clip != stopRev || !audioSource.isPlaying)
            {
                audioSource.Stop();
                audioSource.clip = stopRev;
                audioSource.Play();
            }
        }

        transform.rotation = Quaternion.AngleAxis(forward * Mathf.Rad2Deg * -1, Vector3.up);

        UpdateAudio();
	}

    void UpdateAudio()
    {
        bool moving = Mathf.Abs(targetSpeed) > 0.1f;
        if (!audioSource.isPlaying && moving)
        {
            audioSource.clip = loopRev;
            audioSource.Play();
        }
    }

    void FixedUpdate()
    {
        Vector3 vel = rigidbody.velocity;
        vel.y = 0;
        Vector3 targetVel = new Vector3(Mathf.Cos(forward), 0, Mathf.Sin(forward)) * targetSpeed;
        Vector3 deltaVel = targetVel - vel;
        Vector3 force = deltaVel * rigidbody.mass;
        rigidbody.AddForce(force, ForceMode.Impulse);
    }

    void OnCollisionEnter(Collision collision)
    {
        Person p = collision.gameObject.GetComponent<Person>();
        if (p != null)
        {
            p.AttachToCar();
        }
    }

    public Vector3 GetFront()
    {
        return transform.position + new Vector3(Mathf.Cos(forward), 0, Mathf.Sin(forward));
    }
}
