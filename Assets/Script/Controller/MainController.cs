using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour
{

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
    private bool navigatable = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            dijsktra = new DijsktraAlgorithm();
            canvasButton = GameObject.Find("Canvas").GetComponent<CanvasButtonScript>();
            canvasResolution = GameObject.Find("Canvas").GetComponent<CanvasResolutionScript>();
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Navigate()
    /* run navigate algorithm by usion beginpoint and destpoint to update all markerdata value
	if in same floor, navigate to connecttor of floor 
	if can't find route, arrow will directly point to destination*/
    {
        appState = AppState.Navigate;
        BuildingData building = this.beginPoint.GetComponent<MarkerData>()
                .GetFloor().GetComponent<FloorData>()
                .GetBuilding().GetComponent<BuildingData>();

        if (building.IsSameFloor(this.beginPoint, this.destinationPoint))
        {
            Debug.Log("Same Floor  Navigating " + this.beginPoint.GetComponent<MarkerData>().floor);
            if (this.beginPoint == this.destinationPoint)
            {
                Debug.Log("=== Founded Destination ===");
                appState = AppState.Idle;
            }
            else if (dijsktra.FindShortestPath(this.beginPoint.GetComponent<MarkerData>().GetFloor(),
                  this.beginPoint, this.destinationPoint))
            {
                navigatable = true;
            }
            else
            {
                navigatable = false;
            }
        }
        else
        {
            Debug.Log("Different Floor  Navigating " + this.beginPoint.GetComponent<MarkerData>().floor +" To " + this.destinationPoint.GetComponent<MarkerData>().floor);
            bool begintoLift = dijsktra.FindShortestPath(
                    this.beginPoint.GetComponent<MarkerData>().GetFloor(),
                    this.beginPoint, building.GetConnector(this.beginPoint));
            bool liftToDest = dijsktra.FindShortestPath(
                    this.destinationPoint.GetComponent<MarkerData>().GetFloor(),
                    building.GetConnector(this.destinationPoint), this.destinationPoint);
            if (begintoLift && liftToDest)
            {
                navigatable = true;
            }
            else
            {
                navigatable = false;
            }
        }
        //case point to null of successor
    }

    public void SetBeginPoint(GameObject beginPoint)
    /* create started AR when camera detect marker */
    {
        if (beginPoint.GetComponent<MarkerData>() != null)
        {
            this.beginPoint = beginPoint;
            Debug.Log("Set Begin Point to " + beginPoint.GetComponent<MarkerData>().roomName);
        }
        // not navigate here
    }

    //function recive prefabType loop all child if met active it and flag as met, if meet more than one destroy it

    public void SetDestinationPoint(GameObject destinationPoint)
    {
        if (destinationPoint.GetComponent<MarkerData>() != null)
        {
            this.destinationPoint = destinationPoint;
            canvasButton.OnCloseSerch();
            Debug.Log("Set Destination Point to " + destinationPoint.GetComponent<MarkerData>().roomName);
        }

    }

    public void ShowAR() /* show AR depending on state, works with ArControlScript, navigate before show */
    {
        foreach (Transform child in this.beginPoint.transform)
        {
            child.gameObject.SetActive(false);
        }

        if (appState == AppState.Idle) //set desboard if not in old node or navigated to destination
        {
			if(this.beginPoint != null && this.destinationPoint != null)
			{
				if (this.beginPoint.GetComponent<MarkerData>().roomName == this.destinationPoint.GetComponent<MarkerData>().roomName)
				{
					this.beginPoint.GetComponent<ArControlScript>().CreateCheckTrue();
					appState = AppState.Idle;
				}
				else 
				{
					this.beginPoint.GetComponent<ArControlScript>().CreateDescriptionBoard();
				}
			}
			else
			{
				this.beginPoint.GetComponent<ArControlScript>().CreateDescriptionBoard();
			}
        }
        else if (appState == AppState.Navigate)  //set arrow
        {
            if (this.beginPoint.GetComponent<MarkerData>().roomName == this.destinationPoint.GetComponent<MarkerData>().roomName)
            {
                this.beginPoint.GetComponent<ArControlScript>().CreateCheckTrue();
                appState = AppState.Idle;
            }
            else //create arrow and point to next node/ 
            {
                this.beginPoint.GetComponent<ArControlScript>().CreateArrow();
                if (navigatable)
                {
					Debug.Log("point to " +this.beginPoint.GetComponent<MarkerData>().successor.GetComponent<MarkerData>().position);
                    this.beginPoint.GetComponent<ArControlScript>().GetArrow().GetComponent<ArrowScript>()
                            .PointToCoordinate(this.beginPoint.GetComponent<MarkerData>().successor.GetComponent<MarkerData>().position);
                }
                else
                {
                    this.beginPoint.GetComponentInChildren<ArrowScript>()
                            .PointToCoordinate(this.destinationPoint.GetComponent<MarkerData>().position);
                }
            }
        }
    }

}
