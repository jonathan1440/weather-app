using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
 
public class DataPlotter : MonoBehaviour
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
 
    // Use this for initialization
    void Start()
    {
 
        // Set pointlist to results of function Reader with argument inputfile
        pointList = CSVReader.Read(inputfile);
 
        //Log to console
        Debug.Log(pointList);
 
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
 
        //Loop through Pointlist
        for (var i = 0; i < pointList.Count; i++)
        {
            // Get value in poinList at ith "row", in "column" Name
            float x = System.Convert.ToSingle(pointList[i][xName]);
            float y = System.Convert.ToSingle(pointList[i][yName]);
            float z = System.Convert.ToSingle(pointList[i][zName]);
 
            //instantiate the prefab with coordinates defined above
            Instantiate(PointPrefab, new Vector3(x, y, z), Quaternion.identity);
 
            // Instantiate as gameobject variable so that it can be manipulated within loop
            GameObject dataPoint = Instantiate(
                    PointPrefab,
                    new Vector3(x, y, z),
                    Quaternion.identity);
 
            // Make child of PointHolder object, to keep points within container in hierarchy
            dataPoint.transform.parent = PointHolder.transform;
 
            // Assigns original values to dataPointName
            string dataPointName = pointList[i][xName] + " " + pointList[i][yName] + " " + pointList[i][zName];
 
            // Assigns name to the prefab
            dataPoint.transform.name = dataPointName;         
        }
    }
}