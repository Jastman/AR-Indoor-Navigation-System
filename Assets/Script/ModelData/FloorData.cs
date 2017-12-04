using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorData : MonoBehaviour {

	public List<GameObject> markerList;
	public List<GameObject> connectorList; //this list may check when remove
	public string floorName = "0";
	public int floorIndex = 0;

	// Use this for initialization
	void Start () {
		/* Work after All Marker Data get their position */
		floorName = gameObject.name.Replace("Floor","");
		foreach (Transform childTransform in transform)
		{
			GameObject objToAdd = childTransform.gameObject;
			if(objToAdd.GetComponent<MarkerData>() != null)
			{
				MarkerData objData = objToAdd.GetComponent<MarkerData>();
				Debug.Log("Get Marker " + objData.markerName 
					+ ": " +objData.position);
				objData.floor = floorName;
				markerList.Add(objToAdd);
				if(objData.IsConnector) {connectorList.Add(objToAdd);}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public List<GameObject> GetMarkerOfRoom(string rname) /* return all marker object that have same name that provide, nullable */
	{
		List<GameObject> resultList = null;
		foreach (GameObject marker in markerList)
		{
			if(marker.GetComponent<MarkerData>().roomName == rname) {
				resultList.Add(marker);
			}
		}
		return resultList;
	}

	public GameObject GetBuilding()
	{
		//if(this.transform.parent.gameObject.GetComponent<BuildingData>() != null)
		return this.transform.parent.gameObject;
	}

	public GameObject GetConnector() /* no attribute, return first connector on list */
	{
		return connectorList[0];
	}

	public GameObject GetConnector(GameObject node) /* has node, Get Nearest connector object | Fix later */
	{
		return GetConnector();
	}
}
