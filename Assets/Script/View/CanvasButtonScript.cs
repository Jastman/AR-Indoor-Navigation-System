using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasButtonScript : MonoBehaviour {

	public GameObject actionBar;

	public GameObject searchPanel, mapPanel; //panel
	private GameObject hambergerButton, mapButton, searchButton, backButton, clearButton; //button
	private GameObject searchInputField, appName; //InputFields + text
	private Text appNameText;

	private GameObject searchHelpText, searchList;
	private GameObject mapImage, rightButton, leftButton;

	private FloorData showingFloor;
	private BuidingData building;

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

	public void OnShiftMap(bool isForward)
	{
		GameObject floorObject;
		//check current floor
		if (MainStaticData.floor != null) {
			showingFloor = MainStaticData.floor.GetComponent<FloorData>();
		}

		//get next floor from buildingData
		floorObject = isForward ? building.GetNextFloor(showingFloor.floorName) : building.GetNextFloor(showingFloor.floorName);

		// get material from first child of floorData 
		Material floorMaterial = floorObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().materials[0];
		mapImage.GetComponent<Image>().material = floorMaterial;
		showingFloor = floorObject.GetComponent<FloorData>();
	}

	public void ShowMarkerOfFloor(GameObject Floor)
	{
		
	}
}
