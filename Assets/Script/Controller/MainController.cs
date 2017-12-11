using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour
{

    public static MainController instance;
    private DijsktraAlgorithm dijsktra;
    private CanvasButtonScript canvasButton;
    private ShowStateManager showState;
    //private CanvasResolutionScript canvasResolution;
    public GameObject beginPoint = null;
    public GameObject destinationPoint = null;
    public GameObject reachedPoint = null;
    private GameObject oldBeginPoint = null, oldDestinationPoint = null, oldReachePoint = null;
    public enum AppState
    {
        Idle,
        Navigate
    }

    public AppState appState = AppState.Idle;
    private AppState oldAppState = AppState.Idle;
    public bool navigatable = false; //due arcontrolscr use

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            dijsktra = new DijsktraAlgorithm();
            canvasButton = GameObject.Find("Canvas").GetComponent<CanvasButtonScript>();
            //canvasResolution = GameObject.Find("Canvas").GetComponent<CanvasResolutionScript>();
            showState = GameObject.Find("Canvas").GetComponent<ShowStateManager>();
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
        /* observer zone, control state changing */
        if (appState != oldAppState)
        {
            //call observer
            oldAppState = appState;
        }
    }

    #region SetPoint

    public void SetBeginPoint(GameObject beginPoint)
    /* create started AR when camera detect marker */
    {
        if (beginPoint.GetComponent<MarkerData>() != null)
        {
            this.beginPoint = beginPoint;
            Debug.Log("Set Begin Point to " + beginPoint.GetComponent<MarkerData>().roomName);
        }
        //canvasButton.OnBeginPointChange(beginPoint);
        switch (appState)
        {
            case AppState.Idle:
                if (this.beginPoint != null && this.destinationPoint != null)       // already has destpoint
                {
                    NavigateIfNotSamePoint(); //dest not are this pos
                }
                break;
            case AppState.Navigate:         //beginpoint will not null, dest not null in nav
                NavigateIfNotSamePoint();   //new pos not same dest
                break;
            default:
                appState = AppState.Idle;
                break;
        }
        showState.OnBeginPointChange(beginPoint);
        oldBeginPoint = beginPoint;
        ShowAR(beginPoint); //remove if user can set
    }
    public void SetDestinationPoint(GameObject destinationPoint)
    {
        //if (destinationPoint.GetComponent<MarkerData>() != null)
        //{
        this.destinationPoint = destinationPoint;
        //Debug.Log("Set Destination Point to " + destinationPoint.GetComponent<MarkerData>().roomName);
        //}
        //canvasButton.OnDestinationPointChange(destinationPoint);
        switch (appState)
        {
            case AppState.Idle:
                if (this.beginPoint != null && this.destinationPoint != null)   // already has begin point, start navigate
                {
                    NavigateIfNotSamePoint();
                }
                break;
            case AppState.Navigate:
                if (this.destinationPoint == null)
                {
                    appState = AppState.Idle;
                }
                else if (this.beginPoint != null && this.destinationPoint != null)   // already has begin point, start navigate
                {
                    NavigateIfNotSamePoint();       // select dest same as current point false
                }
                break;
            default:
                appState = AppState.Navigate;
                break;
        }
        showState.OnDestinationPointChange(destinationPoint);
        oldDestinationPoint = destinationPoint;
    }
    public void ClearDestinationPoint()
    {
        SetDestinationPoint(null);
        this.reachedPoint = null;
        appState = AppState.Idle;
    }

    #endregion

    #region Navigate

    private bool NavigateIfNotSamePoint()
    /* check before navigate that two new point didn't met destination 
    return true if two point didn't same room and go to navigate*/
    {
        if (this.beginPoint != null && this.destinationPoint != null)
        {
            if (this.beginPoint.GetComponent<MarkerData>().roomName
                == this.destinationPoint.GetComponent<MarkerData>().roomName)
            {
                this.reachedPoint = this.destinationPoint;
                this.destinationPoint = null;
                appState = AppState.Idle;
                return false;
            }
            appState = AppState.Navigate;
            Navigate();
        }
        return true;
    }

    public void Navigate()
    /* run navigate algorithm by usion beginpoint and destpoint to update all markerdata value
	if in same floor, navigate to connecttor of floor 
	if can't find route, arrow will directly point to destination*/
    {
        BuildingData building = this.beginPoint.GetComponent<MarkerData>()
                .GetFloor().GetComponent<FloorData>()
                .GetBuilding().GetComponent<BuildingData>();

        if (building.IsSameFloor(this.beginPoint, this.destinationPoint))
        {
            Debug.Log("Same Floor  Navigating " + this.beginPoint.GetComponent<MarkerData>().floor);
            if (this.beginPoint == this.destinationPoint) //                                <<<<< check room name
            {
                Debug.Log("=== Founded Destination ===");
                appState = AppState.Idle;
                Debug.Log(" Reach at Main navigate and idle");
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
            Debug.Log("==================================== Different Floor  Navigating " + this.beginPoint.GetComponent<MarkerData>().floor + " To " + this.destinationPoint.GetComponent<MarkerData>().floor);
            bool begintoLift = dijsktra.FindShortestPath(
                    this.beginPoint.GetComponent<MarkerData>().GetFloor(),
                    this.beginPoint, building.GetConnector(this.beginPoint));
            Debug.Log("==================================== brk");
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
    #endregion

    #region ShowAR
    //function recive prefabType loop all child if met active it and flag as met, if meet more than one destroy it
    public void ShowAR(GameObject objectToAugment)
    /* show AR depending on state, works with ArControlScript, navigate before show */
    {
        ArControlScript arControl = objectToAugment.GetComponent<ArControlScript>();
        foreach (Transform child in objectToAugment.transform)
        {
            child.gameObject.SetActive(false);
        }

        if (appState == AppState.Idle) //set desboard if not in old node or navigated to destination
        {
            if (this.beginPoint != null && this.reachedPoint != null)
            {
                if (objectToAugment.GetComponent<MarkerData>().roomName == this.reachedPoint.GetComponent<MarkerData>().roomName)
                {
                    arControl.CreateCheckTrue();
                    Debug.Log(" Reach at Main ShowAR Idle Mode and idle");
                }
                else
                {
                    arControl.CreateDescriptionBoard();
                }
            }
            else if (this.beginPoint != null)
            {
                arControl.CreateDescriptionBoard();
            }
        }
        else if (appState == AppState.Navigate)  //set arrow
        {
            arControl.CreateArrow();
            if (navigatable)
            {
                Debug.Log("navigatable point to " + objectToAugment.GetComponent<MarkerData>().successor.GetComponent<MarkerData>().position);
                arControl.GetArrow().GetComponent<ArrowScript>().PointToCoordinate(
                    objectToAugment.GetComponent<MarkerData>().successor.GetComponent<MarkerData>().position);
            }
            else
            {
                int beginfloor = this.beginPoint.GetComponent<MarkerData>().GetFloor().GetComponent<FloorData>().floorIndex;
                int destfloor = this.destinationPoint.GetComponent<MarkerData>().GetFloor().GetComponent<FloorData>().floorIndex;
                if (beginfloor < destfloor)
                {
                    Debug.Log("point down");
                    arControl.GetArrow().GetComponent<ArrowScript>().PointArrowUp();
                }
                else if (beginfloor > destfloor)
                {
                    Debug.Log("point up");
                    arControl.GetArrow().GetComponent<ArrowScript>().PointArrowDown();
                }
                else
                {
                    Debug.Log("ObjecttoAugmnt " + objectToAugment.name + " pointing");
                    arControl.GetArrow().GetComponent<ArrowScript>()
                            .PointToCoordinate(this.destinationPoint.GetComponent<MarkerData>().position);
                    objectToAugment.GetComponentInChildren<ArrowScript>()
                            .PointToCoordinate(this.destinationPoint.GetComponent<MarkerData>().position);
                }
            }
        }
    }
    #endregion
}
