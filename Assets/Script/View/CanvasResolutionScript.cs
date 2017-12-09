using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasResolutionScript : MonoBehaviour {
	
	private const float DEFAULT_DPI = 160.0f;

	public enum ResolutionType 
     {
         ldpi,
         mdpi,
         hdpi,
         xhdpi
     }

	public float actionBarHeight;

	public GameObject actionBar, searchPanel, mapPanel;
	public Text debugText;
	public Image actionBarImage;
	private RectTransform actionBarRect, searchInputField, appName;
	private RectTransform mapButton, searchButton, hambergerButton, backButton, clearButton;

	private RectTransform searchHelpText, searchList, viewPort, scrollbar;
	private GridLayoutGroup searchContent;
	private RectTransform mapImage, rightButton, leftButton, navline, userDot;

	private List<GameObject> markerList; //not use now
	

	// Use this for initialization
	void Start () {
		Screen.fullScreen = false;
		actionBarHeight = DpToPixel(56.0f);

		actionBarRect = actionBarImage.gameObject.GetComponent<RectTransform>();
		actionBarRect.sizeDelta = new Vector2(Screen.width, actionBarHeight);
		actionBarRect.anchoredPosition = new Vector2(0, actionBarHeight/-2) ;
		
		mapButton = actionBarImage.gameObject.transform.Find("MapButton").GetComponent<RectTransform>();
		SetMapButtonInMain();

		searchButton = actionBarImage.gameObject.transform.Find("SearchButton").GetComponent<RectTransform>();
		SetSearchButtonInMain();
		
		hambergerButton = actionBarImage.gameObject.transform.Find("HambergerButton").GetComponent<RectTransform>();
		SetHambergerButtonInMain();

		appName = actionBarImage.gameObject.transform.Find("AppName").GetComponent<RectTransform>();
		SetAppNameInMain();

		backButton = actionBarImage.gameObject.transform.Find("BackButton").GetComponent<RectTransform>();
		searchInputField = actionBarImage.gameObject.transform.Find("SearchInputField").GetComponent<RectTransform>();
		clearButton = actionBarImage.gameObject.transform.Find("ClearSearchButton").GetComponent<RectTransform>();

		/* search */
		searchHelpText = searchPanel.transform.Find("HelpText").GetComponent<RectTransform>();
		searchList = searchPanel.transform.Find("Scroll View").GetComponent<RectTransform>();
		viewPort = searchList.gameObject.transform.Find("Viewport").GetComponent<RectTransform>();
		scrollbar = searchList.gameObject.transform.Find("Scrollbar Vertical").GetComponent<RectTransform>();
		searchContent = viewPort.gameObject.transform.Find("Content").gameObject.GetComponent<GridLayoutGroup>();

		/* map */
		mapImage = mapPanel.transform.Find("MapScrollViewArea").GetComponent<RectTransform>();
		rightButton = mapPanel.transform.Find("RightButton").GetComponent<RectTransform>();
		leftButton = mapPanel.transform.Find("LeftButton").GetComponent<RectTransform>();
		// navline = mapImage.gameObject.transform.Find("Line").GetComponent<RectTransform>();
		// userDot = mapImage.gameObject.transform.Find("UserDot").GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update () {
		//zoom map?
		debugText.text = string.Format("DPI:{0} Scale:{1} PixelToDP:{2}", Screen.dpi, GetScale(), PixelToDp(100));
	}

	/* set position and size of button */
	#region Set Main UI
	public void SetMapButtonInMain()
	{
		mapButton.sizeDelta = new Vector2(topButtonSize(), topButtonSize());
		mapButton.anchoredPosition = new Vector2((topButtonPosition()*-1), topButtonPosition()*-1);
	}
	public void SetSearchButtonInMain()
	{
		searchButton.sizeDelta = new Vector2(topButtonSize(), topButtonSize());
		searchButton.anchoredPosition = new Vector2(topButtonPosition()*-1 - topButtonSize() - topButtonPadding(), topButtonPosition()*-1);
	}
	public void SetHambergerButtonInMain()
	{
		hambergerButton.sizeDelta = new Vector2(topButtonSize(), topButtonSize());
		hambergerButton.anchoredPosition = new Vector2(topButtonPosition(), topButtonPosition()*-1);
	}

	public void SetAppNameInMain()
	{
		appName.sizeDelta = new Vector2(Screen.width - (actionBarHeight*3), topButtonSize());
		appName.anchoredPosition = new Vector2(appName.sizeDelta.x/2 + actionBarHeight, 0);
	}
	#endregion

	#region Set Search UI
	public void SetBackButtonInSearch()
	{
		backButton.sizeDelta = new Vector2(topButtonSize(), topButtonSize());
		searchButton.anchoredPosition = new Vector2((topButtonPosition()*-1), topButtonPosition()*-1);
	}
	public void SetClearButtonInSearch()
	{
		float clearButtonPadding = (actionBarHeight*0.15f);
		clearButton.sizeDelta = new Vector2(actionBarHeight*0.3f, actionBarHeight*0.3f);
		clearButton.anchoredPosition = new Vector2(((clearButton.sizeDelta.x/2) + clearButtonPadding)*-1 , 0);
	}
	public void SetSearchFieldInSearch()
	{
		searchInputField.sizeDelta = new Vector2(Screen.width - actionBarHeight, topButtonSize());
		searchInputField.anchoredPosition = new Vector2(searchInputField.sizeDelta.x/2 + actionBarHeight, 0);
		foreach (Text tx in GetComponentsInChildren<Text>())
		{
			tx.fontSize = GetScaledFontSize(48);
		}
	}
	public void SetHelpTextInSearch()
	{
		float helpTextPadding = 20f;
		searchHelpText.sizeDelta = new Vector2(Screen.width - helpTextPadding, 50); //<< 50
		searchHelpText.anchoredPosition = new Vector2(0, -1*(actionBarHeight + (helpTextPadding/2f) + (searchHelpText.sizeDelta.y/2f)));
		searchHelpText.gameObject.GetComponent<Text>().fontSize = GetScaledFontSize(36);
	}
	public void SetScrollListInSearch()
	{
		float scrollbarPadding = 20f;
		Debug.Log("scrollbar set");
		searchList.sizeDelta = new Vector2(
			Screen.width, 
			Screen.height - actionBarHeight - searchHelpText.sizeDelta.y - scrollbarPadding
			);
		searchList.anchoredPosition = new Vector2(0, -1f*(searchList.sizeDelta.y/2 + actionBarHeight 
			+ searchHelpText.sizeDelta.y + (scrollbarPadding/2)));
	}
	public void SetContentInSearch()
	{
		float spacingofcellsize = 0.1f;
		searchContent.cellSize = new Vector2(searchList.sizeDelta.x - spacingofcellsize, actionBarHeight*0.9f);
		searchContent.spacing = new Vector2(searchContent.cellSize.y*spacingofcellsize, searchContent.cellSize.y*spacingofcellsize);
	}

	#endregion

	#region Set Map UI
	public void SetBackButtonInMap()
	{
		backButton.sizeDelta = new Vector2(topButtonSize(), topButtonSize());
		searchButton.anchoredPosition = new Vector2((topButtonPosition()*-1), topButtonPosition()*-1);
	}
	public void SetAppNameInMap()
	{
		appName.sizeDelta = new Vector2(Screen.width - (actionBarHeight*3), topButtonSize());
		appName.anchoredPosition = new Vector2(appName.sizeDelta.x/2 + actionBarHeight, 0);
	}
	public void SetMapImageInMap()
	{
		float mapPadding = 30f;
		mapImage.sizeDelta = new Vector2(Screen.width-mapPadding, Screen.width-mapPadding);
		mapImage.anchoredPosition = new Vector2(0, actionBarHeight/1.3f);
		// navline.sizeDelta = mapImage.sizeDelta;
		// navline.anchoredPosition = Vector2.zero;
	}
	public void SetArrowButtonInMap()
	{
		rightButton.sizeDelta = new Vector2(actionBarHeight*1.3f, actionBarHeight*1.3f);
		rightButton.anchoredPosition = new Vector2(Screen.width/3.5f, Screen.height/-2.5f);
		leftButton.sizeDelta = new Vector2(actionBarHeight*1.3f, actionBarHeight*1.3f);
		leftButton.anchoredPosition = new Vector2(Screen.width/-3.5f, Screen.height/-2.5f);
	}
	#endregion

	#region MapMarker
	private void GetMarker()
	{
		//if no one active, instancetiate one
	}
	#endregion
	

	#region Size Calculation 
	/* Button size Calculation */
	private float topButtonSize()
	{
		
		return actionBarHeight*(0.8f);
	}

	private float topButtonPadding()
	{
		return ((actionBarHeight - topButtonSize()) / 2);
	}

	private float topButtonPosition()
	{
		return ((topButtonSize() / 2) + topButtonPadding());
	}



	/* Pixel Resolution calculation */
	public static float DpToPixel(float dp)
     {
         // Convert the dps to pixels
         //return (int) (dp * GetScale() + 0.5f); 
		 return dp * (GetDPI() / DEFAULT_DPI);
     }

	public static float PixelToDp(float px)
     {
         // Convert the pxs to dps
         return (int) (px / GetScale() - 0.5f);
     }

	 private static float GetScale()
     {
         return GetDPI() / DEFAULT_DPI;
     }

	 private static float GetDPI()
     {
         return Screen.dpi == 0 ? DEFAULT_DPI : Screen.dpi;
		 //return DEFAULT_DPI;
     }

	 public static ResolutionType GetResolutionType()
     {
         float scale = GetDPI() / DEFAULT_DPI;
         
         ResolutionType res;
         
         //http://developer.android.com/guide/practices/screens_support.html
         if(scale > 1.5f)
         {
             res = CanvasResolutionScript.ResolutionType.xhdpi;
         }
         else if(scale > 1f)
         {
             res = CanvasResolutionScript.ResolutionType.hdpi;
         }
         else if(scale > 0.75f)
         {
             res = CanvasResolutionScript.ResolutionType.mdpi;
         }
         else
         {
             res = CanvasResolutionScript.ResolutionType.ldpi;
         }
         
         return res;
     }

	public int GetScaledFontSize (int baseFontSize) {
		int uiBaseScreenHeight = 720;
		float uiScale = Screen.height / uiBaseScreenHeight;
		int scaledFontSize = Mathf.RoundToInt(baseFontSize * uiScale);
		return scaledFontSize;
	}


	 /* Unuse grapvyard: get dp value return in pixel */
	private float DPinPixel(int dp)
	{
		return dp * (Screen.dpi/160f);
	}
	#endregion
}
