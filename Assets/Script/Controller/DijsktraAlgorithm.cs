using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DijsktraAlgorithm : MonoBehaviour {

	private FloorData floorData;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public bool FindShortestPath(FloorData floorData, MarkerData startNode, MarkerData finishNode)
	{
		MarkerData currentNode = startNode;
		startNode.isCurrent = true;
		startNode.isReach = true;
		foreach (MarkerData markerData in floorData.markerList)
		{
			if(markerData.isCurrent)
			{
				currentNode = markerData;
			}
			else if(!markerData.isReach)
			{
				Debug.Log("Reached node:" + markerData.markerName + " ");
				if(markerData.markerName == startNode.markerName)
				{
					markerData.cost = 0;
					markerData.isReach = true;
					markerData.isCurrent = true;
				}
				/* check neighbor and update */
				foreach (MarkerData connectedNode in currentNode.neighborList)
				{
					float costFromThisNode = markerData.cost + Vector3.Distance(markerData.position, connectedNode.position);
					if(costFromThisNode < connectedNode.cost)
					{
						connectedNode.cost = costFromThisNode;
						connectedNode.predecessor = markerData;
						Debug.Log("  Update " + connectedNode.name +" to" + connectedNode.cost);
					}
				}
			}
			else{
				Debug.Log("Skipped node:" + markerData.markerName);
			}
			
		}

		/* choose shortest node to current node */
		MarkerData choosenNode = floorData.markerList[0];
		foreach (MarkerData markerData in floorData.markerList)
		{
			if(!markerData.isReach && choosenNode.cost > markerData.cost) //and not in dead node
			{
				choosenNode = markerData;
				markerData.isReach = true;
				markerData.isCurrent = true;
			}
		}
		return true;
	}

	public void ResetVertexData(FloorData floorData)
	{
		foreach (MarkerData markerData in floorData.markerList)
		{
			markerData.cost = Single.PositiveInfinity;
			markerData.isReach = false;
			markerData.isCurrent = false;
			markerData.predecessor = null;
			markerData.successor = null;
		}
	}
}
