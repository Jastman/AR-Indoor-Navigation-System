using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DijsktraAlgorithm : MonoBehaviour {

	private FloorData floorData;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public bool FindShortestPath(GameObject floorObject, GameObject startNode, GameObject finishNode)
	{
		ResetAllVertexData(floorObject);
		FloorData currentFloor = floorObject.GetComponent<FloorData>();
		
		List<GameObject> unVisitedList = floorObject.GetComponent<FloorData>().markerList.ToList(); 
		
		bool isFounded = false;
		float costToadjacentNode = 0;
		GameObject currentNode = startNode;
		MarkerData currentMarkerData = currentNode.GetComponent<MarkerData>();
		currentMarkerData.cost = 0;

		Debug.Log(" first node cost 0");
		
		Debug.Log(" - - - - " + (unVisitedList.Count > 0));
		while(currentNode != finishNode && (unVisitedList.Count>=0) ) //unVisitedList.Count.CompareTo(0)  ||  (unvisitedLeft > 0)   
		{
			
			//check adjacentNode
			foreach (GameObject adjacentObject in currentMarkerData.adjacentNodeList)
			{
				MarkerData adjacentMarkerData = adjacentObject.GetComponent<MarkerData>();
				Debug.Log(" -@ " + currentMarkerData.markerName +
						"  -Adjacent |" + adjacentMarkerData.markerName + 
						"| End at " +finishNode.GetComponent<MarkerData>().markerName);
				
				bool test = (adjacentMarkerData.markerName == finishNode.GetComponent<MarkerData>().markerName);
				if(adjacentObject == finishNode) { // if adjacent is final node
					adjacentMarkerData.cost = Vector3.Distance(currentMarkerData.position, adjacentMarkerData.position) + currentMarkerData.cost;
					unVisitedList.Remove(currentNode);
					unVisitedList.Remove(adjacentObject);
					adjacentMarkerData.predecessor = currentNode;
					currentNode = adjacentObject;
					currentMarkerData = currentNode.GetComponent<MarkerData>();
					isFounded = true;
					Debug.Log(" - - Break - -");
					break;
				} else if (unVisitedList.Contains(adjacentObject) ) { //neightbor are not visited, Update it
					costToadjacentNode = Vector3.Distance(currentMarkerData.position, adjacentMarkerData.position) + currentMarkerData.cost;
					if (costToadjacentNode < adjacentMarkerData.cost) 
					{
						Debug.Log("cost from here are less than older " + costToadjacentNode + "<" + adjacentMarkerData.cost);
						adjacentMarkerData.cost = costToadjacentNode;
						adjacentMarkerData.predecessor = currentNode;
						Debug.Log("Predecessor of "+adjacentMarkerData.markerName+ " are " + adjacentMarkerData.predecessor.GetComponent<MarkerData>().markerName);
					}
					
					Debug.Log("  Update "+ adjacentMarkerData.markerName+ "  to " + adjacentMarkerData.cost );
				}
			}
			if(isFounded){break;}

			// find Least cost  and choose to current node
			GameObject leastCostNode = finishNode;
			foreach (GameObject unVisitedObj in unVisitedList)
			{
				MarkerData unVisitedNode = unVisitedObj.GetComponent<MarkerData>();
				if(unVisitedNode.cost < leastCostNode.GetComponent<MarkerData>().cost && unVisitedObj != currentNode)
				{
					leastCostNode = unVisitedObj;
				}
				Debug.Log("Compare "+unVisitedNode.markerName+"-  "+unVisitedNode.cost+"<"+leastCostNode.GetComponent<MarkerData>().cost 
					+"  Least cost are:" + leastCostNode.GetComponent<MarkerData>().markerName + " cost:" + leastCostNode.GetComponent<MarkerData>().cost);
			}
			unVisitedList.Remove(currentNode);
			Debug.Log("change predecessor of leastcostnode|" +leastCostNode.GetComponent<MarkerData>().markerName+
				 "| form " + leastCostNode.GetComponent<MarkerData>().predecessor + " To " + currentNode);
			currentNode = leastCostNode;
			currentMarkerData = currentNode.GetComponent<MarkerData>();
			Debug.Log(" =====" +" Unvisited left " + unVisitedList.Count + " >=0 is " + (unVisitedList.Count>=0));
			Debug.Log("===== CurrentNode are " + currentNode.GetComponent<MarkerData>().markerName );
			
		}

		foreach (GameObject prnode in floorObject.GetComponent<FloorData>().markerList)
		{
			Debug.Log("CheckCost " + prnode.name+" " +prnode.GetComponent<MarkerData>().cost + " " );
		}

		/* set successor from reverse finishNode's preDecessor */
		currentNode = finishNode;
		Debug.Log(" From : " + finishNode.GetComponent<MarkerData>().markerName);
		while (currentNode != startNode)
		{
			currentNode.GetComponent<MarkerData>().predecessor.GetComponent<MarkerData>().successor = currentNode;
			currentNode = currentNode.GetComponent<MarkerData>().predecessor;
			Debug.Log(currentNode.GetComponent<MarkerData>().markerName);
		}
		Debug.Log(" To : " + startNode.GetComponent<MarkerData>().markerName);

		return isFounded;
	}

	private GameObject FindGameObjectFromMarkerData(GameObject floorObject, MarkerData markerData)
	{
		foreach (GameObject markerob in floorObject.GetComponent<FloorData>().markerList)
		{
			if(markerob.GetComponent<MarkerData>().markerName == markerData.markerName)
			{
				return markerob;
			}
		}
		return floorObject.GetComponent<FloorData>().markerList[0].gameObject;
	}

	public void ResetAllVertexData(GameObject floorObject)
	{
		foreach (GameObject markerObj in floorObject.GetComponent<FloorData>().markerList)
		{
			MarkerData markerData = markerObj.GetComponent<MarkerData>();
			markerData.cost = Single.PositiveInfinity;
			markerData.predecessor = null;
			markerData.successor = null;
		}
	}


}
