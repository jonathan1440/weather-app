using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Utility;

public class IsItRaining : MonoBehaviour {

	//Name of input data file, no extension
	public string inputfile;
	
	// List for holding data from CSV reader
	private List<Dictionary<string, object>> pointList;
	
	// 0.01 inches of rain per 1 rain counter value
	public float rainCounterUnits = 0.1f;

	//time since last rain counter increase within which it will be considerded to be raining.
	public int timeSinceRCIncrease = 60;

	//is it raining?
	private bool isItCurrentlyRaining = false;
	
	//Set default raining/not raining messages
	public string rainingMsg = "It is raining.";
	public string notRainingMSg = "It is not raining.";
	
	// Use this for initialization
	void Start ()
	{
		//Set pointlist to results of function Reader with argument inputfile
		pointList = CSVReader.Read(inputfile);
		
		//Convert timeSinceRCIncrease from seconds to data entries
		//Since they are both floats, all decimals will be ignored
		timeSinceRCIncrease = timeSinceRCIncrease / 15;
	}
	
	// Update is called once per frame
	void Update ()
	{
		//Gotta check for new data each frame
		
		//Set pointlist to results of function Reader with argument inputfile
		pointList = CSVReader.Read(inputfile);
		
		//Check to see if its raining
		//Get length of pointList aka data
		int last = pointList.Count;
		
		//Get a baseline for the raincounter
		float mostRecentRC = Convert.ToSingle(pointList[last - 1]["rain counter"]);
		for (int i = last - 1 - timeSinceRCIncrease; i < last; i++)
		{
			isItCurrentlyRaining = false;
			float rc = Convert.ToSingle(pointList[i]["rain counter"]);

			if (!(rc < mostRecentRC)) continue;
			isItCurrentlyRaining = true;
			break;
		}

		//set message to be displayed
		if (isItCurrentlyRaining)
		{
			transform.guiText = rainingMsg;
		}
		
		
	}
}
