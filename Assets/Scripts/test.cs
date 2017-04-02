
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class test : MonoBehaviour
{

    public Text countText;

    // Use this for initialization
    void Start()
    {
        countText = GetComponent<Text>();
    }
    // Update is called once per frame
    void Update()
    {
        stepDetector();
        countText.text = steps.ToString();

    }

    private float loLim = 0.005F;
    private float hiLim = 0.08F;
    private int steps = 0;
    private bool stateH = false;
    private float fHigh = 10.0F;
    private float curAcc = 0F;
    private float fLow = 0.1F;
    private float avgAcc;

    public int stepDetector()
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

        return steps;
    }
}


