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
	public Image actionBarImage;
	private RectTransform actionBar, mapButton, searchButton, hambergerButton;

	

	// Use this for initialization
	void Start () {
		Screen.fullScreen = false;
		actionBarHeight = DpToPixel(100);

		actionBar = actionBarImage.gameObject.GetComponent<RectTransform>();
		actionBar.sizeDelta = new Vector2(Screen.width, actionBarHeight);
		actionBar.anchoredPosition = new Vector2(0, actionBarHeight/-2) ;
		
		mapButton = actionBarImage.gameObject.transform.Find("MapButton").GetComponent<RectTransform>();
		mapButton.sizeDelta = new Vector2(topButtonSize(), topButtonSize());
		mapButton.anchoredPosition = new Vector2((topButtonPosition()*-1), topButtonPosition()*-1);
		

		searchButton = actionBarImage.gameObject.transform.Find("SearchButton").GetComponent<RectTransform>();
		searchButton.sizeDelta = new Vector2(topButtonSize(), topButtonSize());
		searchButton.anchoredPosition = new Vector2(topButtonPosition()*-1 - topButtonSize() - topButtonPadding(), topButtonPosition()*-1);
		
		hambergerButton = actionBarImage.gameObject.transform.Find("HambergerButton").GetComponent<RectTransform>();
		hambergerButton.sizeDelta = new Vector2(topButtonSize(), topButtonSize());
		hambergerButton.anchoredPosition = new Vector2(topButtonPosition(), topButtonPosition()*-1);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

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

	/* get dp value return in pixel */
	private float DPinPixel(int dp)
	{
		return dp * (Screen.dpi/160f);
	}



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
}
