using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class House : MonoBehaviour 
{
    public int familySize;
    public Text counter;

    private List<Person> people = new List<Person>();
    private HashSet<Person> avoid = new HashSet<Person>();
    private List<Person> hermits = new List<Person>();

    void Start()
    {
        UpdateText();
    }

    void OnCollisionEnter(Collision collision)
    {
        Person p = collision.gameObject.GetComponent<Person>();
        if (p != null)
        {
            if (p.stuckToCar && !avoid.Contains(p))
            {
                if (p.hermit)
                {
                    foreach (Person person in people)
                    {
                        person.gameObject.SetActive(true);
                        //person.transform.position = (person.transform.position - transform.position).normalized + transform.position;
                        //person.stuckToCar = false;
                        person.GetComponent<Rigidbody>().AddForce((person.transform.position - transform.position).normalized * person.GetComponent<Rigidbody>().mass, ForceMode.Impulse);
                        avoid.Add(person);
                    }
                    Invoke("ClearAvoid", 5f);
                    people.Clear();

                    p.DetachFromCar();
                    p.gameObject.SetActive(false);
                    hermits.Add(p);
                    Invoke("ReleaseHermits", 5f);
                }
                else
                {
                    if (!isFull)
                    {
                        p.DetachFromCar();
                        p.gameObject.SetActive(false);
                        people.Add(p);
                    }
                }
            }

            UpdateText();
        }
    }

    void Update()
    {
        Vector3 camPos = Camera.main.transform.position;
        camPos.y = transform.position.y;
        counter.transform.rotation = Quaternion.LookRotation(transform.position - camPos);
    }

    private void UpdateText()
    {
        counter.text = people.Count + "/" + familySize;
    }

    public bool isFull
    {
        get { return people.Count == familySize; }
    }

    void ClearAvoid()
    {
        avoid.Clear();
    }

    void ReleaseHermits()
    {
        foreach (Person person in hermits)
        {
            person.gameObject.SetActive(true);
            // person.transform.position = (person.transform.position - transform.position).normalized + transform.position;
            person.stuckToCar = false;
            person.GetComponent<Rigidbody>().AddForce((person.transform.position - transform.position).normalized * person.GetComponent<Rigidbody>().mass, ForceMode.Impulse);
        }

        hermits.Clear();
    }
}
