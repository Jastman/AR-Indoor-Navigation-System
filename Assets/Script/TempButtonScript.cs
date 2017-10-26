using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class TempButtonScript : MonoBehaviour {

	public float zRotateDegree = 0;

	private GameObject arcamera, destinationPoint, testFloor;

	// Use this for initialization
	void Start () {
		arcamera = GameObject.Find("ARCamera");
		testFloor = GameObject.Find("TestMap");

		destinationPoint = GameObject.Find("Point");
		IEnumerable<TrackableBehaviour> trackableList = arcamera.GetComponent<CameraFocusController>().GetActiveTrackable();
		foreach (TrackableBehaviour tr in trackableList)
		{
			tr.gameObject.GetComponentInChildren<ArrowScript>().
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
			tr.gameObject.GetComponent<ArControlScript>().GetArrow().GetComponent<ArrowScript>().RotateArrow(new Vector3(0, 0, zRotateDegree));
			Debug.Log(tr.gameObject.name + " to Left");
		}
	}

	public void RotateRight()
	{
		zRotateDegree -= 10;

		IEnumerable<TrackableBehaviour> trackableList = arcamera.GetComponent<CameraFocusController>().GetActiveTrackable();
		foreach (TrackableBehaviour tr in trackableList)
		{
			tr.gameObject.GetComponent<ArControlScript>().GetArrow().GetComponent<ArrowScript>().RotateArrow(new Vector3(0, 0, zRotateDegree));
			Debug.Log(tr.gameObject.name + " to Right");
		}
	}

	public void PointToZero()
	{
		zRotateDegree = 0;
		IEnumerable<TrackableBehaviour> trackableList = arcamera.GetComponent<CameraFocusController>().GetActiveTrackable();
		foreach (TrackableBehaviour tr in trackableList)
		{
			tr.gameObject.GetComponent<ArControlScript>().GetArrow().GetComponent<ArrowScript>().PointToZero();
			Debug.Log(tr.gameObject.name + " to Zero");
		}
	}

	public void FindPath() //find path from current marker --get destination
	{
		IEnumerable<TrackableBehaviour> trackableList = arcamera.GetComponent<CameraFocusController>().GetActiveTrackable();
		foreach (TrackableBehaviour tr in trackableList)
		{
			MainController.instance.SetBeginPoint(tr.gameObject);
			if(MainController.instance.beginPoint != null && MainController.instance.destinationPoint != null) {
				MainController.instance.Navigate();
				Debug.Log("Navigate");
			}
			MainController.instance.ShowAR();
		}
		
	}
}
