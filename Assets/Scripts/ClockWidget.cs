using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClockWidget : MonoBehaviour
{
	[Tooltip("data file, without extension")]
	public string inputfile;
	
	//to store simulation start time
	[Tooltip("Start hour")]
	public int startHour;
	[Tooltip("Start minute")]
	public int startMinute;
	[Tooltip("Start second")]
	public int startSecond;

	public int secondsPerUpdate;
	
	//time rate of change
	[Tooltip("seconds to elapse per frame")]
	public float troc;

	//should time change or not?
	[Tooltip("Should time change/continue?")]
	public Boolean play;

	[Tooltip("ROC text GameObject")]
	public Text rocText;
	
	//simulation time variables
	public float totalGameSeconds = 0;
	private int gameHour;
	private int gameMinute;
	private int gameSecond;
	private float previousGameSeconds;
	public string currentGameTime;

	//GameObjects that need to access the current game time. This is REALLY BAD PRACTICE but I'm on a time crunch
	public GameObject rw;
	public GameObject ww;
	public GameObject tw;

	//Text component for displaying the in-simulation time
	public Text displayTime;

	public void updateROC(float newValue)
	{
		troc = newValue * newValue;
		//Debug.Log(newValue * newValue);
	}

	void updateTime()
	{
		//Debug.Log(totalGameSeconds);
		//update total seconds
		totalGameSeconds += troc * Time.deltaTime;
		//Debug.Log(totalGameSeconds);

		//convert it to h:m:s
		gameHour = Mathf.FloorToInt(totalGameSeconds / 3600);
		//Debug.Log("gh"+gameHour.ToString());
		gameMinute = Mathf.FloorToInt((totalGameSeconds % 3600) / 60);
		//Debug.Log("gm"+gameMinute.ToString());
		gameSecond = Mathf.FloorToInt((totalGameSeconds % 3600) % 60);
		//Debug.Log("gs"+gameSecond.ToString());

		//create string of current game time
		string gh = gameHour.ToString();
		string gm = gameMinute.ToString();
		string gs = gameSecond.ToString();
		//Debug.Log(gs);
		if (gm.Length == 1)
		{
			gm = "0" + gm;
		}

		if (gs.Length == 1)
		{
			gs = "0" + gs;
		}
		//Debug.Log(gs);
		
		currentGameTime = gh + ":" + gm + ":" + gs;
		//Debug.Log(currentGameTime);
		
		//display current game time
		displayTime.text = currentGameTime;

		rocText.text = "x" + troc;
	}

	void updateWidgets()
	{
		//REALLY BAD PRACTICE:
		rw.GetComponent<RainWidget>().curTime = currentGameTime;
		ww.GetComponent<WindWidget>().curTime = currentGameTime;
		tw.GetComponent<TempWidget>().curTime = currentGameTime;
				
		rw.GetComponent<RainWidget>().secondsPerUpdate = secondsPerUpdate;
		/*
		rw.GetComponent<RainWidget>().inputfile = inputfile;
		ww.GetComponent<WindWidget>().inputfile = inputfile;
		tw.GetComponent<TempWidget>().inputfile = inputfile;*/
	}

	// Use this for initialization
	void Start ()
	{
		//get start seconds
		totalGameSeconds += startHour * 3600 + startMinute * 60 + startSecond;
		
		previousGameSeconds = totalGameSeconds;
		
		updateTime();
		
		updateWidgets();
	}
	
	// Update is called once per frame
	void Update()
	{
		if (play)
		{

			updateTime();
			//Debug.Log("time calculated");
			
			if (true)//Math.Abs(previousGameSeconds + secondsPerUpdate - totalGameSeconds) < 0)
			{
				updateWidgets();
				//Debug.Log("time updated");
				
				previousGameSeconds = totalGameSeconds;
			}
		}
	}
}

