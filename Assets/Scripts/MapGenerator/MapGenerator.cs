using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapGenerator : MonoBehaviour
{
    public enum TileDirection
    {
        Up,
        Down,
        Left,
        Right
    }

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
    [SerializeField]
    private int cornerTileGap = 2;

    private Vector3 lastPosition;

    private GameObject lastGameObject = null;

    private TileDirection lastTileDirection;
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

    public void GenerateMap2()
    {
        if (lastGameObject != null)
            Destroy(lastGameObject);
        lastPosition = Vector3.zero;
        GameObject mapParent = new GameObject("MapParent");
        mapParent.transform.position = lastPosition;
        int numOfTiles = Random.Range(minTiles, maxTiles);
        print("number of tiles: " + numOfTiles);
        int counter = numOfTiles;

        // spawn start tile
        var start = startPrefabs[Random.Range(0, startPrefabs.Length)];
        var tile = SpawnTile(lastPosition, Quaternion.identity, start);
        tile.transform.parent = mapParent.transform;
        lastTileDirection = TileDirection.Up;
        counter--;

        bool hasCorner = false;
        int cornerCoolDown = cornerTileGap;
        while (counter > 1)
        {
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
                    SpawnTileCorner(mapParent.transform);
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
            if (prefab != null)
            {
                SpawnTileMid(prefab, mapParent.transform);
            }
            if (hasCorner)
            {
                cornerCoolDown--;
            }
            if (cornerCoolDown <= 0)
            {
                hasCorner = false;
                cornerCoolDown = cornerTileGap;
            }
            counter--;
        }
        // spawn hole tile
        if (counter <= 1)
        {
            SpawnTileEnd(mapParent.transform);
            counter--;
        }
        lastGameObject = mapParent;
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
        int countAllowCorner = 0;
        bool hasCornerBefore = false;
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
                    if(!hasCornerBefore)
                        angleY = randNum % 2 == 0 ? 180 : 270;
                    else
                        angleY = randNum % 2 == 0 ? 0 : 180;
                    hasCorner = true;
                    changeDirection = false;
                    countAllowCorner = 3;
                  
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
                //newPos.z += tileSize;
                switch (lastTileDirection)
                {
                    case TileDirection.Up: newPos.z += tileSize;
                        break;
                    case TileDirection.Down: newPos.z -= tileSize;
                        break;
                    case TileDirection.Left: newPos.x -= tileSize;
                        break;
                    case TileDirection.Right:
                        newPos.x += tileSize;
                        break;

                }
            }else if (!hasCorner && !changeDirection)
            {
                newPos.z += tileSize;
                lastTileDirection = TileDirection.Up;
            }
            else
            {
                int multiplier = angleY == 180 ? 1 : -1;
                newPos.x += multiplier * tileSize;
                if (angleY == 180)
                    localAngleY = 90;
                else if (angleY == 270)
                    localAngleY = -90;
                lastTileDirection = multiplier == 1 ? TileDirection.Right : TileDirection.Left;
            }
           
          
            var go = SpawnTile(newPos, Quaternion.Euler(0, localAngleY!= 0? localAngleY: angleY, 0), prefab);
            go.transform.parent = mapParent.transform;
            counter--;
            if(hasCorner)
                countAllowCorner--;
            if (countAllowCorner <= 0)
            {
                hasCorner = false;
                hasCornerBefore = true;
            }
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


    GameObject SpawnTileMid(GameObject prefab, Transform mapParent)
    {
        //up: 0
        //left:90
        //right: 90
        //down: 0
        float angle = 0;
        var newPos = lastPosition;
        switch (lastTileDirection)
        {
            case TileDirection.Up:
                angle = 0;
                newPos.z += tileSize;
                break;
            case TileDirection.Down:
                angle = 0;
                newPos.z -= tileSize;
                break;
            case TileDirection.Left:
                angle = 90;
                newPos.x -= tileSize;
                break;
            case TileDirection.Right:
                angle = 90;
                newPos.x += tileSize;
                break;
        }
        //var prefab = endPrefabs[Random.Range(0, endPrefabs.Length)];
        var tile = SpawnTile(newPos, Quaternion.Euler(0, angle, 0), prefab);
        tile.transform.position = newPos;
        tile.transform.parent = mapParent;
        lastPosition = newPos;
        return tile;
    }
    GameObject SpawnTileCorner(Transform mapParent)
    {
        //up: 180, 270
        //left: 0, 180
        //right: 0, 270
        //down: 0, 90
        float angle1 = 180;
        float angle2 = 270;
        var newPos = lastPosition;
        switch (lastTileDirection)
        {
            case TileDirection.Up:
                angle1 = 180;
                angle2 = 270;
                newPos.z += tileSize;
                break;
            case TileDirection.Down:
                angle1 = 0;
                angle2 = 90;
                newPos.z -= tileSize;
                break;
            case TileDirection.Left:
                angle1 = 90;
                angle2 = 180;
                newPos.x -= tileSize;
                break;
            case TileDirection.Right:
                angle1 = 0;
                angle2 = 270;
                newPos.x += tileSize;
                break;
        }
        var randNum = Random.Range(0, 10);
        float angleY = randNum % 2 == 0 ? angle1 : angle2;
        if (angleY == 180)
        {
            if (lastTileDirection == TileDirection.Up)
            {
                lastTileDirection = TileDirection.Right;
            }
            else
            {
                lastTileDirection = TileDirection.Down;
            }
            
        }else if (angleY == 270)
        {
            if (lastTileDirection == TileDirection.Up)
            {
                lastTileDirection = TileDirection.Left;
            }
            else
            {
                lastTileDirection = TileDirection.Down;
            }
        }
        else if (angleY == 0)
        {
            if (lastTileDirection == TileDirection.Down)
            {
                lastTileDirection = TileDirection.Left;
            }
            else
            {
                lastTileDirection = TileDirection.Up;
            }
        }
        else if (angleY == 90)
        {
            if (lastTileDirection == TileDirection.Down)
            {
                lastTileDirection = TileDirection.Right;
            }
            else
            {
                lastTileDirection = TileDirection.Up;
            }
        }
        var prefab = cornerPrefabs[Random.Range(0, cornerPrefabs.Length)];
        var tile = SpawnTile(newPos, Quaternion.Euler(0, angleY, 0), prefab);
        tile.transform.position = newPos;
        tile.transform.parent = mapParent;
        lastPosition = newPos;
        return tile;
    }
    GameObject SpawnTileEnd(Transform mapParent)
    {
        //up: 180
        //left: 90
        //right:270
        //down: 0
        float angle = 180;
        var newPos = lastPosition;
        switch (lastTileDirection)
        {
            case TileDirection.Up:
                angle = 180;
                newPos.z += tileSize;
                break;
            case TileDirection.Down:
                angle = 0;
                newPos.z -= tileSize;
                break;
            case TileDirection.Left:
                angle = 90;
                newPos.x -= tileSize;
                break;
            case TileDirection.Right:
                angle = 270;
                newPos.x += tileSize;
                break;
        }
        var prefab = endPrefabs[Random.Range(0, endPrefabs.Length)];
        var tile = SpawnTile(newPos, Quaternion.Euler(0, angle, 0), prefab);
        tile.transform.position = newPos;
        tile.transform.parent = mapParent;
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
