﻿using UnityEngine;
using System.Collections;

public class Person : MonoBehaviour 
{
    public float runRadius;
    public bool hermit;
    [HideInInspector]
    public bool stuckToCar;
    public float avoidSpeed;

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
            yield return StartCoroutine(Walk(new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)) * 3));
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
            rigidbody.velocity = Vector3.zero;
            current = transform.position;
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
            rigidbody.velocity = Vector3.zero;
            Vector3 diff = (transform.position - car.transform.position);
            diff.Normalize();
            diff *= avoidSpeed * Time.deltaTime;

            rigidbody.MovePosition(transform.position + diff);
            yield return wait;
        }
    }

    private IEnumerator StickToCar()
    {
        WaitForFixedUpdate wait = new WaitForFixedUpdate();


        while (stuckToCar)
        {
            if (EscapedCar())
                break;

            Vector3 diff = car.GetFront() - transform.position;
            rigidbody.AddForce(diff * rigidbody.mass, ForceMode.Impulse);
            yield return wait;
        }

        stuckToCar = false;
        StartCoroutine(MainLoop());
    }


    private bool IsNearCar()
    {
        return Vector3.Distance(car.transform.position, transform.position) < runRadius;
    }

    private bool EscapedCar()
    {
        return Vector3.Distance(car.transform.position, transform.position) > runRadius * 0.5f;
    }

    public void AttachToCar()
    {
        // Check done to prevent bugginess
        if (gameObject.activeSelf)
        {
            StopAllCoroutines();
            stuckToCar = true;
            StartCoroutine(StickToCar());
        }
    }

    public void DetachFromCar()
    {
        StopAllCoroutines();
        rigidbody.velocity = Vector3.zero;
        stuckToCar = true;
        StartCoroutine(MainLoop());
    }
}
