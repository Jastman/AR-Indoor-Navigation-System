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
		if(MainController.instance.beginPoint != null && MainController.instance.destinationPoint != null) {
			MainController.instance.Navigate();
		}
	}

	public void PrintName()
	{
		Debug.Log(room.GetComponent<MarkerData>().roomName);
	}
}
