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

	public GameObject actionBar;
	public Image actionBarImage;
	private RectTransform actionBarRect, searchInputField, appName;
	private RectTransform mapButton, searchButton, hambergerButton, backButton, clearButton;

	

	// Use this for initialization
	void Start () {
		Screen.fullScreen = false;
		actionBarHeight = DpToPixel(100);

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
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/* set position and size of button */
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
	public static int DpToPixel(float dp)
     {
         // Convert the dps to pixels
         return (int) (dp * GetScale() + 0.5f);
     }

	public static int PixelToDp(float px)
     {
         // Convert the pxs to dps
         return (int) (px / GetScale() - 0.5f);
     }

	 private static float GetDPI()
     {
         //return Screen.dpi == 0 ? DEFAULT_DPI : Screen.dpi;
		 return DEFAULT_DPI;
     }

	 private static float GetScale()
     {
         return GetDPI() / DEFAULT_DPI;
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
