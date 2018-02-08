using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.UI;

public class TempWidget : MonoBehaviour {

	//Name of input data file, no extension
	public string inputfile;
	
	// List for holding data from CSV reader
	private List<Dictionary<string, object>> pointList;
	
	//time to match with times in data
	public string curTime;
	
	//for text components to edit
	public Text curTemp;
	public Text highTemp;
	public Text lowTemp;
	
	//default messages
	public string defaultCurTemp;
	public string defaultHighTemp;
	public string defaultLowTemp;
	
	//store most recent data
	public int mostRecent;
	//store high temp
	public float max;
	//store low temp
	public float min;
	//store current temp
	public float currentTemp;
	
	//store old curTime, to check if the mostRecent data needs to be updated
	private string oldCurTime;

	/*void Start()
	{
		//Set pointlist to results of function Reader with argument inputfile
		pointList = CSVReader.Read(inputfile);

		//set default messages in case something below goes wrong
		curTemp.text = defaultCurTemp;
		highTemp.text = defaultHighTemp;
		lowTemp.text = defaultLowTemp;

		max = 0;
		min = Convert.ToSingle(pointList[0]["temperature"]);
		foreach (Dictionary<string,object> i in pointList)
		{
			float temp = Convert.ToSingle(i["temperature"]);

			if (temp > max)
			{
				max = temp;
			}

			if (temp < min)
			{
				min = temp;
			}
		}

		currentTemp = Convert.ToSingle(pointList[pointList.Count - 1]["temperature"]);
	}

	void Update()
	{
		curTemp.text = Mathf.Round(currentTemp*10)/10  + "°F";

		highTemp.text = "High: " + Mathf.Round(max*10)/10 + "°F";

		lowTemp.text = "Low: " + Mathf.Round(min*10)/10 + "°F";
	}*/

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
		
		for(int i = 0; i < pointList.Count;){
			string checking = pointList[i]["time"].ToString();
			string[] it = checking.Split(new string[] {":"}, StringSplitOptions.None);
			string[] t = curTime.Split(new string[] {":"}, StringSplitOptions.None);
			Debug.Log(i);
			Debug.Log(checking);
			Debug.Log(it[0]+","+it[1]+","+it[2]);
			Debug.Log(curTime);
			Debug.Log(t[0] + "," + t[1] + "," + t[2]);
			
			if (it[0] == t[0] && it[1] == t[1] && it[2] == t[2])
			{
				Debug.Log("time equalled");
				mostRecent = i;
				break;
			}

			if (int.Parse(it[0]) < int.Parse(t[2]) && double.Parse(it[1]+"."+it[2]) < double.Parse(t[1]+"."+t[2]))
			{
				i++;
			}
			
			if (int.Parse(it[0]) > int.Parse(t[2]) && int.Parse(it[1]) > int.Parse(t[1]) && int.Parse(it[2]) > int.Parse(t[2]))
			{
				Debug.Log("time exceeded");
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

		//Debug.Log(pointList[0]["temperature"].GetType());
		
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
			Debug.Log("170: "+max+","+min+","+currentTemp+","+i);
			updateTempConstants(i);
		}
	}

	private void updateText()
	{
		curTemp.text = currentTemp + "°F";

		highTemp.text = "High: " + max + "°F";

		lowTemp.text = "Low: " + min + "°F";
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
