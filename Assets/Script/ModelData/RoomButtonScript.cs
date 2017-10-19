using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomButtonScript : MonoBehaviour {

	private GameObject mainController;
	public GameObject room;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetDestination()
	{
		MainController.instance.SetDestinationPoint(room);
	}

	public void PrintName()
	{
		Debug.Log(room.GetComponent<MarkerData>().roomName);
	}
}
