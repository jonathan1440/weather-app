﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class WindWidget : MonoBehaviour {

	[Tooltip("Name of input data file, no extension")]
	public string inputfile;
	
	// List for holding data from CSV reader
	private List<Dictionary<string, object>> pointList;
	
	// allows this script to change the wind speed actively being displayed
	[Tooltip("text component for wind speed message")]
	public Text windSpeedMsg;
	[Tooltip("text component for avg wind velocity message")]
	public Text avgWindVelocityMsg;

	// presentation fail-safe
	[Tooltip("default wind speed message")]
	public string defaultWindSpeedMsg;
	[Tooltip("default avg velocity message")]
	public string defaultAvgVelocityMsg;
	
	// data storage
	public float windSpeed;
	public string windDirection;
	public string avgWindDirection;
	public float avgSpeed = 0;
	
	// pointList index of most recent data used by the simulation
	public int mostRecent;
	// current simulation time, compared with data timestamps in data file
	public string curTime;
	// store old curTime, to check if the mostRecent data needs to be updated
	private string oldCurTime;
	
	// to store list of possible wind directions
	// <text direction, direction angle>
	private Dictionary<string, float> cardinalDirections = new Dictionary<string, float>();

	
	private void updateWindData()
	{
		// get wind direction
		windDirection = pointList[mostRecent]["wind direction"].ToString();
		
		// get wind speed
		windSpeed = Convert.ToSingle(pointList[mostRecent]["wind speed"]);
		
		// sum of wind directions 
		float sumDirections = 0;
		// reset avgSpeed
		avgSpeed = 0;
		
		// iterate through data list to sum up the "wind direction" and "wind speed" values
		// doing this for loop every time allow me to not have to make a special case for when the simulation clock
		//  makes a jump
		for (int i = 0; i < mostRecent; i ++)
		{
			sumDirections += cardinalDirections[pointList[i]["wind direction"].ToString()];
			avgSpeed = avgSpeed + Convert.ToSingle(pointList[i]["wind speed"]);
		}

		//complete the averaging of the wind directions
		float avgDir = sumDirections / mostRecent;
		//to round it to the nearest cardinal direction, we first have to enable it to round to the nearest int
		avgDir /= 22.5f;
		//round it to nearest integer
		avgDir = Mathf.Round(avgDir) * 22.5f;
		//get the corresponding cardinal direction 
		avgWindDirection = cardinalDirections.FirstOrDefault(x => x.Value == avgDir).Key;
		
		//complete the averaging of the avg speed
		avgSpeed = Mathf.Round((avgSpeed*10)/(mostRecent+1))/10;
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
	void Start () {
		//Set pointlist to results of function Reader with argument inputfile
		pointList = CSVReader.Read(inputfile);
		
		//set list of possible windspeeds
		cardinalDirections.Add("E",0);
		cardinalDirections.Add("ESE",-22.5f);
		cardinalDirections.Add("SE",-45);
		cardinalDirections.Add("SSE",-67.5f); //
		cardinalDirections.Add("S",-90); //
		cardinalDirections.Add("SSW",-112.5f); //
		cardinalDirections.Add("SW",-135);
		cardinalDirections.Add("WSW",-157.5f); //
		cardinalDirections.Add("W",-180); //
		cardinalDirections.Add("WNW",-202.5f);
		cardinalDirections.Add("NW",-225);
		cardinalDirections.Add("NNW",-247.5f); //
		cardinalDirections.Add("N",-270); //
		cardinalDirections.Add("NNE",-292.5f); //
		cardinalDirections.Add("NE",-315);
		cardinalDirections.Add("ENE",-337.5f);

		//set default messages in case something below goes wrong
		windSpeedMsg.text = defaultWindSpeedMsg;
		avgWindVelocityMsg.text = defaultAvgVelocityMsg;
		
		//store current time
		oldCurTime = curTime;
		
		//get most recent data index
		mostRecent = getMostRecentData(curTime);
		
		updateWindData();
	}

	private void updateText()
	{
		//get most recent wind speed, add it in to default wind speed message
		windSpeedMsg.text = windDirection+"\n"+windSpeed.ToString() + " mph";
		
		//set avg velocity message
		avgWindVelocityMsg.text = "avg. windspeed\nof " + avgSpeed + " mph from\nthe " + avgWindDirection;
	}
	
	// Update is called once per frame
	void Update ()
	{
		updateWindData();
		
		updateText();

		// if time has changed, update data
		if (oldCurTime != curTime)
		{
			oldCurTime = curTime;
			
			mostRecent = getMostRecentData(curTime);
		}
	}
}
