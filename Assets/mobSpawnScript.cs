using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class mobSpawnScript : MonoBehaviour
{
    public int[] numberOfMobsArray2 = new int[3] {1, 2, 3}; //easy, medium, hard
    public int numberOfMobs;
    private GameObject mobSpawned;
    private LayerMask floorLayerMask;
    private LayerMask wallLayerMask;

    void Start()
    {
        floorLayerMask = LayerMask.GetMask("Floor");
        Debug.Log("floor layer mask: " + floorLayerMask.value);
    }

    public void spawnMobs(Vector2 currentPosition, Vector2 roomDimensions)
    {
        List<int> possibleXCoords = new List<int>();
        List<int> possibleYCoords = new List<int>();

        if (roomDimensions.x % 2 == 1)
        {
            roomDimensions.x -= 1;
        }

        if (roomDimensions.y % 2 == 1)
        {
            roomDimensions.y -= 1;
        }
        var bottomLeftCorner = new Vector2(currentPosition.x - roomDimensions.x/2, currentPosition.y - roomDimensions.y/2);

        //add all numbers except largest and smallest in range of room dimensions
        for (int i = 0; i < roomDimensions.x; i++)
        {
            if (i != 0 && i != 1 && i != roomDimensions.x && i != roomDimensions.x - 1)
            {
                possibleXCoords.Add(i);
            }
        }

        for (int i = 0; i < roomDimensions.y; i++)
        {
            if (i != 0 && i != 1  && i != roomDimensions.y && i != roomDimensions.y - 1)
            {
                possibleYCoords.Add(i);
            }
        }

        //spawn mobs
        if (PlayerPrefs.GetString("difficulty") == "easy")
        {
            numberOfMobs = numberOfMobsArray2[0];
        } else if (PlayerPrefs.GetString("difficulty") == "medium")
        {
            numberOfMobs = numberOfMobsArray2[1];
        } else if (PlayerPrefs.GetString("difficulty") == "hard")
        {
            numberOfMobs = numberOfMobsArray2[2];
        }

        for (int i = 0; i < numberOfMobs; i++)
        {
            int attempts = 0;
            while (attempts < 6)
            {
                var xCoord = possibleXCoords[UnityEngine.Random.Range(0, possibleXCoords.Count)];
                var yCoord = possibleYCoords[UnityEngine.Random.Range(0, possibleYCoords.Count)];
                var mob = Resources.Load("enemy" + UnityEngine.Random.Range(1, 5));
                var mobSpawned = (GameObject)Instantiate(mob, new Vector2(bottomLeftCorner.x + xCoord, bottomLeftCorner.y + yCoord), Quaternion.identity);
                floorLayerMask = LayerMask.GetMask("Floor");
                wallLayerMask = LayerMask.GetMask("Wall");
                if (Physics2D.OverlapCircle(mobSpawned.transform.position, 0.1f, floorLayerMask) == null || Physics2D.OverlapCircle(mobSpawned.transform.position, 0.1f, wallLayerMask) != null)
                {
                    //Debug.Log("Destroying mob at position " + mobSpawned.transform.position);
                    //Debug.Log("floorLayerMask.value: " + floorLayerMask.value);
                    Destroy(mobSpawned);
                    attempts++;
                } else {
                    break;
                }
            
            }
            
        }
    }
}

