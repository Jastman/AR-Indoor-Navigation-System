using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerData : MonoBehaviour {

	public Vector3 position = Vector3.zero;
	public Vector3 orientation = Vector3.zero;

	// Use this for initialization
	void Start () {
		position = transform.position;
		orientation = transform.rotation.eulerAngles;
		Debug.Log("Start " + gameObject.name + " " +position);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
