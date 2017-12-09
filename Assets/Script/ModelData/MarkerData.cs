using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerData : MonoBehaviour, ICloneable {

	[Header("Marker Data")]
	public Vector3 position = Vector3.zero;
	public Vector3 referencePosition = Vector3.zero;
	public Vector3 orientation = Vector3.zero;
	public string floor = "0";
	public string markerName = "";
	public string description = "";
	public string roomName = "";
	public bool IsConnector = false;

	[Header("Vertex Data")]
	public float cost = Single.PositiveInfinity;
	public List<GameObject> adjacentNodeList;
	public GameObject predecessor = null;
	public GameObject successor = null;

	public enum NodeType
	{
		None,
		Room,
		Junction,
		Both
	}

	public NodeType nodeType = NodeType.None;



	// Use this for initialization
	void Start () {
		position = transform.localPosition;
		Transform refPoint = transform.Find("ReferencePoint");
		refPoint.SetParent(transform.parent);
		referencePosition = refPoint.localPosition; //or from orientation + 2 radius
		Destroy(refPoint.gameObject);
		orientation = transform.rotation.eulerAngles;
		Debug.Log("Start " + gameObject.name + " " +position);
		markerName = gameObject.name.Replace("ImageTarget ","");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/* deep clone */
	

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

	public object Clone()
    {
		MarkerData markerDataClone = new MarkerData();
		markerDataClone.position = this.position ;
		markerDataClone.referencePosition = this.referencePosition ;
		markerDataClone.orientation = this.orientation ;
		markerDataClone.floor = this.floor;
		markerDataClone.markerName = this.markerName;
		markerDataClone.description = this.description ;
		markerDataClone.roomName = this.roomName ;
		markerDataClone.IsConnector = this.IsConnector;
		
		markerDataClone.cost = this.cost ;
		markerDataClone.adjacentNodeList = this.adjacentNodeList ;
		markerDataClone.predecessor = this.predecessor ;
		markerDataClone.successor = this.successor ;
		markerDataClone.nodeType = this.nodeType ;
		
		return markerDataClone;
    }

#region Floor Method

	public GameObject GetFloor() /* return gameObject of floorData */
	{
		//if(this.transform.parent.gameObject.GetComponent<FloorData>() != null)
		return this.transform.parent.gameObject;
	}

	public bool IsSameFloorWith(string compareFloor)
	{
		if(this.floor == compareFloor)
		{
			return true;
		}
		return false;
	}

	public bool IsSameFloorWith(GameObject compareFloorObj)
	{
		if(this.floor == compareFloorObj.GetComponent<MarkerData>().floor)
		{
			return true;
		}
		return false;
	}
#endregion
	
}
