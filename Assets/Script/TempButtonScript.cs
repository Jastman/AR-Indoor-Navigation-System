using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class TempButtonScript : MonoBehaviour {

	public float yRotateDegree = 0;

	private GameObject arcamera;

	// Use this for initialization
	void Start () {
		arcamera = GameObject.Find("ARCamera");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void RotateLeft()
	{
		yRotateDegree += 10;
		
		IEnumerable<TrackableBehaviour> trackableList = arcamera.GetComponent<CameraFocusController>().GetActiveTrackable();
		foreach (TrackableBehaviour tr in trackableList)
		{
			tr.gameObject.transform.GetChild(0).gameObject.GetComponent<ArrowScript>().RotateArrow(new Vector3(0, yRotateDegree, 0));
			Debug.Log(tr.gameObject.name + " to Left");
		}
	}

	public void RotateRight()
	{
		yRotateDegree -= 10;

		IEnumerable<TrackableBehaviour> trackableList = arcamera.GetComponent<CameraFocusController>().GetActiveTrackable();
		foreach (TrackableBehaviour tr in trackableList)
		{
			tr.gameObject.transform.GetChild(0).gameObject.GetComponent<ArrowScript>().RotateArrow(new Vector3(0, yRotateDegree, 0));
			Debug.Log(tr.gameObject.name + " to Right");
		}
	}
}
