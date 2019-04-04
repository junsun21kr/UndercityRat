using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PopUpDamageController : MonoBehaviour
{

    //static methods require static references
    private static GameObject popupText;
    private static GameObject popupHeadText;
    private static RectTransform parent;
    private static float min = -.5f, max = .5f;

    private void Start()
    {
        //set the parent to the ui group
        parent = GetComponent<RectTransform>();
    }

    public static void CreateFloatingText(string text, Transform location,bool isHead)
    {
        //find if null
        GameObject instance;
        if (isHead == true)
        {
            if(popupHeadText == null)
                popupHeadText = Resources.Load("Prefabs/PopUpPosHeadShot") as GameObject;
            instance = Instantiate(popupHeadText);
        }
        else
        {
            if (popupText == null)
                popupText = Resources.Load("Prefabs/PopUpPos") as GameObject;
            instance = Instantiate(popupText);
        }
        

        //create an instance of the text prefab
        //convert it's called location to screenspace
        Vector2 screenPos = Camera.main.WorldToScreenPoint(location.position+ Vector3.up * Random.Range(min+2f,max+2f)+Vector3.right*Random.Range(min,max));
        //parent that instance to the canvas
        instance.transform.SetParent(parent, false);
        //set the instance's position to screenPos
        instance.transform.position = screenPos;
        //set the text to passed
        instance.GetComponentInChildren<Text>().text = text;
        Destroy(instance, 1f);
    }
}
