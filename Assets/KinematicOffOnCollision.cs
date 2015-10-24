using UnityEngine;
using System.Collections;

public class KinematicOffOnCollision : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision collision) {
		if(collision.gameObject.tag == "Player"){
			gameObject.GetComponent<Rigidbody>().isKinematic = false;

			AudioSource source = gameObject.GetComponent<AudioSource>();
			if(source!=null){
				source.Play();
			}

		}
	}

}
