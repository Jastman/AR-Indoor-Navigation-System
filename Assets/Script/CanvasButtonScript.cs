using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasButtonScript : MonoBehaviour {

	public GameObject actionBar;

	public GameObject searchPanel; //panel
	private GameObject hambergerButton, mapButton, searchButton, backButton, clearButton; //button
	private GameObject searchInputField, appName; //InputFields + text
	private Text appNameText;

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
		clearButton = searchInputField.transform.Find("ClearSearchButton").gameObject;
		backButton.SetActive(false);
		searchInputField.SetActive(false);
		clearButton.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnOpenSerch()
	{
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
