using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Utility;

public class IsItRaining : MonoBehaviour {

	//Name of input data file, no extension
	public string inputfile;
	
	// List for holding data from CSV reader
	private List<Dictionary<string, object>> pointList;
	
	// Indices for columns to be assigned
	public int columnX = 0;
	public int columnY = 1;
	public int columnZ = 2;
	
	// Declare full column names
	public string xName;
	public string yName;
	public string zName;
	
	// Use this for initialization
	void Start ()
	{
		//Set pointlist to results of function Reader with argument inputfile
		pointList = CSVReader.Read(inputfile);
		
		// Declare list of strings, fill with keys (column names)
		List<string> columnList = new List<string>(pointList[1].Keys);
 
		// Print number of keys (using .count)
		Debug.Log("There are " + columnList.Count + " columns in the CSV");
 
		foreach (string key in columnList)
			Debug.Log("Column name is " + key);
 
		// Assign column name from columnList to Name variables
		xName = columnList[columnX];
		yName = columnList[columnY];
		zName = columnList[columnZ];
	}
	
	// Update is called once per frame
	void Update ()
	{
		int last = pointList.Count;
		
	}
}
