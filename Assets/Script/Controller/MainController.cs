using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour {

	public static MainController instance;
	private DijsktraAlgorithm dijsktra;
	private CanvasButtonScript canvasButton;
	private CanvasResolutionScript canvasResolution;
	public GameObject beginPoint = null, destinationPoint = null;
	public enum AppState
	{
		Idle,
		Navigate
	}

	public AppState appState = AppState.Idle;

	void Awake () {
		if (instance == null) {
			instance = this;
			dijsktra = new DijsktraAlgorithm();
			canvasButton = GameObject.Find("Canvas").GetComponent<CanvasButtonScript>();
			canvasResolution = GameObject.Find("Canvas").GetComponent<CanvasResolutionScript>();
		} else if (instance != this) {
			Destroy (gameObject);
		}
		DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void NavigateFrom(GameObject currentPoint) 
	/* get currrent point set to begin and run navigate 
	if in same floor, navigate to connecttor of floor 
	if can't find route, arrow will directly point to destination*/
	{
		appState = AppState.Navigate;
		SetBeginPoint(currentPoint);
		BuildingData building = this.beginPoint.GetComponent<MarkerData>()
				.GetFloor().GetComponent<FloorData>()
				.GetBuilding().GetComponent<BuildingData>();

		if (building.IsSameFloor(this.beginPoint, this.destinationPoint)) 
		{
			if (this.beginPoint == this.destinationPoint){
				Debug.Log("=== Founded Destination ===");
				appState = AppState.Idle;
			} else if (dijsktra.FindShortestPath(this.beginPoint.GetComponent<MarkerData>().GetFloor(), 
					this.beginPoint, this.destinationPoint)) {
				this.beginPoint.gameObject.transform.GetChild(0).gameObject.GetComponent<ArrowScript>()
					.PointToCoordinate(this.beginPoint.GetComponent<MarkerData>().successor.GetComponent<MarkerData>().position);
			} else {
				this.beginPoint.gameObject.transform.GetChild(0).gameObject.GetComponent<ArrowScript>()
					.PointToCoordinate(this.destinationPoint.GetComponent<MarkerData>().position);
			}
		} 
		else 
		{
			if (dijsktra.FindShortestPath(
					this.beginPoint.GetComponent<MarkerData>().GetFloor(), 
					this.beginPoint, building.GetConnector(this.beginPoint)) 
				&& dijsktra.FindShortestPath(
						this.beginPoint.GetComponent<MarkerData>().GetFloor(),
						building.GetConnector(this.destinationPoint), this.destinationPoint)
				)
			{
				this.beginPoint.gameObject.transform.GetChild(0).gameObject.GetComponent<ArrowScript>()
					.PointToCoordinate(this.beginPoint.GetComponent<MarkerData>().successor.GetComponent<MarkerData>().position);
			} else {
				this.beginPoint.gameObject.transform.GetChild(0).gameObject.GetComponent<ArrowScript>()
					.PointToCoordinate(this.destinationPoint.GetComponent<MarkerData>().position);
			}
		}
		//case point to null of successor
	}

	public void SetBeginPoint(GameObject beginPoint)
	{
		if(beginPoint.GetComponent<MarkerData>() != null)
		{
			this.beginPoint = beginPoint;
			Debug.Log("Set Begin Point to " + beginPoint.GetComponent<MarkerData>().roomName);
		}
		
	}

	public void SetDestinationPoint(GameObject destinationPoint)
	{
		if(destinationPoint.GetComponent<MarkerData>() != null)
		{
			this.destinationPoint = destinationPoint;
			Debug.Log("Set Destination Point to " + destinationPoint.GetComponent<MarkerData>().roomName);
		}
		
	}

	// private bool IsSameFloor(GameObject firstNode, GameObject secondNode) /* check bool is same floor */
	// {
	// 	return firstNode.GetComponent<MarkerData>().floor == secondNode.GetComponent<MarkerData>().floor;
	// }

	// private GameObject GetConnector(GameObject Node) /* get gameObject of connector of that node floor, May have overloading*/
	// {
	// 	if(Node.GetComponent<MarkerData>().IsConnector) {
	// 		return Node;
	// 	} else { //return first of connector
	// 		return Node.GetComponent<MarkerData>().GetFloor().GetComponent<FloorData>().connectorList[0];
	// 	}
	// }
}
