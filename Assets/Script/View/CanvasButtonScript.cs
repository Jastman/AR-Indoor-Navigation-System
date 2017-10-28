using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class CanvasButtonScript : MonoBehaviour
{

    public GameObject markerPrefab, roomButtonPrefab;
    public GameObject actionBar;
    public GameObject searchPanel, mapPanel; //panel
    private GameObject hambergerButton, mapButton, searchButton, backButton, clearButton; //button
    private GameObject searchInputField, appName; //InputFields + text
    private Text appNameText;

    private GameObject searchHelpText, searchList, viewPort, scrollbar, searchContent;
    private GameObject mapImage, rightButton, leftButton, navline, userDot;

    private BuildingData building;
    private GameObject showingFloor;
    private List<GameObject> searchShowList;

    private enum Page
    {
        Main,
        Search,
        Map
    }
    private Page page = Page.Main;

    private CanvasResolutionScript canvasResolutionScript;
    private ToastMessageScript toastMessageScript;

    private bool canQuitApp = true;

    // Use this for initialization
    void Start()
    {
        building = GameObject.Find("IT Buiding").GetComponent<BuildingData>();
        showingFloor = building.floorList[0];
        searchShowList = new List<GameObject>();

        canvasResolutionScript = gameObject.GetComponent<CanvasResolutionScript>();
        toastMessageScript = gameObject.GetComponent<ToastMessageScript>();
        toastMessageScript.showToastOnUiThread(UIValue.value.STRING_FINDMARKER);

        hambergerButton = actionBar.gameObject.transform.Find("HambergerButton").gameObject;
        mapButton = actionBar.gameObject.transform.Find("MapButton").gameObject;
        searchButton = actionBar.gameObject.transform.Find("SearchButton").gameObject;
        appName = actionBar.gameObject.transform.Find("AppName").gameObject;
        appNameText = appName.GetComponent<Text>();

        backButton = actionBar.gameObject.transform.Find("BackButton").gameObject;
        searchInputField = actionBar.gameObject.transform.Find("SearchInputField").gameObject;
        clearButton = actionBar.gameObject.transform.Find("ClearSearchButton").gameObject;

        /* search */
        searchHelpText = searchPanel.transform.Find("HelpText").gameObject;
        searchList = searchPanel.transform.Find("Scroll View").gameObject;
        viewPort = searchList.gameObject.transform.Find("Viewport").gameObject;
        scrollbar = searchList.gameObject.transform.Find("Scrollbar Vertical").gameObject;
        searchContent = viewPort.gameObject.transform.Find("Content").gameObject;

        /* map */
        mapImage = mapPanel.transform.Find("MapImage").gameObject;
        rightButton = mapPanel.transform.Find("RightButton").gameObject;
        leftButton = mapPanel.transform.Find("LeftButton").gameObject;
        navline = mapImage.transform.Find("Line").gameObject;
        userDot = mapImage.transform.Find("UserDot").gameObject;

        backButton.SetActive(false);
        searchInputField.SetActive(false);
        clearButton.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            switch (page)
            {
                case Page.Search: OnCloseSerch(); break;
                case Page.Map: OnCloseMap(); break;
                case Page.Main:
                    if (canQuitApp)
                    {
                        Application.Quit();
                    }
                    else
                    {
                        canQuitApp = true;
                        //showToast
                    }
                    break;
                default: OnCloseSerch(); break;
            }
        }
    }

    public void OnBackButton()
    {
        switch (page)
        {
            case Page.Search: OnCloseSerch(); break;
            case Page.Map: OnCloseMap(); break;
            default: OnCloseSerch(); break;
        }
    }

    #region Open New Page

    public void OnOpenSerch()
    {
        page = Page.Search;
        mapPanel.SetActive(false);
        searchPanel.SetActive(true);
        hambergerButton.SetActive(false);
        mapButton.SetActive(false);
        searchButton.SetActive(false);
        appName.SetActive(false);

        backButton.SetActive(true);
        searchInputField.SetActive(true);
        clearButton.SetActive(true);

        canvasResolutionScript.SetBackButtonInSearch();
        canvasResolutionScript.SetClearButtonInSearch();
        canvasResolutionScript.SetSearchFieldInSearch();

        canvasResolutionScript.SetHelpTextInSearch();
        canvasResolutionScript.SetScrollListInSearch();
        canvasResolutionScript.SetContentInSearch();
        OnTyping();
    }

    public void OnCloseSerch()
    {
        page = Page.Main;
        mapPanel.SetActive(false);
        searchPanel.SetActive(false);
        hambergerButton.SetActive(false);
        mapButton.SetActive(true);
        searchButton.SetActive(true);
        appName.SetActive(true);

        backButton.SetActive(false);
        searchInputField.SetActive(false);
        clearButton.SetActive(false);

        //canvasResolutionScript.SetHambergerButtonInMain();
        canvasResolutionScript.SetMapButtonInMain();
        canvasResolutionScript.SetSearchButtonInMain();
        canvasResolutionScript.SetAppNameInMain();
    }

    public void OnOpenMap()
    {
        page = Page.Map;
        mapPanel.SetActive(true);
        searchPanel.SetActive(false);
        hambergerButton.SetActive(false);
        mapButton.SetActive(false);
        searchButton.SetActive(false);
        appName.SetActive(true);

        backButton.SetActive(true);
        searchInputField.SetActive(false);
        clearButton.SetActive(false);

        canvasResolutionScript.SetAppNameInMap();
        canvasResolutionScript.SetAppNameInMap();

        canvasResolutionScript.SetMapImageInMap();
        canvasResolutionScript.SetArrowButtonInMap();
        if (MainController.instance.beginPoint != null)
        {
            showingFloor = MainController.instance.beginPoint.GetComponent<MarkerData>().GetFloor();
        }
        UpdateMap(showingFloor);
    }

    public void OnCloseMap()
    {
        page = Page.Main;
        mapPanel.SetActive(false);
        searchPanel.SetActive(false);
        hambergerButton.SetActive(false);
        mapButton.SetActive(true);
        searchButton.SetActive(true);
        appName.SetActive(true);

        backButton.SetActive(false);
        searchInputField.SetActive(false);
        clearButton.SetActive(false);

        //canvasResolutionScript.SetHambergerButtonInMain();
        canvasResolutionScript.SetMapButtonInMain();
        canvasResolutionScript.SetSearchButtonInMain();
        canvasResolutionScript.SetAppNameInMain();
    }
    #endregion

    #region State From Main

    // may move these public function

    //recive from MainController, may be implement from callback
    public void OnAppStateChange()
    {
        ChangeActionBarColor();
    }
    public void OnBeginPointChange(GameObject bpoint)
    {
        GameObject dpoint = MainController.instance.destinationPoint;
        if (MainController.instance.appState == MainController.AppState.Idle)
        {
            ChangeActionText("อยู่ที่: " + bpoint.GetComponent<MarkerData>().roomName);
            toastMessageScript.showToastOnUiThread(
                    UIValue.value.STRING_CURRENT_POSITION + bpoint.GetComponent<MarkerData>().roomName);
            if (MainController.instance.reachedPoint != null)
            {
                if (bpoint.GetComponent<MarkerData>().roomName ==
                    MainController.instance.reachedPoint.GetComponent<MarkerData>().roomName)
                {
                    ChangeActionText("ถึงแล้ว: " + bpoint.GetComponent<MarkerData>().roomName);
                    toastMessageScript.showToastOnUiThread(
                        UIValue.value.STRING_REACH_DESTINATION + bpoint.GetComponent<MarkerData>().roomName + "แล้ว");
                    Debug.Log(" Reach at Main ShowAR Idle Mode and idle");
                }
            }
            else if (dpoint == null)
            {
                ChangeActionText("AR Indoor Navigation System");
                toastMessageScript.showToastOnUiThread(UIValue.value.STRING_SELECTDEST);
            }
        }
        else if (MainController.instance.appState == MainController.AppState.Navigate)
        {
            if (bpoint.GetComponent<MarkerData>().roomName == dpoint.GetComponent<MarkerData>().roomName)
            {
                ChangeActionText("ถึงแล้ว: " + bpoint.GetComponent<MarkerData>().roomName);
                toastMessageScript.showToastOnUiThread(
                    UIValue.value.STRING_REACH_DESTINATION + bpoint.GetComponent<MarkerData>().roomName + "แล้ว");
                Debug.Log(" Reach at Main ShowAR Idle Mode and idle");
            }
            else
            {
                ChangeActionText("ไปยัง: " + dpoint.GetComponent<MarkerData>().roomName);
                toastMessageScript.showToastOnUiThread(
                        UIValue.value.STRING_CURRENT_POSITION + bpoint.GetComponent<MarkerData>().roomName);
            }

        }
    }
    public void OnDestinationPointChange(GameObject dpoint)
    {
        GameObject bpoint = MainController.instance.beginPoint;
        GameObject rpoint = MainController.instance.reachedPoint;
        if (MainController.instance.appState == MainController.AppState.Idle)
        {
            //dpoint change to null in idle
            if (bpoint == null)
            {
                ChangeActionText("ต้องการไปยัง: " + dpoint.GetComponent<MarkerData>().roomName);
                toastMessageScript.showToastOnUiThread(UIValue.value.STRING_FINDMARKER);
            }
            if (bpoint != null && dpoint != null) //
            {
                if (bpoint.GetComponent<MarkerData>().roomName == dpoint.GetComponent<MarkerData>().roomName)
                {
                    ChangeActionText("ถึงแล้ว: " + bpoint.GetComponent<MarkerData>().roomName);
                    toastMessageScript.showToastOnUiThread(
                        UIValue.value.STRING_REACH_DESTINATION + bpoint.GetComponent<MarkerData>().roomName + "แล้ว");
                    Debug.Log(" Reach at Main ShowAR Idle Mode and idle");
                }
                else
                {
                    ChangeActionText("ไปยัง: " + dpoint.GetComponent<MarkerData>().roomName);
                    toastMessageScript.showToastOnUiThread(
                            UIValue.value.STRING_CHANGE_DESTINATION + dpoint.GetComponent<MarkerData>().roomName + "แล้ว");
                }
            }
            else
            {
                ChangeActionText("ไปยัง: " + dpoint.GetComponent<MarkerData>().roomName);
                toastMessageScript.showToastOnUiThread(
                        UIValue.value.STRING_CHANGE_DESTINATION + dpoint.GetComponent<MarkerData>().roomName + " แล้ว "
                        + UIValue.value.STRING_TOCANCLE_NAVIGATE);
            }
        }
        else if (MainController.instance.appState == MainController.AppState.Navigate)
        {
            if (dpoint == null)
            {
                ChangeActionText("อยู่ที่: " + bpoint.GetComponent<MarkerData>().roomName);
                toastMessageScript.showToastOnUiThread(UIValue.value.STRING_REVOKE_NAVIGATE);
            }
            else if (bpoint.GetComponent<MarkerData>().roomName == dpoint.GetComponent<MarkerData>().roomName)
            {
                ChangeActionText("ถึงแล้ว: " + bpoint.GetComponent<MarkerData>().roomName);
                toastMessageScript.showToastOnUiThread(
                    UIValue.value.STRING_REACH_DESTINATION + bpoint.GetComponent<MarkerData>().roomName + "แล้ว");
                Debug.Log(" Reach at Main ShowAR Idle Mode and idle");
            }
            else
            {
                ChangeActionText("ไปยัง: " + dpoint.GetComponent<MarkerData>().roomName);
                toastMessageScript.showToastOnUiThread(
                        UIValue.value.STRING_CHANGE_DESTINATION + dpoint.GetComponent<MarkerData>().roomName);
            }
        }
    }
    //used by above
    public void ChangeActionBarColor() //OnAppStateChange
    {
        switch (MainController.instance.appState)
        {
            case MainController.AppState.Idle:
                actionBar.GetComponent<Image>().color = new Color32(60, 126, 255, 255);
                break;
            case MainController.AppState.Navigate:
                actionBar.GetComponent<Image>().color = new Color32(126, 60, 255, 255);
                break;
            default: actionBar.GetComponent<Image>().color = new Color32(60, 126, 255, 255); break;
        }
    }

    public void ChangeActionText(string actext)
    {
        appName.GetComponent<Text>().text = actext;
    }

    #endregion


    #region Search Action

    public void OnTyping()
    {
        string typingWord = searchInputField.GetComponent<InputField>().text;
        searchShowList.Clear();
        foreach (GameObject floor in building.floorList)
        {
            foreach (GameObject marker in floor.GetComponent<FloorData>().markerList)
            {
                MarkerData markerData = marker.GetComponent<MarkerData>();
                if (typingWord == "" && !IsDuplicateShowingRoom(searchShowList, markerData.roomName))
                {
                    Debug.Log(typingWord + " In " + markerData.roomName);
                    searchShowList.Add(marker);
                }
                else if (markerData.roomName.Contains(typingWord) && !IsDuplicateShowingRoom(searchShowList, markerData.roomName))
                {
                    Debug.Log(typingWord + " In " + markerData.roomName);
                    searchShowList.Add(marker);
                }
            }
        }
        ShowAllRoomOf(searchShowList);
    }

    // detect too much loop here
    private bool IsDuplicateShowingRoom(List<GameObject> searchLst, string findingMarkerName) /* false if already has marker of that room */
    {
        foreach (GameObject mk in searchLst)
        {
            if (mk.GetComponent<MarkerData>().roomName == findingMarkerName)
            {
                return true;
            }
        }
        return false;
    }

    private void ShowAllRoomOf(List<GameObject> searchMarkerList)
    {
        //if system have begin point, need to color it, set to false
        bool beginColored = !(MainController.instance.beginPoint != null);
        bool destColored = !(MainController.instance.destinationPoint != null);
        //destroy all list
        foreach (Transform ch in searchContent.transform)
        {
            Destroy(ch.gameObject);
        }
        foreach (GameObject markerob in searchMarkerList)
        {
            GameObject roomButton = Instantiate(roomButtonPrefab);
            roomButton.GetComponent<RoomButtonScript>().room = markerob;
            MarkerData markerdata = markerob.GetComponent<MarkerData>();
            roomButton.transform.SetParent(searchContent.transform);
            Text roomButtonText = roomButton.transform.GetChild(0).gameObject.GetComponent<Text>();
            roomButtonText.text = markerdata.roomName;
            roomButtonText.fontSize = canvasResolutionScript.GetScaledFontSize(45);
            if (!beginColored)
            {
                if (MainController.instance.beginPoint.GetComponent<MarkerData>().roomName == markerdata.roomName)
                {
                    roomButtonText.text = "ต้นทาง: " + markerdata.roomName;
                    roomButtonText.fontStyle = FontStyle.Bold;
                    roomButtonText.fontSize = canvasResolutionScript.GetScaledFontSize(47); // gray
                    beginColored = true;
                }
            }
            if (!destColored)
            {
                if (MainController.instance.destinationPoint.GetComponent<MarkerData>().roomName == markerdata.roomName)
                {
                    roomButton.GetComponent<RoomButtonScript>().isDestination = true;
                    roomButton.GetComponent<Image>().color = new Color32(126, 60, 255, 255); // purple
                    roomButtonText.text = "ปลายทาง: " + markerdata.roomName;
                    destColored = true;
                }
            }
        }
    }
    #endregion

    #region Map Action

    public void OnShiftMap(bool isForward)
    {
        BuildingData building = showingFloor.GetComponent<FloorData>().GetBuilding().GetComponent<BuildingData>();

        //get next floor from buildingData
        GameObject floorObject = isForward ?
            building.GetNextFloor(showingFloor.GetComponent<FloorData>().floorName) :
            building.GetPreviousFloor(showingFloor.GetComponent<FloorData>().floorName);

        UpdateMap(floorObject);
    }

    private void UpdateMap(GameObject floorObject) /* Update current floor of map page */
    {
        BuildingData building = floorObject.GetComponent<FloorData>().GetBuilding().GetComponent<BuildingData>();
        // get material from first child of floorData 
        Material floorMaterial = floorObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().materials[0];
        mapImage.GetComponent<Image>().material = floorMaterial;
        ShowMarkerOfFloor(floorObject);

        //check floor, start stop of that floor for line
        if (MainController.instance.appState == MainController.AppState.Navigate
            && MainController.instance.destinationPoint != null)
        {
            GameObject beginFloor = MainController.instance.beginPoint.GetComponent<MarkerData>().GetFloor();
            GameObject destinationFloor = MainController.instance.destinationPoint.GetComponent<MarkerData>().GetFloor();
            if (building.IsSameFloor(MainController.instance.beginPoint, MainController.instance.destinationPoint) //loking fl in same
                && beginFloor == floorObject)
            {
                ShowLine(MainController.instance.beginPoint, MainController.instance.destinationPoint);
                Debug.Log(" Show Line In Same Floor");
            }
            else
            {
                if (beginFloor == floorObject)
                { //swap to begin fl
                    ShowLine(MainController.instance.beginPoint, building.GetConnector(MainController.instance.beginPoint));
                }
                else if (destinationFloor == floorObject)
                { //swap in dest fl
                    ShowLine(building.GetConnector(MainController.instance.destinationPoint), MainController.instance.destinationPoint);
                }
                else
                {
                    //check is looking floor are inbeetween 
                    //if yes green dot in lift
                    if (beginFloor.GetComponent<FloorData>().floorIndex < destinationFloor.GetComponent<FloorData>().floorIndex
                        && floorObject.GetComponent<FloorData>().floorIndex < destinationFloor.GetComponent<FloorData>().floorIndex
                        && floorObject.GetComponent<FloorData>().floorIndex > beginFloor.GetComponent<FloorData>().floorIndex)
                    {
                        ShowLine(floorObject.GetComponent<FloorData>().connectorList[0]);
                    }
                    else if (beginFloor.GetComponent<FloorData>().floorIndex > destinationFloor.GetComponent<FloorData>().floorIndex
                      && floorObject.GetComponent<FloorData>().floorIndex > destinationFloor.GetComponent<FloorData>().floorIndex
                      && floorObject.GetComponent<FloorData>().floorIndex < beginFloor.GetComponent<FloorData>().floorIndex)
                    {
                        ShowLine(floorObject.GetComponent<FloorData>().connectorList[0]);
                    }
                    else
                    {
                        ClearLine();
                    }
                    //if no, will not show line in 
                }
            }
        }
        else if (MainController.instance.appState == MainController.AppState.Idle)
        {
            ClearLine();
        }
        //check floor and current position for user dot
        userDot.SetActive(false);
        if (MainController.instance.beginPoint != null)
        {
            if (MainController.instance.beginPoint.GetComponent<MarkerData>().GetFloor() == floorObject)
            {
                ShowUserDot(MainController.instance.beginPoint);
            }
        }

        showingFloor = floorObject;
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
