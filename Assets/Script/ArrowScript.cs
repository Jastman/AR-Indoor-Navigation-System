using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void RotateArrow(Vector3 eulerRotation)
	{
		transform.Rotate(eulerRotation, Space.World);
		transform.Rotate(eulerRotation, Space.World);
		Debug.Log(transform.rotation + " " + (eulerRotation.x-transform.rotation.x));
	}
}
