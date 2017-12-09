using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class MapControlScript : MonoBehaviour, IPointerClickHandler
{

    public GameObject mapImageObject;
    public GameObject markerPrefab;
    private GameObject navline, userDot;
    private int tabCount = 0;
    private float maxDoubleTabTime = 0.5f;
    private float newTime;
    private float zoomSpeed = 0.5f;
    private bool IsNormalSize;
    private bool hasMovedFlag = false;
    private bool isUserInThisFloor = false;
    float mapPadding = 30.0f;

    private GameObject[] nodeForLineArr = new GameObject[3];
    private GameObject showingFloor;
    private BuildingData building;

    RectTransform mapImage;

    // Use this for initialization
    void Start()
    {
        Debug.Log("==================Start");
        mapImage = this.gameObject.GetComponent<RectTransform>();
        navline = this.transform.Find("Line").gameObject;
        userDot = this.transform.Find("UserDot").gameObject;
        // !!! warn !!!  showingfloor change when change bulding
        building = GameObject.Find("IT Buiding").GetComponent<BuildingData>();
        showingFloor = building.floorList[0];
    }

    void Awake()
    {
        Debug.Log("===================Awake");
        mapImage = this.gameObject.GetComponent<RectTransform>();
        navline = this.transform.Find("Line").gameObject;
        userDot = this.transform.Find("UserDot").gameObject;
        // !!! warn !!!  showingfloor change when change bulding
        building = GameObject.Find("IT Buiding").GetComponent<BuildingData>();
        showingFloor = building.floorList[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 2)
        {
            //condition that 2 finger once in map
            Debug.Log("2 Touch" + Input.GetTouch(0).phase + "|" + Input.GetTouch(1).phase);
            ZoomMap();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    //implement from ipointerclickhandler for click one time only
    {
        Debug.Log("It's me: " + name);
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            //Debug.Log("  hasmoved:" +hasMovedFlag + " |" +touch.phase);
            if (touch.phase == TouchPhase.Moved)
            {
                hasMovedFlag = true;
            }
            if (touch.phase == TouchPhase.Ended)
            {
                if (!hasMovedFlag)
                {
                    tabCount += 1;
                }
                hasMovedFlag = false;
            }

            if (tabCount == 1)
            {
                newTime = Time.time + maxDoubleTabTime;
            }
            else if (tabCount >= 2 && Time.time <= newTime)
            {
                if (!IsNormalSize)
                {
                    RestoreMap();
                }
                else
                {
                    ZoomMapx2();
                }
                //Whatever you want after a dubble tap    
                print("Dubble tap");
                tabCount = 0;
            }
        }
        if (Time.time > newTime)
        {
            tabCount = 0;
        }
    }

    private void ZoomMapx2()
    {
        ChangeMapSize(
                mapImage.sizeDelta.x * 1.5f,
                mapImage.sizeDelta.y * 1.5f
            );
        IsNormalSize = false;
    }

    private void ZoomMap()
    {
        // Store both touches.
        Touch touchZero = Input.GetTouch(0);
        Touch touchOne = Input.GetTouch(1);

        // Find the position in the previous frame of each touch.
        Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
        Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

        // Find the magnitude of the vector (the distance) between the touches in each frame.
        float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
        float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

        // Find the difference in the distances between each frame.
        float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

        mapImage.sizeDelta = new Vector2(                               // warn sizedelta use 2 here
            mapImage.sizeDelta.x - (deltaMagnitudeDiff * zoomSpeed),
            mapImage.sizeDelta.y - (deltaMagnitudeDiff * zoomSpeed)
        );
        ChangeMapSize(
            Mathf.Clamp(mapImage.sizeDelta.x, Screen.width - mapPadding, 3000),
            Mathf.Clamp(mapImage.sizeDelta.y, Screen.width - mapPadding, 3000)
        );
        //mapImage.anchoredPosition = Vector2.zero;
        IsNormalSize = false;
    }

    private void RestoreMap()
    {
        int mapsize = Mathf.FloorToInt(Screen.width - mapPadding);
        ChangeMapSize(Screen.width - mapPadding, Screen.width - mapPadding);
        mapImage.anchoredPosition = new Vector2(0, 0);
        IsNormalSize = true;
    }

    public void UpdateMap(GameObject floorObject)
    //recive floor obj to change from canvasbutton 
    //change floor pic and find path
    {
        RestoreMap();
        //BuildingData building = floorObject.GetComponent<FloorData>().GetBuilding().GetComponent<BuildingData>();
        // get material from first child of floorData 
        Material floorMaterial = floorObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().materials[0];
        this.gameObject.GetComponent<Image>().material = floorMaterial;
        ShowMarkerOfFloor(floorObject);

        FloorData floorObjectData = floorObject.GetComponent<FloorData>();

        GameObject beginPoint = MainController.instance.beginPoint;
        GameObject destinationPoint = MainController.instance.destinationPoint;        

        //check floor, start stop of that floor for line
        if (MainController.instance.appState == MainController.AppState.Navigate 
            && beginPoint != null && destinationPoint != null)
        {
            MarkerData beginPointData = beginPoint.GetComponent<MarkerData>();
            MarkerData destinationPointData = destinationPoint.GetComponent<MarkerData>();
            GameObject beginFloor = MainController.instance.beginPoint.GetComponent<MarkerData>().GetFloor();
            GameObject destinationFloor = MainController.instance.destinationPoint.GetComponent<MarkerData>().GetFloor();
            FloorData beginFloorData = beginFloor.GetComponent<FloorData>();
            FloorData destinationFloorData = destinationFloor.GetComponent<FloorData>();

            if (beginPointData.IsSameFloorWith(destinationPoint) && beginFloor == floorObject)
            //building.IsSameFloor(MainController.instance.beginPoint, MainController.instance.destinationPoint) //loking fl in same
            {
                nodeForLineArr[0] = beginPoint;
                nodeForLineArr[1] = destinationPoint;
                Debug.Log(" Show Line In Same Floor");
            }
            else
            {
                if (beginFloor == floorObject)
                { //swap to begin fl
                    nodeForLineArr[0] = beginPoint;
                    nodeForLineArr[1] = beginFloorData.GetConnector();
                }
                else if (destinationFloor == floorObject)
                { //swap in dest fl
                    nodeForLineArr[0] = destinationFloorData.GetConnector();
                    nodeForLineArr[1] = destinationPoint;
                }
                else
                {
                    //check is looking floor are inbeetween 
                    //if yes green dot in lift
                    if (beginFloorData.floorIndex < destinationFloorData.floorIndex
                        && floorObjectData.floorIndex < destinationFloorData.floorIndex
                        && floorObjectData.floorIndex > beginFloorData.floorIndex)
                    {
                        nodeForLineArr[0] = floorObjectData.connectorList[0];
                        nodeForLineArr[1] = null;
                    }
                    else if (beginFloorData.floorIndex > destinationFloorData.floorIndex
                      && floorObjectData.floorIndex > destinationFloorData.floorIndex
                      && floorObjectData.floorIndex < beginFloorData.floorIndex)
                    {
                        nodeForLineArr[0] = floorObjectData.connectorList[0];
                        nodeForLineArr[1] = null;
                    }
                    else
                    {
                        nodeForLineArr[0] = null;
                        nodeForLineArr[1] = null;
                    }
                    //if no, will not show line in 
                }
            }
        }
        else if (MainController.instance.appState == MainController.AppState.Idle)
        {
            nodeForLineArr[0] = null;
            nodeForLineArr[1] = null;
        }
        DrawLine();
        //check floor and current position for user dot
        userDot.SetActive(false);
        if (beginPoint != null)
        {
            if (MainController.instance.beginPoint.GetComponent<MarkerData>().GetFloor() == floorObject)
            {
                ShowUserDot(MainController.instance.beginPoint);
                isUserInThisFloor = true;
            }
        }
        showingFloor = floorObject;
    }

    public void ChangeMapSize(float xSize, float ySize)
    {
        Debug.Log("changeMapSize sizedelta " + GetComponent<RectTransform>().sizeDelta);
        mapImage.sizeDelta = new Vector2(xSize, ySize);
        navline.GetComponent<RectTransform>().sizeDelta = mapImage.sizeDelta;
        navline.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        ShowMarkerOfFloor();
        DrawLine();
        if (isUserInThisFloor) { ShowUserDot(MainController.instance.beginPoint); }
    }


    #region In Map Component

    public void ShowMarkerOfFloor() /* resize marker with map size */
    {
        GameObject markers = mapImage.transform.GetChild(0).gameObject;
        //destroy all marker
        foreach (Transform ch in markers.transform)
        {
            Destroy(ch.gameObject);
        }
        //create marker prefab
        List<GameObject> markerList = showingFloor.GetComponent<FloorData>().markerList;
        foreach (GameObject markerob in markerList)
        {
            //instantiate marker at child of Markers
            GameObject markerDot = Instantiate(markerPrefab);
            MarkerData markerdata = markerob.GetComponent<MarkerData>();
            markerDot.transform.SetParent(markers.transform);
            //recttransform coordinate xy 1000/mapimage.sizedelta
            markerDot.GetComponent<RectTransform>().anchoredPosition = new Vector2(
                markerdata.position.x * (mapImage.GetComponent<RectTransform>().sizeDelta.x / 1000),
                markerdata.position.z * (mapImage.GetComponent<RectTransform>().sizeDelta.y / 1000)
            );
        }
    }

    public void ShowMarkerOfFloor(GameObject floorObject) /* show marker in map */
    {
        GameObject markers = mapImage.transform.GetChild(0).gameObject;
        //destroy all marker
        foreach (Transform ch in markers.transform)
        {
            Destroy(ch.gameObject);
        }
        //create marker prefab
        List<GameObject> markerList = floorObject.GetComponent<FloorData>().markerList;
        foreach (GameObject markerob in markerList)
        {
            //instantiate marker at child of Markers
            GameObject markerDot = Instantiate(markerPrefab);
            MarkerData markerdata = markerob.GetComponent<MarkerData>();
            markerDot.transform.SetParent(markers.transform);
            //recttransform coordinate xy 1000/mapimage.sizedelta
            markerDot.GetComponent<RectTransform>().anchoredPosition = new Vector2(
                markerdata.position.x * (mapImage.GetComponent<RectTransform>().sizeDelta.x / 1000),
                markerdata.position.z * (mapImage.GetComponent<RectTransform>().sizeDelta.y / 1000)
            );
        }
    }

    private void DrawLine() /* draw line with map size */
    {
        if (nodeForLineArr[0] == null && nodeForLineArr[1] == null)
        {
            ClearLine();
        }
        else if (nodeForLineArr[0] != null && nodeForLineArr[1] == null)
        {
            ShowLine(nodeForLineArr[0]);
        }
        else if (nodeForLineArr[0] != null && nodeForLineArr[1] != null)
        {
            ShowLine(nodeForLineArr[0], nodeForLineArr[1]);
        }
    }

    private void ShowLine(GameObject begin, GameObject destination) /* show green navigate line on map */
    {
        UILineRenderer line = navline.GetComponent<UILineRenderer>();
        line.Points.Clear();
        MarkerData checkPoint = begin.GetComponent<MarkerData>();
        int i = 0;
        //Debug.Log("Write Line At " + checkPoint.markerName + " " + line.Points(i));
        while (checkPoint.successor != null)
        {
            Debug.Log("Checking Point are " + checkPoint.markerName);
            // last point point to marker position
            line.Points.Add(new Vector2(
                checkPoint.referencePosition.x * (mapImage.GetComponent<RectTransform>().sizeDelta.x / 1000),
                checkPoint.referencePosition.z * (mapImage.GetComponent<RectTransform>().sizeDelta.y / 1000)
            ));
            i++;
            checkPoint = checkPoint.successor.GetComponent<MarkerData>();
        }
        // add last point
        line.Points.Add(new Vector2(
            checkPoint.referencePosition.x * (mapImage.GetComponent<RectTransform>().sizeDelta.x / 1000),
            checkPoint.referencePosition.z * (mapImage.GetComponent<RectTransform>().sizeDelta.y / 1000)
        ));
        line.Points.Add(new Vector2(
            checkPoint.position.x * (mapImage.GetComponent<RectTransform>().sizeDelta.x / 1000),
            checkPoint.position.z * (mapImage.GetComponent<RectTransform>().sizeDelta.y / 1000)
        ));
        line.SetVerticesDirty();
    }

    private void ShowLine(GameObject point) /*draw line on connector point */
    {
        Debug.Log(" in Lift");
        UILineRenderer line = navline.GetComponent<UILineRenderer>();
        line.Points.Clear();
        MarkerData checkPoint = point.GetComponent<MarkerData>();

        line.Points.Add(new Vector2(
            checkPoint.position.x * (mapImage.GetComponent<RectTransform>().sizeDelta.x / 1000),
            checkPoint.position.z * (mapImage.GetComponent<RectTransform>().sizeDelta.y / 1000)
        ));
        line.Points.Add(new Vector2(
            checkPoint.referencePosition.x * (mapImage.GetComponent<RectTransform>().sizeDelta.x / 1000),
            checkPoint.referencePosition.z * (mapImage.GetComponent<RectTransform>().sizeDelta.y / 1000)
        ));
        line.SetVerticesDirty();
    }

    private void ClearLine() /* don't show line in that floor */
    {
        UILineRenderer line = navline.GetComponent<UILineRenderer>();
        line.Points.Clear();
        line.SetVerticesDirty();
    }

    private void ShowUserDot(GameObject point)
    {
        userDot.SetActive(true);
        MarkerData markerdata = point.GetComponent<MarkerData>();
        RectTransform dotRect = userDot.GetComponent<RectTransform>();
        dotRect.anchoredPosition = new Vector2(
            markerdata.referencePosition.x * (mapImage.GetComponent<RectTransform>().sizeDelta.x / 1000),
            markerdata.referencePosition.z * (mapImage.GetComponent<RectTransform>().sizeDelta.y / 1000)
        );
        float deltaX = markerdata.referencePosition.x - markerdata.position.x;
        float deltaY = markerdata.referencePosition.z - markerdata.position.z;
        dotRect.rotation = Quaternion.Euler(new Vector3(0, 0,
            (((Mathf.Atan2(deltaY, deltaX)) * 180 / Mathf.PI) + 90)
        ));
    }
    #endregion

}
