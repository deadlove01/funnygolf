using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorLine : MonoBehaviour
{
    public LayerMask ignoreLayer;
    private LineRenderer lineRenderer;

    [SerializeField]
    private float reflectDistance = 10f;

    #region simple singleton

    public static IndicatorLine Instance;
    private int lineIndex = 0;
    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineIndex = 0;
    }
    #endregion

    public void SetActiveLine(bool value)
    {
        lineRenderer.enabled = value;
    }
	
	// Update is called once per frame
	public void DrawLine (Vector3 startPos, Vector3 direction) {
        //print("start: "+startPos+", direction: "+direction);
        Ray ray = new Ray(startPos, direction);
        RaycastHit hit;
	    LayerMask layerMask = ~ignoreLayer;
        if (Physics.Raycast(ray, out hit, 50f, layerMask))
        {
            //print("hit");
            Vector3 reflectAngle = Vector3.Reflect(ray.direction, hit.normal) * reflectDistance;
            lineRenderer.SetPositions(new[]
            {
                startPos, new Vector3(hit.point.x, startPos.y, hit.point.z), 
                new Vector3(reflectAngle.x, startPos.y, reflectAngle.z)
                //, 
                //reflectAngle

            });
        }

        //lineRenderer.SetPosition(0, startPos);
        //lineRenderer.SetPosition(1, direction);
    }
}
