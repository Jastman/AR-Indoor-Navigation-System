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

	private float actionBarHeight;

	public GameObject actionBar, searchPanel, mapPanel;
	public Text debugText;
	public Image actionBarImage;
	private RectTransform actionBarRect, searchInputField, appName;
	private RectTransform mapButton, searchButton, hambergerButton, backButton, clearButton;

	private RectTransform searchHelpText, searchList;
	private RectTransform mapImage, rightButton, leftButton;
	

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
		searchHelpText = searchInputField.transform.Find("HelpText").GetComponent<RectTransform>();
		searchList = searchInputField.transform.Find("Scroll View").GetComponent<RectTransform>();

		/* map */
		mapImage = mapPanel.transform.Find("MapImage").GetComponent<RectTransform>();
		rightButton = mapPanel.transform.Find("RightButton").GetComponent<RectTransform>();
		leftButton = mapPanel.transform.Find("LeftButton").GetComponent<RectTransform>();
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
		clearButton.sizeDelta = new Vector2(actionBarHeight*0.3f, actionBarHeight*0.3f);
		clearButton.anchoredPosition = new Vector2(((clearButton.sizeDelta.x/2) + (actionBarHeight*0.1f))*-1 , 0);
	}
	public void SetSearchFieldInSearch()
	{
		searchInputField.sizeDelta = new Vector2(Screen.width - actionBarHeight - (actionBarHeight*0.5f), topButtonSize());
		searchInputField.anchoredPosition = new Vector2(searchInputField.sizeDelta.x/2 + actionBarHeight, 0);
	}
	public void SetSeHelpTextInSearch()
	{
		float helpTextPadding = 50f;
		searchHelpText.sizeDelta = new Vector2(Screen.width - helpTextPadding, 50); //<< 50
		searchHelpText.anchoredPosition = new Vector2(0, actionBarHeight + helpTextPadding + searchHelpText.sizeDelta.y);
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
	}
	public void SetArrowButtonInMap()
	{
		rightButton.sizeDelta = new Vector2(actionBarHeight*1.3f, actionBarHeight*1.3f);
		rightButton.anchoredPosition = new Vector2(Screen.width/3.5f, Screen.height/-2.5f);
		leftButton.sizeDelta = new Vector2(actionBarHeight*1.3f, actionBarHeight*1.3f);
		leftButton.anchoredPosition = new Vector2(Screen.width/-3.5f, Screen.height/-2.5f);
	}
	#endregion
	


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


	 /* Unuse grapvyard: get dp value return in pixel */
	private float DPinPixel(int dp)
	{
		return dp * (Screen.dpi/160f);
	}
}
