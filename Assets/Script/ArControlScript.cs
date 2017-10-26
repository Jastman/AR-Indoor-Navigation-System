using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArControlScript : MonoBehaviour
{
    public GameObject arrowPrefab, checkTruePrefab, descriptionBoardPrefab;
	public float descriptionBoardSize = 1f, arrowSize = 1f, checkSize = 1f;
	public Vector3 objectPosition = Vector3.zero;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public GameObject CreateArrow() /* find or create Arrow AR return script of arrow */
    {
        List<ArrowScript> arrowList = new List<ArrowScript>();
        gameObject.GetComponentsInChildren<ArrowScript>(true, arrowList);
        GameObject arrowObj;
        if (arrowList.Count == 1)
        {
            arrowObj = arrowList[0].gameObject; //not transform to gameobj
            arrowObj.SetActive(true);
        }
        else
        {
            foreach (ArrowScript dsb in arrowList)
            {
                Destroy(dsb.gameObject);
            }
            arrowObj = Instantiate(arrowPrefab);
        }

        arrowObj.transform.SetParent(gameObject.transform);
        arrowObj.transform.localPosition = Vector3.zero;
        arrowObj.transform.rotation = arrowObj.transform.parent.rotation;
        arrowObj.transform.localScale = Vector3.one * arrowSize;

        /* set started arrow rotation here */
        ArrowScript arrow = arrowObj.GetComponent<ArrowScript>();
        //arrow.PointToZero();
		Debug.Log(gameObject.name  + " attract arrow to nav");
        return arrowObj;
		
    }

    public GameObject CreateCheckTrue()
    {
        List<CheckTrueScript> checktrueList = new List<CheckTrueScript>();
        gameObject.GetComponentsInChildren<CheckTrueScript>(true, checktrueList);
        GameObject checktrueObj;
        if (checktrueList.Count == 1)
        {
            checktrueObj = checktrueList[0].gameObject;
            checktrueObj.SetActive(true);
        }
        else
        {
            foreach (CheckTrueScript dsb in checktrueList)
            {
                Destroy(dsb.gameObject);
            }
            checktrueObj = Instantiate(checkTruePrefab);
        }
        checktrueObj.transform.SetParent(gameObject.transform);
        checktrueObj.transform.localPosition = Vector3.zero;
        checktrueObj.transform.rotation = gameObject.transform.parent.rotation;
        checktrueObj.transform.localScale = Vector3.one * checkSize;
        Debug.Log(gameObject.name + " attract checkTrue to found dest");
        return checktrueObj;
    }

    public GameObject CreateDescriptionBoard()
    {
        //check have that conponent if 1 use it again, if 0 or more destroy all and create new one
        List<DescriptionBoardScript> desBoardList = new List<DescriptionBoardScript>();
        gameObject.GetComponentsInChildren<DescriptionBoardScript>(true, desBoardList);
        GameObject desBoardObj;
        if (desBoardList.Count == 1)
        {
            desBoardObj = desBoardList[0].gameObject; //not transform to gameobj
            desBoardObj.SetActive(true);
        }
        else
        {
            foreach (DescriptionBoardScript dsb in desBoardList)
            {
                Destroy(dsb.gameObject);
            }
            desBoardObj = Instantiate(descriptionBoardPrefab);
        }
        desBoardObj.transform.SetParent(gameObject.transform);
        desBoardObj.transform.localPosition = Vector3.zero;
        desBoardObj.transform.rotation = desBoardObj.transform.parent.rotation;
        desBoardObj.transform.localScale = Vector3.one * descriptionBoardSize;

        DescriptionBoardScript desBoard = desBoardObj.GetComponent<DescriptionBoardScript>();
        desBoard.SetRoomName(gameObject.GetComponent<MarkerData>().roomName);
        desBoard.SetRoomDest(gameObject.GetComponent<MarkerData>().description);

        Debug.Log(gameObject.name + " attract desboard to idle");
		return desBoardObj;
    }

	public GameObject GetArrow()
	{
		foreach (Transform child in gameObject.transform)
		{
			if(child.gameObject.GetComponent<ArrowScript>() != null)
			{
				return child.gameObject;
			}
		}
		return CreateArrow();
	}

	public GameObject GetDescriptionBoard()
	{
		return null;
	}

	public GameObject GetCheckTrue()
	{
		return null;
	}

}
