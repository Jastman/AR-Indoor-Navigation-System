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
		canvasResolutionScript = gameObject.GetComponent<CanvasResolutionScript>();
		hambergerButton = actionBar.gameObject.transform.Find("HambergerButton").gameObject;
		mapButton = actionBar.gameObject.transform.Find("MapButton").gameObject;		
		searchButton = actionBar.gameObject.transform.Find("SearchButton").gameObject;
		appName = actionBar.gameObject.transform.Find("AppName").gameObject;
		appNameText = appName.GetComponent<Text>();

		backButton = actionBar.gameObject.transform.Find("BackButton").gameObject;
		searchInputField = actionBar.gameObject.transform.Find("SearchInputField").gameObject;
		clearButton = actionBar.gameObject.transform.Find("ClearSearchButton").gameObject;
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

		canvasResolutionScript.SetSeHelpTextInSearch();
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
}
