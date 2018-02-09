using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.UI;

public class TempWidget : MonoBehaviour {

	[Tooltip("Name of input data file, no extension")]
	public string inputfile;
	
	// List for holding data from CSV reader
	private List<Dictionary<string, object>> pointList;
	
	//time to match with times in data
	public string curTime;
	
	//for text components to edit
	[Tooltip("Link to text object for displaying current temp")]
	public Text curTemp;
	[Tooltip("Link to text object for displaying high temp")]
	public Text highTemp;
	[Tooltip("Link")]
	public Text lowTemp;
	
	//default messages
	[Tooltip("Default message for current temp")]
	public string defaultCurTemp;
	[Tooltip("Default message for high temp")]
	public string defaultHighTemp;
	[Tooltip("Default message for low temp")]
	public string defaultLowTemp;
	
	//store most recent data
	private int mostRecent;
	//store high temp
	[Tooltip("High temp")]
	public float max;
	//store low temp
	[Tooltip("Low temp")]
	public float min;
	//store current temp
	[Tooltip("Current temp")]
	public float currentTemp;
	
	//store old curTime, to check if the mostRecent data needs to be updated
	private string oldCurTime;

	private void updateTempConstants(int i)
	{
		float ttc = Convert.ToSingle(pointList[i]["temperature"].ToString());
			
		//is current max temp still the max temp?
		if (max < ttc)
		{
			max = ttc;
		}
		
		if (min > ttc)
		//is current min temp still the min temp?
		{
			min = ttc;
		}
	
		currentTemp = Convert.ToSingle(pointList[i]["temperature"].ToString());
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
		
		//set default messages in case something below goes wrong
		curTemp.text = defaultCurTemp;
		highTemp.text = defaultHighTemp;
		lowTemp.text = defaultLowTemp;

		mostRecent = 0;
		
		//get up to date
		max = Convert.ToSingle(pointList[0]["temperature"]);
		min = Convert.ToSingle(pointList[0]["temperature"]);
		currentTemp = Convert.ToSingle(pointList[0]["temperature"]);

		//store current time
		oldCurTime = curTime;
		
		//get most recent data
		mostRecent = getMostRecentData(curTime);
		
		for(int i = 0; i < mostRecent; i ++)
		{
			updateTempConstants(i);
		}
	}

	private void updateText()
	{
		curTemp.text = currentTemp + "°F";

		highTemp.text = "high: " + max + "°F";

		lowTemp.text = "low: " + min + "°F";
	}
	
	// Update is called once per frame
	void Update ()
	{
		updateTempConstants(mostRecent);
		
		updateText();

		// if time has changed, update data
		if (oldCurTime != curTime)
		{
			oldCurTime = curTime;
			
			mostRecent = getMostRecentData(curTime);
		}
	}
}
