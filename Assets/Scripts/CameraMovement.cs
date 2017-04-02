using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private float magnitudeThreshold = 0.1F;
    private float move = 10;
    private static int maxSamples = 5;

    private float loLim = 0.006F;
    private float hiLim = 0.06F;
    private int steps = 0;
    private bool stateH = false;
    private float fHigh = 10.0F;
    private float curAcc = 0F;
    private float fLow = 0.1F;
    private float avgAcc;
    private int lastStep = 0;

    // Variables for vector averaging
    private float avgMagnitude = 0;
    private float[] pastMagnitudes = new float[maxSamples];
    private float avgDirection = 0;
    private float[] pastDirections = new float[maxSamples];
    private float avgX = 0;
    private float[] pastXVal = new float[maxSamples];
    private float avgZ = 0;
    private float[] pastZVal = new float[maxSamples];
    private int head = 0;
    private float PI = Mathf.PI;


    // Use this for initialization
    void Start()
    {
        for(int i = 0; i<maxSamples; i++) {
            pastMagnitudes[i] = 0;
            pastDirections[i] = 0;
            pastXVal[i] = 0;
            pastZVal[i] = 0;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (findVector() && stepDetector()) {
            float angle = transform.GetChild(0).transform.rotation.eulerAngles.y;
            Vector3 temp = new Vector3(transform.position.x + move*Mathf.Cos(angle), -3, transform.position.z + move*Mathf.Sin(angle));
            transform.position = temp;
        }
        //transform.rotation = transform.GetChild(0).transform.rotation;
    }


    public bool stepDetector()
    {
        curAcc = Mathf.Lerp(curAcc, Input.acceleration.magnitude, Time.deltaTime * fHigh);
        avgAcc = Mathf.Lerp(avgAcc, Input.acceleration.magnitude, Time.deltaTime * fLow);
        float delta = curAcc - avgAcc;
        if (!stateH)
        {
            if (delta > hiLim)
            {
                stateH = true;
                steps++;
            }
        }
        else
        {
            if (delta < loLim)
            {
                stateH = false;
            }
        }

        avgAcc = curAcc;
        //calDistance(steps);

        if (lastStep == steps)
        {
            return false;
        }
        else
        {
            lastStep = steps;
            return true;
        }
    }

    public bool findVector()
    {
        // Noise filter
        float ax = 0;
        float az = 0;
        if( System.Math.Abs(Input.acceleration.x) > 0.05 )
            ax = Input.acceleration.x;
        if( System.Math.Abs(Input.acceleration.z) > 0.05 )
            az = Input.acceleration.z;

        float magnitude = Mathf.Sqrt(ax * ax + az * az);
        
        avgMagnitude = avgMagnitude + ((magnitude - pastMagnitudes[head]) / maxSamples);
        pastMagnitudes[head] = magnitude;

        /*
        float direction = 0;

        if (ax == 0)
        {
            if (az > 0)
            {
                direction = PI / 2;
            }
            else
            {
                direction = 3 * PI / 2;
            }
        }
        else if (az == 0)
        {
            if (ax > 0)
            {
                direction = 0;
            }
            else
            {
                direction = PI;
            }
        }
        else
        {
            direction = Mathf.Atan( System.Math.Abs(az) / System.Math.Abs(ax));

            if (az > 0 && ax < 0)
            {
                direction = PI - direction;
            }
            else if (az < 0 && ax < 0)
            {
                direction += PI;
            }
            else if (az < 0 && ax > 0)
            {
                direction = 2 * PI - direction;
            }
        }
        avgDirection = avgDirection + (direction - pastDirections[prevVal]) / maxSamples;
        pastDirections[head] = direction;
        */
        
        avgX = avgX + (ax - pastXVal[head]) / maxSamples;
        pastXVal[head] = ax;
        avgZ = avgZ + (az - pastZVal[head]) / maxSamples;
        pastZVal[head] = az;
      
        head++;
        if (head == maxSamples)
        {
            head = 0;
        }

        return (avgMagnitude > magnitudeThreshold);
    }
}
