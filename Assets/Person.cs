using UnityEngine;
using System.Collections;

public class Person : MonoBehaviour 
{
    public float runRadius;
    private Car car;
    private Rigidbody rigidbody;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        this.car = Car.Instance;
        StartCoroutine(MainLoop());
    }

    public void SetCar(Car car)
    {
        this.car = car;
    }

    IEnumerator MainLoop()
    {
        while (true)
        {
            yield return StartCoroutine(Idle(Random.value * 2f));
            if (IsNearCar()) yield return StartCoroutine(AvoidCar());
            yield return StartCoroutine(Walk(new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1)) * 3));
            if (IsNearCar()) yield return StartCoroutine(AvoidCar());
        }
    }

    IEnumerator Idle(float time)
    {
        float start = Time.time;
        while (Time.time < start + time)
        {
            if (IsNearCar())
                break;

            yield return null;
        }
    }

    IEnumerator Walk(Vector3 delta)
    {
        WaitForFixedUpdate wait = new WaitForFixedUpdate();
        Vector3 start = transform.position;
        Vector3 end = start + delta;
        Vector3 current = start;

        while (current != end)
        {
            if (IsNearCar())
                break;

            current = Vector3.MoveTowards(current, end, 1f * Time.deltaTime);
            rigidbody.MovePosition(current);
            yield return wait;
        }
    }

    IEnumerator AvoidCar()
    {
        WaitForFixedUpdate wait = new WaitForFixedUpdate();

        while (Vector3.Distance(car.transform.position, transform.position) < runRadius * 1.25f)
        {
            Vector3 diff = (transform.position - car.transform.position);
            diff.Normalize();
            diff *= 1f * Time.deltaTime;

            rigidbody.MovePosition(transform.position + diff);
            yield return wait;
        }
    }

    private bool IsNearCar()
    {
        return Vector3.Distance(car.transform.position, transform.position) < runRadius;
    }

}
