using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClockWidget : MonoBehaviour
{
	//to store simulation start time
	[Tooltip("Start hour")]
	public int startHour;
	[Tooltip("Start minute")]
	public int startMinute;
	[Tooltip("Start second")]
	public int startSecond;
	
	//time rate of change
	[Tooltip("seconds to elapse per frame")]
	public float troc;

	//should time change or not?
	[Tooltip("Should time change/continue?")]
	public Boolean play;

	//simulation time variables
	public float totalGameSeconds = 0;
	private int gameHour;
	private int gameMinute;
	private int gameSecond;
	public string currentGameTime;

	//GameObjects that need to access the current game time. This is REALLY BAD PRACTICE but I'm on a time crunch
	public GameObject rw;
	public GameObject ww;
	public GameObject tw;

	//Text component for displaying the in-simulation time
	public Text displayTime;

	// Use this for initialization
	void Start ()
	{
		//get start seconds
		totalGameSeconds += startHour * 3600 + startMinute * 60 + startSecond;
		//update total seconds
		totalGameSeconds += troc * Time.deltaTime;

		//convert it to h:m:s
		gameHour = Mathf.RoundToInt(totalGameSeconds % 60);
		gameMinute = Mathf.RoundToInt((totalGameSeconds - gameHour * 3600) % 60);
		gameSecond = Mathf.RoundToInt((totalGameSeconds - gameHour * 3600 - gameMinute * 60) % 60);

		//create string of current game time
		currentGameTime = gameHour.ToString() + ":" + gameMinute.ToString() + ":" + gameSecond.ToString();
		
		//display current game time
		displayTime.text = currentGameTime;
	}
	
	// Update is called once per frame
	void Update()
	{
		if (play)
		{
			totalGameSeconds += troc * Time.deltaTime;

			gameHour = Mathf.RoundToInt(totalGameSeconds % 60);
			gameMinute = Mathf.RoundToInt((totalGameSeconds - gameHour * 3600) % 60);
			gameSecond = Mathf.RoundToInt((totalGameSeconds - gameHour * 3600 - gameMinute * 60) % 60);

			currentGameTime = gameHour.ToString() + ":" + gameMinute.ToString() + ":" + gameSecond.ToString();

			//REALLY BAD PRACTICE:
			rw.GetComponent<RainWidget>().curTime = currentGameTime;
			ww.GetComponent<WindWidget>().curTime = currentGameTime;
			tw.GetComponent<TempWidget>().curTime = currentGameTime;

			displayTime.text = currentGameTime;
		}
	}
}

