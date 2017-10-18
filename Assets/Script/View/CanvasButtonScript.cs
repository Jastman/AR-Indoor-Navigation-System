using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasButtonScript : MonoBehaviour {

	public GameObject markerPrefab, roomButtonPrefab;
	public GameObject actionBar;
	public GameObject searchPanel, mapPanel; //panel
	private GameObject hambergerButton, mapButton, searchButton, backButton, clearButton; //button
	private GameObject searchInputField, appName; //InputFields + text
	private Text appNameText;

	private GameObject searchHelpText, searchList, viewPort, scrollbar, searchContent;
	private GameObject mapImage, rightButton, leftButton;

	private BuidingData building;
	private FloorData showingFloor;
	private List<GameObject> searchShowList;


	private enum Page
	{
		Main,
		Search,
		Map
	}
	private Page page = Page.Main;

	private CanvasResolutionScript canvasResolutionScript;

	// Use this for initialization
	void Start () {
		building = GameObject.Find("IT Buiding").GetComponent<BuidingData>();
		showingFloor = building.floorList[0].GetComponent<FloorData>();
		searchShowList = new List<GameObject>();

		canvasResolutionScript = gameObject.GetComponent<CanvasResolutionScript>();
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

		backButton.SetActive(false);
		searchInputField.SetActive(false);
		clearButton.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
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
		hambergerButton.SetActive(true);
		mapButton.SetActive(true);
		searchButton.SetActive(true);
		appName.SetActive(true);

		backButton.SetActive(false);
		searchInputField.SetActive(false);
		clearButton.SetActive(false);

		canvasResolutionScript.SetHambergerButtonInMain();
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
	}

	public void OnCloseMap()
	{
		page = Page.Main;
		mapPanel.SetActive(false);
		searchPanel.SetActive(false);
		hambergerButton.SetActive(true);
		mapButton.SetActive(true);
		searchButton.SetActive(true);
		appName.SetActive(true);

		backButton.SetActive(false);
		searchInputField.SetActive(false);
		clearButton.SetActive(false);

		canvasResolutionScript.SetHambergerButtonInMain();
		canvasResolutionScript.SetMapButtonInMain();
		canvasResolutionScript.SetSearchButtonInMain();
		canvasResolutionScript.SetAppNameInMain();
	}

	public void OnTyping()
	{
		string typingWord = searchInputField.GetComponent<InputField>().text;
		searchShowList.Clear();
		foreach (GameObject floor in building.floorList)
		{
			foreach (GameObject marker in floor.GetComponent<FloorData>().markerList)
			{
				MarkerData markerData = marker.GetComponent<MarkerData>();
				if(typingWord == "") {
					Debug.Log(typingWord +" In " + markerData.roomName);
					searchShowList.Add(marker);	
				} else if (markerData.roomName.Contains(typingWord)) {
					Debug.Log(typingWord +" In " + markerData.roomName);
					searchShowList.Add(marker);
				}
			}
		}
		ShowAllRoomOf(searchShowList);
	}

	private void ShowAllRoomOf(List<GameObject> searchMarkerList)
	{
		//destroy all list
		Debug.Log(searchContent.transform.childCount);
		foreach (Transform ch in searchContent.transform)
		{
			Debug.Log("Destroy" + ch);
			Destroy(ch.gameObject);
		}
		foreach (GameObject markerob in searchMarkerList)
		{
			GameObject roomButton = Instantiate(roomButtonPrefab);
			MarkerData markerdata = markerob.GetComponent<MarkerData>();
			roomButton.transform.SetParent(searchContent.transform);
			Text roomButtonText = roomButton.transform.GetChild(0).gameObject.GetComponent<Text>();
			roomButtonText.text = markerdata.roomName;
			roomButtonText.fontSize = canvasResolutionScript.GetScaledFontSize(45);
		}
	}

	public void OnShiftMap(bool isForward)
	{
		GameObject floorObject;
		//check current floor
		if (MainStaticData.floor != null) {
			showingFloor = MainStaticData.floor.GetComponent<FloorData>();
		}

		//get next floor from buildingData
		floorObject = isForward ? building.GetNextFloor(showingFloor.floorName) : building.GetPreviousFloor(showingFloor.floorName);

		// get material from first child of floorData 
		Material floorMaterial = floorObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().materials[0];
		mapImage.GetComponent<Image>().material = floorMaterial;
		ShowMarkerOfFloor(floorObject);
		showingFloor = floorObject.GetComponent<FloorData>();
	}

	public void ShowMarkerOfFloor(GameObject floorObject)
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
				markerdata.position.x * (mapImage.GetComponent<RectTransform>().sizeDelta.x/1000),
				markerdata.position.z * (mapImage.GetComponent<RectTransform>().sizeDelta.y/1000)
			);
		}
		
		
	}
}
