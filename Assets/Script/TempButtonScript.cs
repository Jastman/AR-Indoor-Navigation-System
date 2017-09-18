using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class TempButtonScript : MonoBehaviour {

	public float zRotateDegree = 0;

	private GameObject arcamera, destinationPoint;

	// Use this for initialization
	void Start () {
		arcamera = GameObject.Find("ARCamera");

		destinationPoint = GameObject.Find("Point");
		IEnumerable<TrackableBehaviour> trackableList = arcamera.GetComponent<CameraFocusController>().GetActiveTrackable();
		foreach (TrackableBehaviour tr in trackableList)
		{
			tr.gameObject.transform.GetChild(0).gameObject.GetComponent<ArrowScript>().
				PointToCoordinate(destinationPoint.GetComponent<MarkerData>().position);
			Debug.Log(tr.gameObject.name + " Turn Default Position");
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void RotateLeft()
	{
		zRotateDegree += 10;
		
		IEnumerable<TrackableBehaviour> trackableList = arcamera.GetComponent<CameraFocusController>().GetActiveTrackable();
		foreach (TrackableBehaviour tr in trackableList)
		{
			tr.gameObject.transform.GetChild(0).gameObject.GetComponent<ArrowScript>().RotateArrow(new Vector3(0, 0, zRotateDegree));
			Debug.Log(tr.gameObject.name + " to Left");
		}
	}

	public void RotateRight()
	{
		zRotateDegree -= 10;

		IEnumerable<TrackableBehaviour> trackableList = arcamera.GetComponent<CameraFocusController>().GetActiveTrackable();
		foreach (TrackableBehaviour tr in trackableList)
		{
			tr.gameObject.transform.GetChild(0).gameObject.GetComponent<ArrowScript>().RotateArrow(new Vector3(0, 0, zRotateDegree));
			Debug.Log(tr.gameObject.name + " to Right");
		}
	}

	public void PointToZero()
	{
		zRotateDegree = 0;
		IEnumerable<TrackableBehaviour> trackableList = arcamera.GetComponent<CameraFocusController>().GetActiveTrackable();
		foreach (TrackableBehaviour tr in trackableList)
		{
			tr.gameObject.transform.GetChild(0).gameObject.GetComponent<ArrowScript>().PointToZero();
			Debug.Log(tr.gameObject.name + " to Zero");
		}
	}
}
