using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomButtonScript : MonoBehaviour {

	public GameObject room;
	public bool isDestination = false;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OpenRoomDialog()
	{
		gameObject.transform.root.GetComponent<CanvasButtonScript>().OnOpenRoomDialoge(room, isDestination);
	}

	public void PrintName()
	{
		Debug.Log(room.GetComponent<MarkerData>().roomName);
	}
}
