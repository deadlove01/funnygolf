using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircleControl : MonoBehaviour
{

    public Image circleImage;


    private GameObject circleGo;
    
	// Use this for initialization
	void Start ()
	{
	    circleGo = GameObject.FindGameObjectWithTag(Constants.CIRCLE_IMAGE_TAG);
        circleImage = circleGo.GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update ()
	{
	    if (circleImage == null)
	        return;
	    //var newPos = Camera.main.WorldToScreenPoint(transform.position);
	    var newPos = GameManager.Instance.GetCurrentCamera().WorldToScreenPoint(transform.position);
	    circleImage.transform.position = newPos;

	    circleImage.transform.RotateAround(circleImage.transform.position, Vector3.forward, 20 * Time.deltaTime);
    }

}
