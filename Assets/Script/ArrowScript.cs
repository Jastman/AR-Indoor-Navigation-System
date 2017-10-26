using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour {

	MarkerData targetData;
	private const int markerXRotation = 270;

	// Use this for initialization
	void Start () {
		targetData = transform.parent.gameObject.GetComponent<MarkerData>();
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

	public void PointToCoordinate(Vector3 destination) /* Get Destination of object to point, return degree and rotate arrow */
	{	
		targetData = transform.parent.gameObject.GetComponent<MarkerData>();
		float planeDistance = Vector2.Distance(targetData.position, destination);
		float deltaX = targetData.position.x - destination.x;
		float deltaY = targetData.position.z - destination.z;
		
		transform.Rotate(0, 0, (((Mathf.Atan2(deltaY, -deltaX))*180/Mathf.PI) + markerXRotation) + (360-targetData.orientation.y) );
		Debug.Log("-- " + targetData.position + " " + destination);
		Debug.Log("-- Point ToCoorDinate -- " + deltaY + " " + deltaX + " "  + (((Mathf.Atan2(deltaY, -deltaX))*180/Mathf.PI) + 270));
	}

	public void PrintSmt(string p){
		Debug.Log(p);
	}
}
