using UnityEngine;
using System.Collections;

public class PersonSpawner : MonoBehaviour {

	public Terrain terrain;
	private Vector3 terrainBounds;
	// Use this for initialization

	void Awake(){
		terrainBounds = terrain.terrainData.size;
		
		for(int i = 0; i < 50;i++){
			GameObject instance = Instantiate(Resources.Load("Person", typeof(GameObject))) as GameObject;
			instance.transform.position = new Vector3(Random.Range(1,terrainBounds.x -1),1,Random.Range(1,terrainBounds.z -1));
		}
	}
}
