using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorData : MonoBehaviour {

	public List<GameObject> markerList;
	public List<GameObject> connectorList; //this list may check when remove
	public string floorName = "0";

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

	public GameObject GetBuilding()
	{
		//if(this.transform.parent.gameObject.GetComponent<BuildingData>() != null)
		return this.transform.parent.gameObject;
	}

	
}
