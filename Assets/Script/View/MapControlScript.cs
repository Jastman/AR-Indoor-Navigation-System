using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MapControlScript : MonoBehaviour, IPointerClickHandler
{

    public GameObject mapImageObject;
    private int tabCount = 0;
    private float maxDoubleTabTime = 0.3f;
    private float newTime;
    private float zoomSpeed = 0.5f;
    private bool IsNormalSize;
    private bool hasMovedFlag = false;
    float mapPadding = 30f;

    RectTransform mapImage;

    // Use this for initialization
    void Start()
    {
        mapImage = gameObject.GetComponent<RectTransform>();
        RestoreMap();
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetMouseButtonUp(0))
        // {
        //     tabCount += 1;
        //     if (tabCount == 1)
        //     {
        //         newTime = Time.time + maxDoubleTabTime;
        //     }
        //     else if (tabCount == 2 && Time.time <= newTime)
        //     {
        //         if(!IsNormalSize)
        //         {
        //             RestoreMap();
        //         }
        //         else
        //         {
        //             ZoomMap();
        //         }
        //         //Whatever you want after a dubble tap    
        //         print("Dubble tap");
        //         tabCount = 0;
        //     }
        // }


        if (Input.touchCount == 2)
        {
            //condition that 2 finger once in map
            Debug.Log("2 Touch" + Input.GetTouch(0).phase + "|" + Input.GetTouch(1).phase );
            ZoomMap();
        }
        else if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            //Debug.Log("  hasmoved:" +hasMovedFlag + " |" +touch.phase);
            if (touch.phase == TouchPhase.Moved)
            {
                hasMovedFlag = true;
            }
            if (touch.phase == TouchPhase.Ended)
            {
                if(!hasMovedFlag)
                {
                    tabCount += 1;
                }
                hasMovedFlag = false;
            }

            if (tabCount == 1)
            {
                newTime = Time.time + maxDoubleTabTime;
            }
            else if (tabCount >= 2 && Time.time <= newTime)
            {
                if (!IsNormalSize)
                {
                    RestoreMap();
                }
                else
                {
                    ZoomMapx2();
                }
                //Whatever you want after a dubble tap    
                print("Dubble tap");
                tabCount = 0;
            }
        }
        if (Time.time > newTime)
        {
            tabCount = 0;
        }



    }

    private void ZoomMapx2()
    {
        mapImage.sizeDelta = new Vector2(
                mapImage.sizeDelta.x * 1.5f,
                mapImage.sizeDelta.y * 1.5f
            );
        IsNormalSize = false;
    }

    private void ZoomMap()
    {
        // Store both touches.
        Touch touchZero = Input.GetTouch(0);
        Touch touchOne = Input.GetTouch(1);

        // Find the position in the previous frame of each touch.
        Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
        Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

        // Find the magnitude of the vector (the distance) between the touches in each frame.
        float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
        float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

        // Find the difference in the distances between each frame.
        float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

        mapImage.sizeDelta = new Vector2(
            mapImage.sizeDelta.x - (deltaMagnitudeDiff * zoomSpeed),
            mapImage.sizeDelta.y - (deltaMagnitudeDiff * zoomSpeed)
        );
        mapImage.sizeDelta = new Vector2(
            Mathf.Clamp(mapImage.sizeDelta.x, Screen.width - mapPadding, 3000),
            Mathf.Clamp(mapImage.sizeDelta.y, Screen.width - mapPadding, 3000)
        );
        //mapImage.anchoredPosition = Vector2.zero;
        IsNormalSize = false;
    }

    private void RestoreMap()
    {
        mapImage.sizeDelta = new Vector2(Screen.width - mapPadding, Screen.width - mapPadding);
        mapImage.anchoredPosition = new Vector2(0, 0);
        IsNormalSize = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("It's me: " + name);

    }
}
