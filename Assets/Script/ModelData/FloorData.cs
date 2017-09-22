using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorData : MonoBehaviour {

	public List<MarkerData> markerList;
	public string floor = "0";

	// Use this for initialization
	void Start () {
		/* Work after All Marker Data get their position */
		floor = gameObject.name.Replace("Floor","");
		foreach (MarkerData markerdata in GetComponentsInChildren<MarkerData>())
		{
			Debug.Log("Get Marker " + markerdata.markerName + ": " +markerdata.position);
			markerdata.floor = floor;
			markerList.Add(markerdata);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
