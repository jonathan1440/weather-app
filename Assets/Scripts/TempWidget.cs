using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
	
	// Use this for initialization
	void Start () {
		//Set pointlist to results of function Reader with argument inputfile
		pointList = CSVReader.Read(inputfile);
		
		//set default messages in case something below goes wrong
		curTemp.text = defaultCurTemp;
		highTemp.text = defaultHighTemp;
		lowTemp.text = defaultLowTemp;
	}

	private Dictionary<string, object> getDataForTime(string curTime)
	{
		string mostRecent;
		int i = 0;
		while (true)
		{
			string checking = pointList[i]["time"].ToString();
			string[] it = checking.Split(new string[] {":"}, StringSplitOptions.None);
			string[] t = curTime.Split(new string[] {":"}, StringSplitOptions.None);
			
			if (it == t)
			{
				return pointList[i];
			}

			if (int.Parse(it[0]) < int.Parse(t[2]) && int.Parse(it[1]) < int.Parse(t[1]) && int.Parse(it[2]) < int.Parse(t[2]))
			{
				i++;
			}
			
			if (int.Parse(it[0]) > int.Parse(t[2]) && int.Parse(it[1]) > int.Parse(t[1]) && int.Parse(it[2]) > int.Parse(t[2]))
			{
				return pointList[i-1];
			}
		}
	}

	private void getCurrentTemp()
	{
		var data = getDataForTime(curTime);

		curTemp.text = data["temperature"] + "°F";
	}

	private void getHighTemp()
	{
		int i = 0;
		int max = int.Parse(pointList[0]["temperature"].ToString());
		while (true)
		{
			//To continue or no?
			string checking = pointList[i]["time"].ToString();
			string[] it = checking.Split(new string[] {":"}, StringSplitOptions.None);
			string[] t = curTime.Split(new string[] {":"}, StringSplitOptions.None);
			
			if (int.Parse(it[0]) <= int.Parse(t[2]) && int.Parse(it[1]) <= int.Parse(t[1]) && int.Parse(it[2]) <= int.Parse(t[2]))
			{
				int ttc = int.Parse(pointList[i]["temperature"].ToString());
			
				//is current max temp still the max temp?
				if (max < ttc)
				{
					max = ttc;
				}
				
				i++;
			}
			
			if (int.Parse(it[0]) > int.Parse(t[2]) && int.Parse(it[1]) > int.Parse(t[1]) && int.Parse(it[2]) > int.Parse(t[2]))
			{
				break;
			}
		}
		
		highTemp.text = "High: " +max+ "°F";
	}
	
	private void getLowTemp()
	{
		int i = 0;
		int min = int.Parse(pointList[0]["temperature"].ToString());
		while (true)
		{
			//To continue or no?
			string checking = pointList[i]["time"].ToString();
			string[] it = checking.Split(new string[] {":"}, StringSplitOptions.None);
			string[] t = curTime.Split(new string[] {":"}, StringSplitOptions.None);
			
			if (int.Parse(it[0]) <= int.Parse(t[2]) && int.Parse(it[1]) <= int.Parse(t[1]) && int.Parse(it[2]) <= int.Parse(t[2]))
			{
				int ttc = int.Parse(pointList[i]["temperature"].ToString());
			
				//is current max temp still the max temp?
				if (min > ttc)
				{
					min = ttc;
				}
				
				i++;
			}
			
			if (int.Parse(it[0]) > int.Parse(t[2]) && int.Parse(it[1]) > int.Parse(t[1]) && int.Parse(it[2]) > int.Parse(t[2]))
			{
				break;
			}
		}
		
		highTemp.text = "Low: " +min+ "°F";
	}
	
	// Update is called once per frame
	void Update () {
		getCurrentTemp();
		
		getHighTemp();
		
		getLowTemp();
	}
}
