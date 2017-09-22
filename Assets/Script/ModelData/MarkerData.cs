using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerData : MonoBehaviour {

	[Header("Marker Data")]
	public string floor = "0";
	public string markerName = "";
	public string description = "";
	public string roomName = "";

	[Header("Vertex Data")]
	public float cost = Single.PositiveInfinity;
	public List<MarkerData> neighborList;
	public bool isReach;
	public bool isCurrent;
	public MarkerData predecessor = null;
	public MarkerData successor = null;

	public enum NodeType
	{
		None,
		Room,
		Junction,
		Both
	}

	public NodeType nodeType = NodeType.None;

	public Vector3 position = Vector3.zero;
	public Vector3 orientation = Vector3.zero;

	// Use this for initialization
	void Start () {
		position = transform.position;
		orientation = transform.rotation.eulerAngles;
		Debug.Log("Start " + gameObject.name + " " +position);
		markerName = gameObject.name.Replace("ImageTarget ","");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/* Set room data from someWhere */
	public void SetRoomData(string name, string des)
	{
		roomName = name;
		description = des;
	}


	#region Check Node Type
	/* Method for Check Node Type */
	public bool IsJunctionNode()
	{
		return nodeType == NodeType.Junction || nodeType == NodeType.Both ? true : false ;
	}

	public bool IsRoomNode()
	{
		return nodeType == NodeType.Room || nodeType == NodeType.Both ? true : false ;
	}

	public bool IsNoneNode()
	{
		return nodeType == NodeType.None;
	}
	#endregion
}
