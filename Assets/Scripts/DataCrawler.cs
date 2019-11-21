using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataCrawler : MonoBehaviour
{
    public static Action<List<string>> provideData;
    
    public Transform sensorOrigin;
    public char separator;
    public int batchSize;

    private List<string> data;
    private Rigidbody rb;

    private void Awake()
    {
        CSVWriter.requestSeparator += () => separator;
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        data = new List<string>();
    }

    // Update is called once per frame
    void Update()
    {
        // Bit shift the index of the layer (8) to get a bit mask
        int layerMask = 1 << 8;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;
        
        RaycastHit leftSensor;
        RaycastHit frontSensor;
        RaycastHit rightSensor;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(sensorOrigin.position, transform.TransformDirection(Vector3.left), out leftSensor, Mathf.Infinity, layerMask))
        {
            Debug.DrawRay(sensorOrigin.position, transform.TransformDirection(Vector3.left) * leftSensor.distance, Color.yellow);
        }
        
        if (Physics.Raycast(sensorOrigin.position, transform.TransformDirection(Vector3.forward), out frontSensor, Mathf.Infinity, layerMask))
        {
            Debug.DrawRay(sensorOrigin.position, transform.TransformDirection(Vector3.forward) * frontSensor.distance, Color.yellow); ;
        }
        
        if (Physics.Raycast(sensorOrigin.position, transform.TransformDirection(Vector3.right), out rightSensor, Mathf.Infinity, layerMask))
        {
            Debug.DrawRay(sensorOrigin.position, transform.TransformDirection(Vector3.right) * rightSensor.distance, Color.yellow);
        }

        string sensorOutput = leftSensor.distance.ToString() + separator + frontSensor.distance + separator +
                              rightSensor.distance + separator + transform.rotation.y;
        data.Add(sensorOutput);
        
        if (data.Count >= batchSize)
        {
            provideData?.Invoke(data);
            data.Clear();
        }
    }
}
