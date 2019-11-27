using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DataCrawler : MonoBehaviour
{
    public static Action<List<string>> provideData;
    
    public Transform sensorOrigin;
    public char separator;
    public int batchSize;

    private List<string> data;
    private Rigidbody rb;

    //Ui variables
    [SerializeField]
    private TextMeshProUGUI directionUI;
    [SerializeField]
    private TextMeshProUGUI sensorLeftUI;
    [SerializeField]
    private TextMeshProUGUI sensorRightUI;
    [SerializeField]
    private TextMeshProUGUI sensorFrontUI;
    [SerializeField]
    private Slider generatingUI;

    private void Awake()
    {
        CSVWriter.requestSeparator += () => separator;
        rb = GetComponent<Rigidbody>();

    }

    private void Start()
    {
        data = new List<string>();
        generatingUI.value = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //UI slider controller, percent of writer csv file
        if (generatingUI.value < 1000)
        {
            generatingUI.value += 1;
        }
        else
        {
            generatingUI.value = 0;
        }

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

        /*float directionOutput = transform.localEulerAngles.y > 180
            ? transform.localEulerAngles.y - 360
            : transform.localEulerAngles.y;*/
        float directionOutput = rb.angularVelocity.y;

        //Ploting infos in UI
        if (directionOutput < -0.1)
        {
            directionUI.text = "<<<";
            directionOutput = -1;
        }else if (directionOutput > 0.1)
        {
            directionUI.text = ">>>";
            directionOutput = 1;
        }
        else
        {
            directionUI.text = "^";
            directionOutput = 0;
        }
        
        sensorFrontUI.text = frontSensor.distance.ToString();
        sensorLeftUI.text = leftSensor.distance.ToString();
        sensorRightUI.text = rightSensor.distance.ToString();

        string sensorOutput = (leftSensor.distance.ToString()+ separator + frontSensor.distance + separator +
                              rightSensor.distance + separator + directionOutput).Replace(',', '.');
        data.Add(sensorOutput);

        if (data.Count >= batchSize)
        {
            provideData?.Invoke(data);
            data.Clear();
        }
    }
}
