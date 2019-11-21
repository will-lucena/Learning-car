using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class CSVWriter : MonoBehaviour
{
    private List<string[]> rowData = new List<string[]>();
   

    // Use this for initialization
    void Start () {
        Save();
    }
    
    void Save(){

        // Creating First row of titles manually..
        string[] rowDataTemp = new string[3];
        rowDataTemp[0] = "sensor-left";
        rowDataTemp[1] = "sensor-front";
        rowDataTemp[2] = "sensor-right";
        rowDataTemp[2] = "speed";
        rowData.Add(rowDataTemp);

        // You can add up the values in as many cells as you want.
        for(int i = 0; i < 10; i++){
            rowDataTemp = new string[3];
            rowDataTemp[0] = "Sushanta"+i; // name
            rowDataTemp[1] = ""+i; // ID
            rowDataTemp[2] = "$"+UnityEngine.Random.Range(5000,10000); // Income
            rowData.Add(rowDataTemp);
        }

        string[][] output = new string[rowData.Count][];

        for(int i = 0; i < output.Length; i++){
            output[i] = rowData[i];
        }

        int     length         = output.GetLength(0);
        string     delimiter     = ",";

        StringBuilder sb = new StringBuilder();
        
        for (int index = 0; index < length; index++)
            sb.AppendLine(string.Join(delimiter, output[index]));
        
        
        string filePath = getPath();

        StreamWriter outStream = System.IO.File.CreateText(filePath);
        outStream.WriteLine(sb);
        outStream.Close();
    }

    // Following method is used to retrive the relative path as device platform
    private string getPath(){
#if UNITY_EDITOR
        return Application.dataPath +"/CSV/"+"Saved_data.csv";
#elif UNITY_ANDROID
        return Application.persistentDataPath+"Saved_data.csv";
#elif UNITY_IPHONE
        return Application.persistentDataPath+"/"+"Saved_data.csv";
#else
        return Application.dataPath +"/"+"Saved_data.csv";
#endif
    }
}
