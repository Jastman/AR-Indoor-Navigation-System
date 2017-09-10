using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageTargetData : MonoBehaviour {

	public Vector3 position;
	public Vector3 orientation;

	// Use this for initialization
	void Start () {
		position = transform.position;
		orientation = transform.rotation.eulerAngles;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
