using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * This is essentially a top level script because all the others rely on this one to provide the simulation time
 */


public class ClockWidget : MonoBehaviour
{
	[Tooltip("data file, without extension")]
	public string inputfile;
	
	//to store simulation start time
	[Tooltip("Start hour")]
	public int startHour = 0;
	[Tooltip("Start minute")]
	public int startMinute = 0;
	[Tooltip("Start second")]
	public int startSecond = 0;

	public int secondsPerUpdate;
	
	//time rate of change
	[Tooltip("seconds (in simulation time) to elapse per frame")]
	public float troc;

	//should time change or not?
	[Tooltip("Should time change/continue?")]
	public Boolean play;

	[Tooltip("ROC text GameObject")]
	public Text rocText;
	[Tooltip("start time text GameObject")]
	public Text StartTimeText;
	
	//simulation time variables
	public float totalGameSeconds = 0;
	private int gameHour;
	private int gameMinute;
	private int gameSecond;
	private float previousGameSeconds;
	public string currentGameTime;

	//GameObjects that need to access the current game time. This is bad practice but I'm on a time crunch
	public GameObject rw;
	public GameObject ww;
	public GameObject tw;

	//Text component for displaying the in-simulation time
	public Text displayTime;

	//used by troc slider to update time ROC
	public void updateROC(float newValue)
	{
		troc = Mathf.Round(newValue * newValue * 10) / 10f;
	}

	//used by start hour slider to update startHour
	public void updateStartHour(float newValue)
	{
		startHour = Mathf.RoundToInt(newValue);
	}

	//used by start min slider to update startMinute;
	public void updateStartMin(float newValue)
	{
		startMinute = Mathf.RoundToInt(newValue);
	}

	//used by jump button to update totalGameSeconds to match start time variables
	public void updateStartTime()
	{
		totalGameSeconds = startHour * 3600 + startMinute * 60 + startSecond;
	}

	void updateTime()
	{
		//update total seconds
		totalGameSeconds += troc * Time.deltaTime;

		//calculate h/m/s from total game seconds
		gameHour = Mathf.FloorToInt(totalGameSeconds / 3600);
		gameMinute = Mathf.FloorToInt((totalGameSeconds % 3600) / 60);
		gameSecond = Mathf.FloorToInt((totalGameSeconds % 3600) % 60);

		//create string of current game time
		string gh = gameHour.ToString();
		string gm = gameMinute.ToString();
		string gs = gameSecond.ToString();
		
		if (gm.Length == 1)
		{
			gm = "0" + gm;
		}
		if (gs.Length == 1)
		{
			gs = "0" + gs;
		}
		
		currentGameTime = gh + ":" + gm + ":" + gs;
		
		//display current game time
		displayTime.text = currentGameTime;
		//display current time rate of change
		rocText.text = "x" + troc;

		
		string sh = startHour.ToString();
		string sm = startMinute.ToString();
		if (sm.Length == 1)
		{
			sm = "0" + sm;
		}
		
		//display current start time
		StartTimeText.text = sh + ":" + sm;
	}

	void updateWidgets()
	{
		//update current time for other widgets that need it
		//this is bad practice. It should be generalized, but I'm on a time crunch
		rw.GetComponent<RainWidget>().curTime = currentGameTime;
		ww.GetComponent<WindWidget>().curTime = currentGameTime;
		tw.GetComponent<TempWidget>().curTime = currentGameTime;
		
		rw.GetComponent<RainWidget>().secondsPerUpdate = secondsPerUpdate;
		/*
		rw.GetComponent<RainWidget>().inputfile = inputfile;
		ww.GetComponent<WindWidget>().inputfile = inputfile;
		tw.GetComponent<TempWidget>().inputfile = inputfile;
		*/
	}

	// Use this for initialization
	void Start ()
	{
		//set start seconds
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
			
			if (true)//Math.Abs(previousGameSeconds + secondsPerUpdate - totalGameSeconds) < 0)
			{
				updateWidgets();
				
				previousGameSeconds = totalGameSeconds;
			}
		}
	}
}

