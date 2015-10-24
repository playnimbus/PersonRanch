using UnityEngine;
using System.Collections;

public class rotationoflights : MonoBehaviour {
    public float speed;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        speed++;
        gameObject.transform.rotation = new Quaternion(gameObject.transform.rotation.x, gameObject.transform.rotation.y + speed, gameObject.transform.rotation.z, gameObject.transform.rotation.w);
	}
}
