using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.UI;

public class geoController : MonoBehaviour
{
    geolocation geolocation;
    public double latitude = 0;
    public double longitude = 0;
    public Text latitudeValue;
    public Text Direction;
    [SerializeField] public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
       
      
        latitude = Input.location.lastData.latitude;
        longitude = Input.location.lastData.longitude; 
        player = GetComponent<GameObject>();
    }
    private void Update()
    {
        if (Input.location.status == LocationServiceStatus.Running)
        {
            Direction.text = Input.location.lastData.latitude.ToString();
            latitudeValue.text = GPSEncoder.USCToGPS(new Vector2(Input.location.lastData.latitude, Input.location.lastData.longitude)).ToString();

        }
    }
}

public class GPSEncoder {
    private static GPSEncoder _singleton;
    public static Vector3 GPSToUCS(float latitude, float longitude)
    {
        return GetInstance().ConvertGPStoUCS(new Vector2(latitude, longitude));
    }
    public static Vector2 USCToGPS(Vector3 position)
    {
        return GetInstance().ConvertUCStoGPS(position);
    }
    private static GPSEncoder GetInstance()
    {
        if (_singleton == null)
        {
            _singleton = new GPSEncoder();
        }
        return _singleton;
    }

    #region Instance Variables
    geolocation geolocation;
    private Vector2 _localOrigin = Vector2.zero;
    private float _LatOrigin { get { return _localOrigin.x; } }
    private float _LonOrigin { get { return _localOrigin.y; } }

    private float metersPerLat;
    private float metersPerLon;
    #endregion

    #region Instance Functions
    private void FindMetersPerLat(float lat) // Compute lengths of degrees
    {
        float m1 = 111132.92f;    // latitude calculation term 1
        float m2 = -559.82f;        // latitude calculation term 2
        float m3 = 1.175f;      // latitude calculation term 3
        float m4 = -0.0023f;        // latitude calculation term 4
        float p1 = 111412.84f;    // longitude calculation term 1
        float p2 = -93.5f;      // longitude calculation term 2
        float p3 = 0.118f;      // longitude calculation term 3

        lat = lat * Mathf.Deg2Rad;

        // Calculate the length of a degree of latitude and longitude in meters
        metersPerLat = m1 + (m2 * Mathf.Cos(2 * (float)lat)) + (m3 * Mathf.Cos(4 * (float)lat)) + (m4 * Mathf.Cos(6 * (float)lat));
        metersPerLon = (p1 * Mathf.Cos((float)lat)) + (p2 * Mathf.Cos(3 * (float)lat)) + (p3 * Mathf.Cos(5 * (float)lat));
    }

    public Vector3 ConvertGPStoUCS(Vector2 gps)
    {
        FindMetersPerLat(_LatOrigin);
        float zPosition = metersPerLat * (gps.x - _LatOrigin); //Calc current lat
        float xPosition = metersPerLon * (gps.y - _LonOrigin); //Calc current lat
        return new Vector3((float)xPosition, 0, (float)zPosition);
    }

    private Vector2 ConvertUCStoGPS(Vector3 position)
    {
        FindMetersPerLat(_LatOrigin);
        Vector2 geoLocation = new Vector2(0, 0);
        geoLocation.x = (_LatOrigin + (position.z) / metersPerLat); //Calc current lat
        geoLocation.y = (_LonOrigin + (position.x) / metersPerLon); //Calc current lon
        return geoLocation;
    }
    #endregion


}