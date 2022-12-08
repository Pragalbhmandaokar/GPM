using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class geolocation : MonoBehaviour
{
    public static geolocation instance;
    [SerializeField]
    public Text GpsStatus;
    public Text latitudeValue;
    public Text longitudeValue; 
    public Text altitudeValue;
    public Text horizontalAccuracyValue;
    public Text timeStampValue;
    public Text Timer;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        GpsStatus.text = "Started";
       
        Input.location.Start();
        latitudeValue.text = Input.location.lastData.latitude.ToString();
        StartCoroutine(GPSloc());
        
    }

    IEnumerator GPSloc()
    { 
        Timer.text = "Something";
        if(!Input.location.isEnabledByUser)
            yield break;


        int maxWait = 20;
     
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
           // Timer.text = maxWait.ToString();
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        if(maxWait < 1)
        {
            GpsStatus.text = "Time out";
            yield break;

        }

        if(Input.location.status == LocationServiceStatus.Failed)
        {
            GpsStatus.text = "Unable to connect to device";
            yield break;

        }

            //GpsStatus.text = "Running";
            InvokeRepeating("UpdateGpsData", 0f, 0.03f);
            //access granted
    }

    private Vector3 dir;
    string prev;

    private void play(string z, string x)
    {
        double d = double.Parse(prev);

        if (d < double.Parse(z))
        {
            float amountToMove = Time.deltaTime;
            transform.Translate(dir * amountToMove);
            dir = Vector3.forward;
        }
    }
    private void UpdateGpsData()
    {
        if(Input.location.status == LocationServiceStatus.Running)
        {
            GpsStatus.text = "Running";
            latitudeValue.text = Input.location.lastData.latitude.ToString();
            longitudeValue.text = Input.location.lastData.longitude.ToString();
            altitudeValue.text = Input.location.lastData.altitude.ToString();
            horizontalAccuracyValue.text = Input.location.lastData.horizontalAccuracy.ToString();
            timeStampValue.text = Input.location.lastData.timestamp.ToString();
            play(Input.location.lastData.latitude.ToString(), Input.location.lastData.longitude.ToString());
        }
        else
        {
            GpsStatus.text = "Stop";
        }
    }
}
