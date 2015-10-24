using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class House : MonoBehaviour 
{
    public int familySize;
    public Text counter;

    private List<Person> people = new List<Person>();
    private HashSet<Person> avoid = new HashSet<Person>();

    void Start()
    {
        UpdateText();
    }

    void OnCollisionEnter(Collision collision)
    {
        Person p = collision.gameObject.GetComponent<Person>();
        if (p != null)
        {
            if (isFull) return;

            if (p.stuckToCar && !avoid.Contains(p))
            {
                if (p.hermit)
                {
                    foreach (Person person in people)
                    {
                        person.gameObject.SetActive(true);
                        p.transform.position = (person.transform.position - transform.position).normalized + transform.position;
                        p.stuckToCar = false;
                        person.GetComponent<Rigidbody>().AddForce((person.transform.position - transform.position).normalized * person.GetComponent<Rigidbody>().mass, ForceMode.Impulse);
                        avoid.Add(person);
                    }
                    Invoke("ClearAvoid", 5f);
                    people.Clear();

                    p.DetachFromCar();
                    p.gameObject.SetActive(false);
                    people.Add(p);
                }
                else
                {
                    
                    p.DetachFromCar();
                    p.gameObject.SetActive(false);
                    people.Add(p);
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

    private bool isFull
    {
        get { return people.Count == familySize; }
    }

    void ClearAvoid()
    {
        avoid.Clear();
    }
}
