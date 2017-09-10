using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour {

	ImageTargetData targetData;

	// Use this for initialization
	void Start () {
		Debug.Log("start");
		targetData = transform.parent.gameObject.GetComponent<ImageTargetData>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/* get world coordinate that want to rotate. rotate to input */
	public void RotateArrow(Vector3 eulerRotation)
	{
		transform.rotation = transform.parent.rotation;
		transform.Rotate(eulerRotation);
		Debug.Log(transform.rotation.eulerAngles + " " + (eulerRotation.y-transform.rotation.y) + " " + eulerRotation.y);
	}

	public void PointToZero()
	{
		transform.rotation = transform.parent.rotation;
		Debug.Log("To Zero " + transform.rotation.eulerAngles);
	}
}
