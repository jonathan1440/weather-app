using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Utility;

public class RainWidget : MonoBehaviour {

	//Name of input data file, no extension
	[Tooltip("Name of input data file, no extension.")]
	public string inputfile;
	
	// List for holding data from CSV reader
	private List<Dictionary<string, object>> pointList;
	
	// 0.01 inches of rain per 1 rain counter value
	[Tooltip("__inches of rain per 1 rain counter unit")]
	public float rainCounterUnits = 0.01f;

	//time since last rain counter increase within which it will be considerded to be raining.
	[Tooltip("time since last rain counter increase within which it will be considered to be raining")]
	public int timeSinceRCIncrease = 60;

	//Set default raining/not raining messages
	[Tooltip("Default \"it is raining\" message")]
	public string rainingMsg;
	[Tooltip("Default \"it is not raining\" message")]
	public string notRainingMsg;

	//for text components to edit
	[Tooltip("Link to text component for saying if it is raining")]
	public Text isRainingTextMsg;
	[Tooltip("Link to text ")]
	public Text rainDepthMsg;
	
	//store most recent data
	private int mostRecent;
	//is it raining?
	[Tooltip("Is it currently raining?")]
	public bool isItCurrentlyRaining = false;
	//store rain depth
	[Tooltip("Rain fallen since start of day")]
	public float rainDepth;
	
	//time to match with times in data
	public string curTime;
	
	//store old curTime, to check if the mostRecent data needs to be updated
	private string oldCurTime;

	public int secondsPerUpdate;

	private void updateRainConstants()
	{
		//get day's rainDepth
		rainDepth = Convert.ToSingle(pointList[mostRecent]["rain counter"]) - Convert.ToSingle(pointList[0]["rain counter"]);
		
		//convert rain depth to inches
		rainDepth *= rainCounterUnits;
		
		//Round off rainDepth
		//rainDepth = Mathf.RoundToInt(rainDepth * 100) / 100;
		
		//Convert timeSinceRCIncrease from seconds to data entries
		//Since they are both floats, all decimals will be ignored
		timeSinceRCIncrease = timeSinceRCIncrease / (secondsPerUpdate + 1);
		
		//check to see if it is raining
		//Get a baseline for the raincounter
		var mostRecentRC = Convert.ToSingle(pointList[mostRecent]["rain counter"]);
		for (var i = Math.Max(mostRecent - timeSinceRCIncrease, 0); i < mostRecent; i++)
		{
			isItCurrentlyRaining = false;
			var rc = Convert.ToSingle(pointList[i]["rain counter"]);

			if (!(rc < mostRecentRC)) continue;
			isItCurrentlyRaining = true;
			break;
		}
	}
	
	private int getMostRecentData(string curTime)
	{
		int mostRecent = 0;
		
		for(int i = 0; i < pointList.Count;)
		{
			DateTime it = DateTime.ParseExact(pointList[i]["time"].ToString(), "H:mm:ss", System.Globalization.CultureInfo.CurrentCulture);
			DateTime t = DateTime.ParseExact(curTime, "H:mm:ss", System.Globalization.CultureInfo.CurrentCulture);
			int compared = DateTime.Compare(it, t);
			
			if (compared == 0)
			{
				mostRecent = i;
				break;
			}

			if (compared < 0)
			{
				i++;
			}
			
			if (compared > 0)
			{
				mostRecent = i-1;
				break;
			}
		}

		return mostRecent;
	}

	// Use this for initialization
	private void Start ()
	{
		//Set pointlist to results of function Reader with argument inputfile
		pointList = CSVReader.Read(inputfile);
		
		//set default messages in case something below goes wrong
		isRainingTextMsg.text = notRainingMsg;
		
		//store current time
		oldCurTime = curTime;
		
		//get most recent data
		mostRecent = getMostRecentData(curTime);
		
		updateRainConstants();
	}
	
	private void updateText()
	{
		//set message to be displayed
		if (isItCurrentlyRaining)
		{
			isRainingTextMsg.text = rainingMsg;
		}
		else
		{
			isRainingTextMsg.text = notRainingMsg;
		}
		
		rainDepthMsg.text = rainDepth.ToString().Substring(0,Math.Min(rainDepth.ToString().Length,5)) + " in";
	}
	
	// Update is called once per frame
	private void Update ()
	{
		updateRainConstants();
		
		updateText();

		// if time has changed, update data
		if (oldCurTime != curTime)
		{
			oldCurTime = curTime;
			
			mostRecent = getMostRecentData(curTime);
		}
	}
}
