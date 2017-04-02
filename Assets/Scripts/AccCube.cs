using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class AccCube : MonoBehaviour {

public static List<SampleReading> accelerations = new List<SampleReading>();
static double accumulate;
public long ticks;
public int count;
public float displacementX;
public float displacementY;
public float displacementZ;
int tabVal;
public string fileName = "Assets/graph.csv";


// Use this for initialization
void Start () {
//File.Delete(fileName);
//File.Create(fileName);
tabVal = 0;
count = 0;
displacementX = 0;
displacementY = 0;
displacementZ = 0;
accelerations.Clear();
accumulate = 0;


//plugin.Call("setSamplingPeriod", 1000); // refreshes sensor 1 milli second each
//InvokeRepeating("CheckSensor", 1.0f, 0.001f);
}



// Update is called once per frame
void Update () {
        /*
    SampleReading cur;
    float ax = Input.acceleration.x;
    float ay = Input.acceleration.y + 1;
    float az = Input.acceleration.z;
    Debug.Log( " x: " + ax.ToString() + " y: " + ay.ToString() + " z: " + az.ToString() );
    //writeTo();
    if ( System.Math.Sqrt( (ax * ax) + (ay * ay) + (az * az) ) > 0.1 )
    {
        cur = new SampleReading(ax, ay, az, ticks);
        accelerations.Add(cur);
        count++;
        ticks++;
    }
    //if (count == 10)
    //{
        //   count = 0;
        getDistance(accelerations);
        //  accelerations.Clear();
        transform.Translate(displacementX, displacementY, displacementZ);*/
        stepDetector();
        Debug.Log(steps);

}
public void writeTo()
{
        
float ax = Input.acceleration.x;
float ay = Input.acceleration.y;
float az = Input.acceleration.z;
string data = "";
data += tabVal.ToString() + ",";
data += ax + ",";
data += ay + ",";
data += az + ",";
tabVal++;

StreamWriter writer = new StreamWriter(fileName, true);
writer.WriteLine(data);
writer.Close();

//AssetDatabase.ImportAsset(fileName);
//TextAsset asset = Resources.Load("test");

}

    private float loLim = 0.005F;
    private float hiLim = 0.3F;
    private int steps = 0;
    private bool stateH = false;
    private float fHigh = 8.0F;
    private float curAcc = 0F;
    private float fLow = 0.2F;
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

    public class SampleReading
{
public float X;
public float Y;
public float Z;
public long TimeTick;
public SampleReading(float m_x, float m_y, float m_z, long time)
{
    X = m_x;
    Y = m_y;
    Z = m_z;
    TimeTick = time;
}

}


private void getDistance(IList<SampleReading> samples)
{
float dx = 0;
float vx = 0;
float dy = 0;
float vy = 0;
float dz = 0;
float vz = 0;


for (var i = 1; i < samples.Count; i++)
{
    var curr = samples[i];
    var prev = samples[i - 1];

    var dt = curr.TimeTick - prev.TimeTick;
    if (dt == 0) continue;

    vx += (prev.X + curr.X) / 2.0f * dt;
    //if(System.Math.Abs(vx) > 0.2)
        dx += vx * dt;

    vy += (prev.Y + curr.Y) / 2.0f * dt;
    //if(System.Math.Abs(vy) > 0.2)
        dy += vy * dt;

    vz += (prev.Z + curr.Z) / 2.0f * dt;
    //if ( System.Math.Abs(vz) > 0.2)
        dz += vz * dt;

            
}
displacementX = dx;
displacementY = dy;
displacementZ = dz;
}
}
