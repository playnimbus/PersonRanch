using UnityEngine;
using System.Collections;

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

    private float targetSpeed = 0;
    private float forward;
    private Rigidbody rigidbody;

    void Awake()
    {
        instance = this;
        rigidbody = GetComponent<Rigidbody>();
    }

	// Update is called once per frame
	void Update () 
    {
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
            factor = Mathf.Max(factor, 0.5f);
            forward -= turnRate * factor * Time.deltaTime;
        }

        transform.rotation = Quaternion.AngleAxis(forward * Mathf.Rad2Deg * -1, Vector3.up);
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
        return transform.position + (new Vector3(Mathf.Cos(forward), 0, Mathf.Sin(forward))) * 2;
    }
}
