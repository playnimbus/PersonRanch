using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Timer : MonoBehaviour 
{
    public Text text;
    private float elapsedTime;

    private House[] houses;


    void Start()
    {
         houses = GameObject.FindObjectsOfType<House>();
    }
	
	// Update is called once per frame
	void Update () 
    {
        elapsedTime += Time.deltaTime;
        int seconds = (int)elapsedTime;
        int minutes = seconds / 60;
        seconds = seconds % 60;
        text.text = minutes + ":" + seconds;

        for (int i = 0; i < houses.Length; i++)
            if (!houses[i].isFull) return;

        Application.LoadLevel(0);
	}


}
