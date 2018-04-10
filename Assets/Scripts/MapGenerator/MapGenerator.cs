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

    private Vector3 lastPosition;
  
	// Use this for initialization
	void Start ()
	{
        Random.InitState(DateTime.Now.Millisecond);
	    lastPosition = Vector3.zero;
	   
	    GenerateMap();

	}
	
	// Update is called once per frame
	void Update () {
		
	}


    void GenerateMap()
    {
        int numOfTiles = Random.Range(minTiles, maxTiles);
        print("number of tiles: "+numOfTiles);
        int counter = numOfTiles;

        // spawn start tile
        var start = startPrefabs[Random.Range(0, startPrefabs.Length)];
        SpawnTile(lastPosition, Quaternion.identity, start);
        counter--;

        // spawn middle tile
        while (counter >1)
        {
            var middle = midNormalPrefabs[Random.Range(0, midNormalPrefabs.Length)];
            var newPos = lastPosition;
            newPos.z += tileSize;
            SpawnTile(newPos, Quaternion.identity, middle);
            counter--;
        }
       

        // spawn corner tile


        // spawn hole tile
        if (counter <= 1)
        {
            var end = endPrefabs[Random.Range(0, endPrefabs.Length)];
            var newPos = lastPosition;
            newPos.z += tileSize;
            SpawnTile(newPos, Quaternion.Euler(0, 180, 0), end);
            counter--;
        }
       
    }

    void SpawnTile(Vector3 newPos, Quaternion rotation,  GameObject prefab)
    {
        //var hole = endPrefabs[Random.Range(0, endPrefabs.Length)];
        var tile = Instantiate(prefab, newPos, rotation);
        tile.transform.position = newPos;
        lastPosition = newPos;
    }

    
}
