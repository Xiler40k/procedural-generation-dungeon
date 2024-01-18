using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HashGrid : MonoBehaviour
{
    public Generation Generation;

    public Hashtable hashGrid = new Hashtable();

    public void addGrid(Vector2 currentPosition, int roomNumber, string direction)
    {
        if (direction == "room") {

            //get info about top left and bottom right corners of room. Iterate key adding for each coord inbetween.

            int key = generateKey(currentPosition);
            hashGrid[key] = 1;
        }
        else if (direction == "up")
        {
            int key = generateKey(currentPosition);
            hashGrid[key] = 1;
        }
        else if (direction == "down")
        {
            int key = generateKey(currentPosition);
            hashGrid[key] = 1;
        }
        else if (direction == "left")
        {
            int key = generateKey(currentPosition);
            hashGrid[key] = 1;
        }
        else if (direction == "right")
        {
            int key = generateKey(currentPosition);
            hashGrid[key] = 1;
        }
    } 

    public int generateKey(Vector2 currentPosition)
    {
        int key = (int)currentPosition.x * 1000 + (int)currentPosition.y;
        return key;
    }

    public bool checkGrid(Vector2 currentPosition)
    {
        int key = generateKey(currentPosition);
        if (hashGrid.ContainsKey(key))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

