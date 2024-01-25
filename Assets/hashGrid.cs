using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HashGrid : MonoBehaviour
{
    private Hashtable hashGrid = new Hashtable();
    public void addRoom(Vector2 currentPosition, int roomNumber, Vector2 roomDimensions)
    {
        //get info about top bottomLeft corners of room. Iterate key adding for each coord inbetween.
        //initiate key
        int key = 0;

        //find bottom left corner of room
        if (roomDimensions.x % 2 == 1)
        {
            roomDimensions.x -= 1;
        }

        if (roomDimensions.y % 2 == 1)
        {
            roomDimensions.y -= 1;
        }
        var bottomLeftCorner = new Vector2(currentPosition.x - roomDimensions.x/2, currentPosition.y - roomDimensions.y/2);

        for (int i = 0;  i < roomDimensions.x; i++)
        {
            for (int j = 0; j < roomDimensions.y; j++)
            {
                key = generateKey(new Vector2(bottomLeftCorner.x + i, bottomLeftCorner.y + j));
                hashGrid[key] = 1;
                
            }
        }
    } 

    public void testing(int param)
    {
        Debug.Log("Succesffuly accessed hash grid with paparemter " + param);
    }

    public void addHallway (Vector2 currentPosition, string direction, int randomHallwayCount)
    {
        int key = 0;
        //works when the hallway is about the be placed (e.g. the current position is at the exit of a room)
        //only adding walls into grid as room will Always collide with the walls for any collision
        if (direction == "up")
        {
            // add collision placeholders for all walls
            for (int i = 0;  i < randomHallwayCount; i++)
            {
                key = generateKey(new Vector2(currentPosition.x + 1, currentPosition.y + i));
                hashGrid[key] = 1;
                key = generateKey(new Vector2(currentPosition.x - 2, currentPosition.y + i));
                hashGrid[key] = 1;
            }
        }
        else if (direction == "down")
        {
            for (int i = 0;  i < randomHallwayCount; i++)
            {
                key = generateKey(new Vector2(currentPosition.x + 1, currentPosition.y - i));
                hashGrid[key] = 1;
                key = generateKey(new Vector2(currentPosition.x - 2, currentPosition.y - i));
                hashGrid[key] = 1;
            }
        }
        else if (direction == "left")
        {
            for (int i = 0;  i < randomHallwayCount; i++)
            {
                key = generateKey(new Vector2(currentPosition.x - i, currentPosition.y + 1));
                hashGrid[key] = 1;
                key = generateKey(new Vector2(currentPosition.x - i, currentPosition.y - 2));
                hashGrid[key] = 1;
            }
        }
        else if (direction == "right")
        {
            for (int i = 0;  i < randomHallwayCount; i++)
            {
                key = generateKey(new Vector2(currentPosition.x + i, currentPosition.y + 1));
                hashGrid[key] = 1;
                key = generateKey(new Vector2(currentPosition.x + i, currentPosition.y - 2));
                hashGrid[key] = 1;
            }
        }
    }

    public int generateKey(Vector2 currentPosition)
    {
        int key = (int)currentPosition.x * 1000 + (int)currentPosition.y;
        return key;
    }

//check grid for all collisions of a room
    public bool checkRoomSpace(Vector2 currentPosition, Vector2 roomDimensions)
    {
        //find bottom left corner of room
        if (roomDimensions.x % 2 == 1)
        {
            roomDimensions.x -= 1;
        }

        if (roomDimensions.y % 2 == 1)
        {
            roomDimensions.y -= 1;
        }
        var bottomLeftCorner = new Vector2(currentPosition.x - roomDimensions.x/2, currentPosition.y - roomDimensions.y/2);

        // check collisions for all possible walls. 
        for (int i = 0;  i < roomDimensions.x; i++)
        {
            for (int j = 0; j < roomDimensions.y; j++)
            {
                if (checkCollision(new Vector2(bottomLeftCorner.x + i, bottomLeftCorner.y + j)))
                {
                    return true;
                    Debug.Log("added room to hash table");
                }
            }
        }
        return false;
    }
    

    //can probably make next section more efficient
    public bool checkHallwaySpace(Vector2 currentPosition, string direction, int randomHallwayCount)
    {
        var defaultOffset = new Vector2(0, 0);
        var defaultOffset2 = new Vector2(0, 0);

        if (direction == "up" || direction == "down") 
        {
            defaultOffset = new Vector2(1, 0);
            defaultOffset2 = new Vector2(-2, 0);
            var defaultVect = new Vector2(0, 1);
            for (int i = 0; i < randomHallwayCount; i++)
            {
                if (checkCollision(currentPosition + defaultOffset + ((direction == "up" ? +1 : -1) * i * defaultVect)) || checkCollision(currentPosition + defaultOffset2 + ((direction == "up" ? +1 : -1) * i * defaultVect)))
                {
                    return true;
                }
            }
            return false;
        }
        else if (direction == "left" || direction == "right")
        {
            defaultOffset = new Vector2(0, 1);
            defaultOffset2 = new Vector2(0, -2);
            var defaultVect = new Vector2(1, 0);
            for (int i = 0; i < randomHallwayCount; i++)
            {
                if (checkCollision(currentPosition + defaultOffset + ((direction == "right" ? +1 : -1) * i * defaultVect)) || checkCollision(currentPosition + defaultOffset2 + ((direction == "right" ? +1 : -1) * i * defaultVect)))
                {
                    return true;
                }
            }
            return false;
        }
        return false;
    }

    private bool checkCollision(Vector2 currentPosition)
    {
        int key = generateKey(currentPosition);
        if (hashGrid.ContainsKey(key))
        {
            Debug.Log("collision detected");
            return true;
        }

        return false;
    }
}