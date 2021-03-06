﻿using System.Collections;
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

    private GameObject searchHelpText, searchList, viewPort, scrollbar, searchContent, roomDataPanel, roomDataDialog;
    private GameObject roomNameTitle, roomMapImage, roomDesData, roomNavigateButton;
    private GameObject mapImage, rightButton, leftButton, navline, userDot;

    
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
    private MapControlScript mapControl;
    private BuildingData building;
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
        toastMessageScript.showToastOnUiThread("ส่องกล้องไปยังจุดต่างๆ เช่น ป้ายบอกทาง เลขห้อง เพื่อเริ่มต้นระบุตำแหน่งของคุณ", false);

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
        roomDataPanel = searchPanel.transform.Find("RoomDataPanel").gameObject;
        roomDataDialog = roomDataPanel.transform.Find("RoomDataDialog").gameObject;
        roomNameTitle = roomDataDialog.transform.Find("RoomNameTitle").gameObject;
        roomMapImage = roomDataDialog.transform.Find("RoomMapImage").gameObject;
        roomDesData = roomDataDialog.transform.Find("RoomData").gameObject;
        roomNavigateButton = roomDataDialog.transform.Find("NavigateButton").gameObject;

        /* map */
        mapImage = mapPanel.transform.Find("MapScrollViewArea").gameObject;
        rightButton = mapPanel.transform.Find("RightButton").gameObject;
        leftButton = mapPanel.transform.Find("LeftButton").gameObject;
        mapControl = mapImage.transform.Find("Mask/MapImage").gameObject.GetComponent<MapControlScript>();
        // navline = mapImage.transform.Find("Line").gameObject;
        // userDot = mapImage.transform.Find("UserDot").gameObject;

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

    public void DummyOnclickFunction()
    {
        Debug.Log("Dummy");
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
        roomDataPanel.SetActive(false);

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
        roomDataPanel.SetActive(false);

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
        roomDataPanel.SetActive(false);

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
        mapControl.UpdateMap(showingFloor);
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
        roomDataPanel.SetActive(false);

        backButton.SetActive(false);
        searchInputField.SetActive(false);
        clearButton.SetActive(false);

        //canvasResolutionScript.SetHambergerButtonInMain();
        canvasResolutionScript.SetMapButtonInMain();
        canvasResolutionScript.SetSearchButtonInMain();
        canvasResolutionScript.SetAppNameInMain();
    }

    public void OnOpenRoomDialoge(GameObject roomObj, bool isDestination)
    /* show room dialoge box data with map */
    {
        MarkerData mkdt = roomObj.GetComponent<MarkerData>();
        roomDataPanel.SetActive(true);
        roomNameTitle.GetComponent<Text>().text = "ข้อมูลห้อง: " + mkdt.roomName;
        Material floorMaterial = mkdt.GetFloor().transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().materials[0];
        roomMapImage.GetComponent<Image>().material = floorMaterial;
        roomDesData.GetComponent<Text>().text = mkdt.description;
        if (!isDestination)
        {
            roomNavigateButton.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Navigate.";
            roomNavigateButton.GetComponent<Button>().onClick.RemoveAllListeners();
            roomNavigateButton.GetComponent<Button>().onClick.AddListener(delegate { SelectToNavigate(roomObj); });
            if(MainController.instance.beginPoint != null)
            {
                if(mkdt.roomName == MainController.instance.beginPoint.GetComponent<MarkerData>().roomName)
                {
                    roomNavigateButton.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Here is Current Room";
                    roomNavigateButton.GetComponent<Button>().onClick.RemoveAllListeners();
                    roomNavigateButton.GetComponent<Button>().onClick.AddListener(delegate { 
                        toastMessageScript.showToastOnUiThread("จุดล่าสุดของคุณคือจุดที่คุณเลือก กรุณาเลือกปลายทางที่อื่น", true);
                        });
                }
            }
        }
        else
        {
            roomNavigateButton.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Cancle Navigate";
            roomNavigateButton.GetComponent<Button>().onClick.RemoveAllListeners();
            roomNavigateButton.GetComponent<Button>().onClick.AddListener(ClearPoint);
        }

        //mark point and navigate draw line to destination
        GameObject destDot = roomMapImage.transform.Find("MarkDot/DestMarker").gameObject;
        destDot.GetComponent<RectTransform>().anchoredPosition = new Vector2(
            mkdt.position.x * (roomMapImage.GetComponent<RectTransform>().sizeDelta.x / 1000),
            mkdt.position.z * (roomMapImage.GetComponent<RectTransform>().sizeDelta.y / 1000)
        );

    }

    public void OnCloseRoomDialog()
    {
        roomDataPanel.SetActive(false);
    }

    private void SelectToNavigate(GameObject roomObj)
    {
        MainController.instance.SetDestinationPoint(roomObj);
        OnCloseRoomDialog();
        OnCloseSerch();
    }

    private void ClearPoint()
    {
        MainController.instance.ClearDestinationPoint();
        OnCloseRoomDialog();
        OnCloseSerch();
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
            roomButtonText.fontSize = canvasResolutionScript.GetScaledFontSize(40);
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
        //BuildingData building = showingFloor.GetComponent<FloorData>().GetBuilding().GetComponent<BuildingData>();

        //get next floor from buildingData
        GameObject floorObject = isForward ?
            building.GetNextFloor(showingFloor.GetComponent<FloorData>().floorName) :
            building.GetPreviousFloor(showingFloor.GetComponent<FloorData>().floorName);
        mapControl.UpdateMap(floorObject);
        showingFloor = floorObject;
    }

    #endregion

}
