using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class WindWidget : MonoBehaviour {

	//Name of input data file, no extension
	public string inputfile;
	
	// List for holding data from CSV reader
	private List<Dictionary<string, object>> pointList;
	
	//for text components to edit
	public Text windSpeedMsg;
	public Text avgWindVelocityMsg;

	//default messages
	public string defaultWindSpeedMsg;
	public string defaultAvgVelocityMsg;
	
	//to store list of possible windspeeds
	private Dictionary<string, float> cardinalDirections = new Dictionary<string, float>();
	
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
	}

	//get most recent wind speed recording from data and output it in the display
	private void getWindSpeed()
	{
		//get most recent wind speed, add it in to default wind speed message
		defaultWindSpeedMsg = pointList[pointList.Count-1]["wind direction"]+"\n"+pointList[pointList.Count-1]["wind speed"].ToString() + " mph";
		//plug wind speed message into display
		windSpeedMsg.text = defaultWindSpeedMsg;
	}
	
	//get average wind velocity
	private void getAvgWVelocity()
	{
		//length of data dictionary as an easily accessible variable
		int last = pointList.Count;
		
		//initialize variables to store stuff to be summed throughout the data
		float sumDirections = 0;
		float avgSpeed = 0;
		
		//iterate through data list to sum up the "wind direction" and "wind speed" values
		for (int i = 0; i < last; i ++)
		{
			sumDirections += cardinalDirections[pointList[i]["wind direction"].ToString()];
			avgSpeed += Convert.ToSingle(pointList[i]["wind speed"]);
		}

		//complete the averaging of the wind direction vectors
		float avgDir = sumDirections / last;
		//to round it to the nearest cardinal direction, we first have to enable it to round to the nearest int
		avgDir /= 22.5f;
		//round it to nearest integer, which is how the cardinal directions are stored
		avgDir = Mathf.Round(avgDir) * 22.5f;
		//convert the vector to the corresponding direction string 
		var avgCardinalDir = cardinalDirections.FirstOrDefault(x => x.Value == avgDir).Key;
		
		//complete the averaging of the avg speed
		avgSpeed = Mathf.Round((avgSpeed*10)/last)/10;

		//set the avg velocity message
		defaultAvgVelocityMsg = "avg. windspeed\nof " + avgSpeed + " mph from\nthe " + avgCardinalDir;
		//plug the avg velocity message into the displa
		avgWindVelocityMsg.text = defaultAvgVelocityMsg;
	}
	
	// Update is called once per frame
	void Update ()
	{
		getWindSpeed();
		
		getAvgWVelocity();
	}
}
