using UnityEngine;
using System.Collections.Generic;

public class House : MonoBehaviour 
{
    List<Person> people = new List<Person>();

    void OnCollisionEnter(Collision collision)
    {
        Person p = collision.gameObject.GetComponent<Person>();
        if (p != null)
        {
            if (p.stuckToCar)
            {
                if (p.hermit)
                {
                    foreach (Person person in people)
                    {
                        person.gameObject.SetActive(true);
                        p.transform.position = (person.transform.position - transform.position).normalized * 4 + transform.position;
                        person.GetComponent<Rigidbody>().AddForce((person.transform.position - transform.position).normalized * person.GetComponent<Rigidbody>().mass, ForceMode.Impulse);
                    }
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
        }
    }
}
