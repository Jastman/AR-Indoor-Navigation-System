using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowStateManager : MonoBehaviour {

	public GameObject actionBar;
	Image actionBarImg;
	Text appNameText;
	private ToastMessageScript toastMessageScript;
	private enum StateColor
	{
		Idle,
		Navigate
	}

	// Use this for initialization
	void Start () {
		toastMessageScript = gameObject.GetComponent<ToastMessageScript>();
		actionBarImg = actionBar.GetComponent<Image>();
		appNameText = actionBar.gameObject.transform.Find("AppName").gameObject.GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//recive from MainController, may be implement from callback
	public void OnBeginPointChange(GameObject bpoint)
    {
        GameObject dpoint = MainController.instance.destinationPoint;
        GameObject rpoint = MainController.instance.reachedPoint;
		string dp = "null";
        string rp = "null";
		string mesge = "---";
		if(dpoint == null && rpoint == null)
		{
			mesge = "ขณะนี้คุณอยู่ที่: " + bpoint.GetComponent<MarkerData>().roomName + " สามารถเลือกปลายทางที่ต้องการได้ที่ปุ่ม \"แว่นขยาย\""; //ขณะนี้คุณอยู่ที่
			ChangeActionText("อยู่ที่:" + bpoint.GetComponent<MarkerData>().roomName);
		}
		else if(dpoint != null && rpoint == null)
		{
			dp = dpoint.GetComponent<MarkerData>().roomName;
			mesge = "ขณะนี้คุณอยู่ที่: " + bpoint.GetComponent<MarkerData>().roomName; //ขณะนี้คุณ ย้ายมา อยู่ที่: 
			ChangeActionText("ไปยัง:" + dpoint.GetComponent<MarkerData>().roomName);

		}
		else if(dpoint == null && rpoint != null)
		{
			rp = rpoint.GetComponent<MarkerData>().roomName;
			if(bpoint.GetComponent<MarkerData>().roomName == rpoint.GetComponent<MarkerData>().roomName)
			{
				mesge = "มาถึงปลายทางแล้ว: " + bpoint.GetComponent<MarkerData>().roomName; //มาถึงปลายทางแล้ว
				ChangeActionText("ถึงแล้ว:" + bpoint.GetComponent<MarkerData>().roomName);
			}
			else
			{
				mesge = "ขณะนี้คุณอยู่ที่: " + bpoint.GetComponent<MarkerData>().roomName; //ย้ายมาที่
				ChangeActionText("อยู่ที่:" + bpoint.GetComponent<MarkerData>().roomName);
			}
		}
		else if(dpoint != null && rpoint != null)
		{
			dp = dpoint.GetComponent<MarkerData>().roomName;
			rp = rpoint.GetComponent<MarkerData>().roomName;
			if(bpoint.GetComponent<MarkerData>().roomName == rpoint.GetComponent<MarkerData>().roomName)
			{
				mesge = "ขณะนี้คุณอยู่ที่: " + bpoint.GetComponent<MarkerData>().roomName; //กลับมาอยู่ที่
				ChangeActionText("ไปยัง:" + dpoint.GetComponent<MarkerData>().roomName);
			}
			else
			{
				mesge = "ขณะนี้คุณอยู่ที่: " + bpoint.GetComponent<MarkerData>().roomName; //ขณะนี้คุณย้ายมาอยู่ที่
				ChangeActionText("ไปยัง:" + dpoint.GetComponent<MarkerData>().roomName);
			}
		}
		Debug.Log(mesge+"  |  Begin Change - State:" + MainController.instance.appState+"  |  "
		+ "dest: " + dp + "  reach: " + rp);
		toastMessageScript.showToastOnUiThread(mesge, true);
		ChangeActionBarColor(MainController.instance.appState == MainController.AppState.Idle ? StateColor.Idle : StateColor.Navigate);
	}

	public void OnDestinationPointChange(GameObject dpoint)
    {
        GameObject bpoint = MainController.instance.beginPoint;
        GameObject rpoint = MainController.instance.reachedPoint;
		string bp = "null";
        string rp = "null";
		string mesge = "---";
		if(dpoint == null)
		{
			mesge = "ยกเลิกการนำทางแล้ว";
			ChangeActionText("AR Indoor Navigation");
		}
		else if(bpoint == null && dpoint != null && rpoint == null)
		{
			mesge = "ต้องการไปยัง: " + dpoint.GetComponent<MarkerData>().roomName + " กรุณาส่องกล้องไปยังรอบๆ เพื่อระบุตำแหน่งของคุณ";
			ChangeActionText("ส่องกล้องไปยังรอบๆ เพื่อระบุตำแหน่ง");
			toastMessageScript.showToastOnUiThread(mesge, true);
		}
		else if(bpoint != null && dpoint != null && rpoint == null)
		{
			bp = bpoint.GetComponent<MarkerData>().roomName;
			mesge = "เริ่มการนำทางไปยัง: " + dpoint.GetComponent<MarkerData>().roomName + " แล้ว";
			ChangeActionText("ไปยัง:" + dpoint.GetComponent<MarkerData>().roomName);
		}
		else if(bpoint == null && dpoint != null && rpoint != null)
		{
			rp = rpoint.GetComponent<MarkerData>().roomName;
			mesge = "เริ่มการนำทางไปยัง? " + dpoint.GetComponent<MarkerData>().roomName + " แล้ว"; //เลือกปลายทางไปที่
			ChangeActionText("ไปยัง:" + dpoint.GetComponent<MarkerData>().roomName);
			
		}
		else if(bpoint != null && dpoint != null && rpoint != null)
		{
			bp = bpoint.GetComponent<MarkerData>().roomName;
			rp = rpoint.GetComponent<MarkerData>().roomName;
			if(bpoint.GetComponent<MarkerData>().roomName == dpoint.GetComponent<MarkerData>().roomName)
			{
				mesge = "จุดล่าสุดของคุณคือจุดที่คุณเลือก กรุณาเลือกปลายทางที่อื่น: " + dpoint.GetComponent<MarkerData>().roomName ;
			}
			else if(dpoint.GetComponent<MarkerData>().roomName == rpoint.GetComponent<MarkerData>().roomName)
			{
				mesge = "เริ่มการนำทางไปยัง: " + dpoint.GetComponent<MarkerData>().roomName + " แล้ว"; //นำทางซ้ำไปที่ๆเคยไป
				ChangeActionText("ไปยัง:" + dpoint.GetComponent<MarkerData>().roomName);
			}
			else
			{
				mesge = "เริ่มการนำทางไปยัง: " + dpoint.GetComponent<MarkerData>().roomName + " แล้ว"; //เลือกปลายทาง ไปที่
				ChangeActionText("ไปยัง:" + dpoint.GetComponent<MarkerData>().roomName);
			}
		}
		Debug.Log(mesge+"  |  Dest Change - State:" + MainController.instance.appState+"  |  "
		+ "begin: " + bp + "  reach: " + rp);
		toastMessageScript.showToastOnUiThread(mesge, true);
		ChangeActionBarColor(MainController.instance.appState == MainController.AppState.Idle ? StateColor.Idle : StateColor.Navigate);
	}

	private void ChangeActionBarColor(StateColor color)
	{
		switch (color)
        {
            case StateColor.Idle:
                actionBar.GetComponent<Image>().color = new Color32(60, 126, 255, 255);
                break;
            case StateColor.Navigate:
                actionBar.GetComponent<Image>().color = new Color32(126, 60, 255, 255);
                break;
            default: actionBar.GetComponent<Image>().color = new Color32(60, 126, 255, 255); break;
        }
	}

	private void ChangeActionText(string actext)
    {
        appNameText.text = actext;
    }
}
