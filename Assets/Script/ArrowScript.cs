using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Debug.Log("start");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/* get world coordinate that want to rotate. rotate to input */
	public void RotateArrow(Vector3 eulerRotation)
	{
		transform.rotation = Quaternion.identity;
		transform.Rotate(eulerRotation, Space.World);
		Debug.Log(transform.rotation.eulerAngles + " " + (eulerRotation.y-transform.rotation.y) + " " + eulerRotation.y);
	}
}
