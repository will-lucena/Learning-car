using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class CSVWriter : MonoBehaviour
{
    public static System.Func<char> requestSeparator;

    private char separator;
    public int filesGenerated = 0;
    // Use this for initialization
    void Start ()
    {
        DataCrawler.provideData += save;
        separator = (char) requestSeparator?.Invoke();
    }
    
    void save(List<string> data)
    {
        string header = "sensor-left" + separator + "sensor-front" + separator + "sensor-right" + separator + "direction";
        data.Insert(0, header);
        string filePath = getPath();

        StringBuilder sb = new StringBuilder();
        sb.AppendLine(header);
        foreach (var line in data)
        {
            sb.AppendLine(line);
        }
        
        StreamWriter outStream = System.IO.File.CreateText(filePath);
        outStream.WriteLine(sb.ToString());
        outStream.Close();
        Debug.Log(filePath);
        filesGenerated++;
    }

    // Following method is used to retrive the relative path as device platform
    private string getPath(){
        return Application.dataPath +"/csv/"+"dataset" + filesGenerated + ".csv";
    }
}
