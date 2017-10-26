using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DescriptionBoardScript : MonoBehaviour {

	private GameObject roomNameBox, roomNameText, roomDestBox, roomDestText;

	// Use this for initialization
	void Start () {
		//dead code
		roomNameBox = gameObject.transform.Find("RoomName").gameObject;
		roomNameText = gameObject.transform.Find("RoomNameText").gameObject;
		roomDestBox = gameObject.transform.Find("RoomDescription").gameObject;
		roomDestText = gameObject.transform.Find("RoomDescriptionText").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetRoomName(string roomname)
	{
		Debug.Log(roomname + " ");
		gameObject.transform.Find("RoomNameText").gameObject.GetComponent<TextMesh>().text = roomname;
		//if string longer than xxx resize 
	}

	public void SetRoomDest(string roomdest) 
	{
		gameObject.transform.Find("RoomDescriptionText").gameObject.GetComponent<TextMesh>().text = roomdest;
		//add linebreaker and fit board with text
	}
}
