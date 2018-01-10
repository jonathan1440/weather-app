using System;
using System.Collections.Generic;
using UnityEngine;
 
public class ScatterPlotter : MonoBehaviour
{
    // Name of the input file, no extension
    public string inputfile;
 
    // List for holding data from CSV reader
    private List<Dictionary<string, object>> pointList;
 
    // Indices for columns to be assigned
    public int columnX = 0;
    public int columnY = 1;
    public int columnZ = 2;
 
    // Full column names
    public string xName;
    public string yName;
    public string zName;
     
    // The prefab for the data points that will be instantiated
    public GameObject PointPrefab;
 
    // Object which will contain instantiated prefabs in hiearchy
    public GameObject PointHolder;
    
    // Scale of graph, aka how far in space the maximum points go
    public float PlotScale = 10;
    
    // Transparency of graph box
    public float BoxAlpha = 0.9f;
 
    // Use this for initialization
    void Start()
    {
 
        // Set pointlist to results of function Reader with argument inputfile
        pointList = CSVReader.Read(inputfile);
 
        //Log to console
        Debug.Log("pointList: "+pointList);
 
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
        
        // Get maximums of each axis
        float xMax = FindMaxValue(xName);
        float yMax = FindMaxValue(yName);
        float zMax = FindMaxValue(zName);
 
        // Get minimums of each axis
        float xMin = FindMinValue(xName);
        float yMin = FindMinValue(yName);
        float zMin = FindMinValue(zName);
        
        //Get change in each axis
        float[] axisDeltas = new float[3];
        axisDeltas[0] = xMax - xMin;
        axisDeltas[1] = yMax - yMin;
        axisDeltas[2] = zMax - zMin;
        
        //Get biggest change
        float maxDelta = axisDeltas[0];
        foreach (float delta in axisDeltas)
        {
            if (Convert.ToSingle(delta) > maxDelta)
            {
                maxDelta = delta;
            }
        }
 
        //Loop through Pointlist
        foreach (var t in pointList)
        {
            // Get value in poinList at ith "row", in "column" Name, normalize
            float x = (Convert.ToSingle(t[xName]) - xMin) / (xMax - xMin);
            float y = (Convert.ToSingle(t[yName]) - yMin) / (yMax - yMin);
            float z = (Convert.ToSingle(t[zName]) - zMin) / (zMax - zMin);
 
            // Instantiate as gameobject variable so that it can be manipulated within loop
            GameObject dataPoint = Instantiate(
                PointPrefab,
                new Vector3(x, y, z) * PlotScale,
                Quaternion.identity);
 
            // Make child of PointHolder object, to keep points within container in hierarchy
            dataPoint.transform.parent = PointHolder.transform;
 
            // Assigns original values to dataPointName
            string dataPointName = t[xName] + " " + t[yName] + " " + t[zName];
 
            // Assigns name to the prefab
            dataPoint.transform.name = dataPointName; 
            
            // Gets material color and sets it to a new RGBA color we define
            dataPoint.GetComponent<Renderer>().material.color = new Color(x,y,z, 1.0f);
        }
        
        //create box to bound graph
        GameObject box = GameObject.CreatePrimitive(PrimitiveType.Cube);
        
        //attach box to Graph GameObject
        box.transform.parent = transform;
        
        //Scale the box to completely surround the dataBalls as a cube
        box.transform.localScale = new Vector3(maxDelta,maxDelta,maxDelta);
    }
    
    //Find the max value of a given column of the pointList dictionary
    private float FindMaxValue(string columnName)
    {
        //set initial value to first value in column
        float maxValue = Convert.ToSingle(pointList[0][columnName]);
 
        //Loop through Dictionary, overwrite existing maxValue if new value is larger
        foreach (var t in pointList)
        {
            if (maxValue < Convert.ToSingle(t[columnName]))
                maxValue = Convert.ToSingle(t[columnName]);
        }
 
        //Spit out the max value
        return maxValue;
    }
    
    //Find the min value of a given column of the pointList dictionary
    private float FindMinValue(string columnName)
    {
        //set initial value to first value in column
        float minValue = Convert.ToSingle(pointList[0][columnName]);
 
        //Loop through Dictionary, overwrite existing minValue if new value is smaller
        foreach (var t in pointList)
        {
            if (Convert.ToSingle(t[columnName]) < minValue)
                minValue = Convert.ToSingle(t[columnName]);
        }
 
        return minValue;
    }
}