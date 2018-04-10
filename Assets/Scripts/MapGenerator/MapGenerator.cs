using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapGenerator : MonoBehaviour
{

    public int minTiles = 5;
    public int maxTiles = 10;
    public float tileSize = 3f;

 
    public GameObject[] startPrefabs;
    public GameObject[] endPrefabs;
    public GameObject[] midNormalPrefabs;
    public GameObject[] midSpecialPrefabs;
    public GameObject[] obstaclePrefabs;
    public GameObject[] cornerPrefabs;

    [Range(0f, 1f)]
    [SerializeField] private float percentCorner = 0.5f;
    [Range(0f, 1f)]
    [SerializeField] private float percentMidSpecial = 0.5f;

    [Range(0f, 1f)]
    [SerializeField]
    private float percentObstacle = 0.3f;

    private Vector3 lastPosition;

    private GameObject lastGameObject = null;
	// Use this for initialization
	void Start ()
	{
        Random.InitState(DateTime.Now.Millisecond);
	    lastPosition = Vector3.zero;
	   
	    //GenerateMap();

	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public void GenerateMap()
    {
        if(lastGameObject != null)
            Destroy(lastGameObject);
        lastPosition = Vector3.zero;
        GameObject mapParent = new GameObject("MapParent");
        mapParent.transform.position = lastPosition;
        int numOfTiles = Random.Range(minTiles, maxTiles);
        print("number of tiles: "+numOfTiles);
        int counter = numOfTiles;

        // spawn start tile
        var start = startPrefabs[Random.Range(0, startPrefabs.Length)];
        var tile = SpawnTile(lastPosition, Quaternion.identity, start);
        tile.transform.parent = mapParent.transform;
        counter--;

        // spawn middle tile
        float angleY = 0;
        bool hasCorner = false;
        bool changeDirection = false;
        while (counter >1)
        {
            float localAngleY = 0;
            GameObject prefab = null;
            var randomPercent = Random.Range(0.0f, 1.0f);
            if (randomPercent < percentMidSpecial)
            {
                prefab = midSpecialPrefabs[Random.Range(0, midSpecialPrefabs.Length)];
            }
            else if (!hasCorner)
            {
                randomPercent = Random.Range(0.0f, 1.0f);
                if (randomPercent < percentCorner)
                {
                    prefab = cornerPrefabs[Random.Range(0, cornerPrefabs.Length)];
                    var randNum = Random.Range(0, 10);
                    angleY = randNum % 2 == 0 ? 180 : 270;
                    hasCorner = true;
                }
                else
                {
                    prefab = midNormalPrefabs[Random.Range(0, midNormalPrefabs.Length)];
                }
               
            }
            else
            {
                randomPercent = Random.Range(0.0f, 1.0f);
                if (randomPercent < percentObstacle)
                {
                    prefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
                }
                else
                {
                    prefab = midNormalPrefabs[Random.Range(0, midNormalPrefabs.Length)];
                }
               
            }

            var newPos = lastPosition;
            if ((hasCorner && !changeDirection)
               )
            {
                changeDirection = true;
                newPos.z += tileSize;
            }else if (!hasCorner)
            {
                newPos.z += tileSize;
            }
            else
            {
                int multiplier = angleY == 180 ? 1 : -1;
                newPos.x += multiplier * tileSize;
                if (angleY == 180)
                    localAngleY = 90;
                else if (angleY == 270)
                    localAngleY = -90;
            }
           
          
            var go = SpawnTile(newPos, Quaternion.Euler(0, localAngleY!= 0? localAngleY: angleY, 0), prefab);
            go.transform.parent = mapParent.transform;
            counter--;
        }
       

        // spawn corner tile


        // spawn hole tile
        if (counter <= 1)
        {
            var end = endPrefabs[Random.Range(0, endPrefabs.Length)];
            var newPos = lastPosition;
            if (angleY != 0)
            {
                int multiplier = angleY == 180 ? 1 : -1;
                newPos.x += multiplier * tileSize;
            }
            else
            {
                newPos.z += tileSize;
            }
           
            var angle = 180;
            if (angleY == 180)
            {
                angle = 270;
            }else if (angleY == 270 || angleY == -90)
                angle = 90;
            var go = SpawnTile(newPos, Quaternion.Euler(0, angle, 0), end);
            go.transform.parent = mapParent.transform;
            counter--;
        }

        lastGameObject = mapParent;
    }

    GameObject SpawnTile(Vector3 newPos, Quaternion rotation,  GameObject prefab)
    {
        //var hole = endPrefabs[Random.Range(0, endPrefabs.Length)];
        var tile = Instantiate(prefab, newPos, rotation);
        tile.transform.position = newPos;
        lastPosition = newPos;
        return tile;
    }

  


    float SpawnCornerTile()
    {
        var randNum = Random.Range(0, 10);
        float angleY = randNum % 2 == 0 ? 180 : 270;
        var end = cornerPrefabs[Random.Range(0, cornerPrefabs.Length)];
        var newPos = lastPosition;
        newPos.z += tileSize;
        SpawnTile(newPos, Quaternion.Euler(0, angleY, 0), end);
        return angleY;
    }
    
}
