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
	public Text avgVelocityMsg;

	//default messages
	public string defaultWindSpeedMsg;
	public string defaultAvgVelocityMsg;
	
	//global variables
	//to store list of possible windspeeds
	private Dictionary<string, Vector2> cardinalDirections = new Dictionary<string, Vector2>();
	
	// Use this for initialization
	void Start () {
		//Set pointlist to results of function Reader with argument inputfile
		pointList = CSVReader.Read(inputfile);
		
		//set list of possible windspeeds
		cardinalDirections.Add("N",new Vector2(0,2));
		cardinalDirections.Add("NNE",new Vector2(1,2));
		cardinalDirections.Add("NE",new Vector2(2,2));
		cardinalDirections.Add("ENE",new Vector2(2,1));
		cardinalDirections.Add("E",new Vector2(2,0));
		cardinalDirections.Add("ESE",new Vector2(2,-1));
		cardinalDirections.Add("SE",new Vector2(2,-2));
		cardinalDirections.Add("SSE",new Vector2(1,-2));
		cardinalDirections.Add("S",new Vector2(0,-2));
		cardinalDirections.Add("SSW",new Vector2(-1,-2));
		cardinalDirections.Add("SW", new Vector2(-2,-2));
		cardinalDirections.Add("WSW",new Vector2(-2,-1));
		cardinalDirections.Add("W",new Vector2(-2,1));
		cardinalDirections.Add("NW",new Vector2(-2,2));
		cardinalDirections.Add("NWN",new Vector2(-1,2));

		//set default messages in case something below goes wrong
		windSpeedMsg.text = defaultWindSpeedMsg;
		avgVelocityMsg.text = defaultAvgVelocityMsg;
	}

	//get most recent wind speed recording from data and output it in the display
	private void getWindSpeed()
	{
		//get most recent wind speed, add it in to default wind speed message
		defaultWindSpeedMsg = pointList[pointList.Count]["wind speed"].ToString() + " mpph";
		//plug wind speed message into display
		windSpeedMsg.text = defaultWindSpeedMsg;
	}
	
	//get average wind velocity
	private void getAvgWVelocity()
	{
		//length of data dictionary as an easily accessible variable
		int last = pointList.Count;
		
		//initialize variables to store stuff to be summed throughout the data
		Vector2 sumDirections = new Vector2(0,0);
		float avgSpeed = 0;
		
		//iterate through data list to sum up the "wind direction" and "wind speed" values
		for (int i = 0; i < last; i ++)
		{
			sumDirections += cardinalDirections[pointList[i]["wind direction"].ToString()];
			avgSpeed += Convert.ToSingle(pointList[i]["wind speed"]);
		}

		//complete the averaging of the wind direction vectors
		Vector2 avgDir = sumDirections / last;
		//round it to nearest integer, which is how the cardinal directions are stored
		avgDir = new Vector2(Mathf.Round(avgDir.x), Mathf.Round(avgDir.y));
		//convert the vector to the corresponding direction string 
		var avgCardinalDir = cardinalDirections.FirstOrDefault(x => x.Value == avgDir).Key;

		//set the avg velocity message
		defaultAvgVelocityMsg = "avg. windspeed\nof " + avgSpeed + " mph from\nthe " + avgCardinalDir;
		//plug the avg velocity message into the displa
		avgVelocityMsg.text = defaultAvgVelocityMsg;
	}
	
	// Update is called once per frame
	void Update ()
	{
		getWindSpeed();
		
		getAvgWVelocity();
	}
}
