using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindWidget : MonoBehaviour {

	//Name of input data file, no extension
	public string inputfile;
	
	// List for holding data from CSV reader
	private List<Dictionary<string, object>> pointList;
	
	//for text components to edit
	public Text windSpeedMsg;
	
	//global variables
	//to store list of possible windspeeds
	private Dictionary<string, Vector2> cardinalDirections = new Dictionary<string, Vector2>();
	
	// Use this for initialization
	void Start () {
		//Set pointlist to results of function Reader with argument inputfile
		pointList = CSVReader.Read(inputfile);
		
		//set list of possible windspeeds
		cardinalDirections.Add("N",new Vector2(0,1));
		cardinalDirections.Add("NNE",new Vector2(0.5f,1));
		cardinalDirections.Add("NE",new Vector2(1,1));
		cardinalDirections.Add("ENE",new Vector2(1,0.5f));
		cardinalDirections.Add("E",new Vector2(1,0));
		cardinalDirections.Add("ESE",new Vector2(1,-0.5f));
		cardinalDirections.Add("SE",new Vector2(1,-1));
		cardinalDirections.Add("SSE",new Vector2(0.5f,-1));
		cardinalDirections.Add("S",new Vector2(0,-1));
		cardinalDirections.Add("SSW",new Vector2(-0.5f,-1));
		cardinalDirections.Add("SW", new Vector2(-1, -1));
		cardinalDirections.Add("WSW",new Vector2(-1,-0.5f));
		cardinalDirections.Add("W",new Vector2(-1,0.5f));
		cardinalDirections.Add("NW",new Vector2(-1,1));
		cardinalDirections.Add("NWN",new Vector2(-0.5f,1));
	}

	//get most recent wind speed recording from data and output it in the display
	private void setWindWspeed()
	{
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
