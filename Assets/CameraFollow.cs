using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour 
{
    public GameObject target;

    private Vector3 distance;

    void Start()
    {
        distance = transform.position - target.transform.position;
    }
	
	// Update is called once per frame
	void FixedUpdate () 
    {
        Vector3 current = transform.position - target.transform.position;
        Vector3 updated = Vector3.Lerp(current, distance, 0.1f);
        transform.position = target.transform.position + updated;
	}
}
