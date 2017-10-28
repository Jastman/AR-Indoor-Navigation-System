using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIValue : MonoBehaviour {

	public static UIValue value;
	public string STRING_FINDMARKER = "ส่องกล้องไปยังจุดต่างๆ เช่น ป้ายบอกทาง เลขห้อง\nเพื่อเริ่มต้นระบุตำแหน่งของคุณ";
	public string STRING_SELECTDEST = "เลือกปลายทางที่ต้องการไปที่ปุ่ม \"แว่นขยาย\" ";
	public string STRING_TOCANCLE_NAVIGATE = "\n สามารถกดที่ปลายทางอีกครั้ง เพื่อยกเลิกการนำทาง";
    public string STRING_REVOKE_NAVIGATE = "ได้ยกเลิกการนำทางแล้ว";
	public string STRING_CURRENT_POSITION = "ขณะนี้คุณอยู่ที่: \n";
	public string STRING_CHANGE_DESTINATION = "เริ่มการนำทางไปยัง: \n";
    public string STRING_REACH_DESTINATION = "คุณได้มาถึงปลายทาง: \n";

	// Use this for initialization
	void Awake()
    {
        if (value == null)
        {
            value = this;
        }
        else if (value != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
