using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generation : MonoBehaviour
{
    public GameObject room;
    public GameObject parent;
    public GameObject vCorridor;
    public GameObject hCorridor;
    // Start is called before the first frame update
    void Start()
    {
        var myNewObject = Instantiate(room, new Vector2(0,0), Quaternion.identity, gameObject.transform);
        chooseHallwayRandom();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void chooseHallwayRandom()
    {
        var direction = "";
        var vect2 = new Vector2(0, 0);
        var directionNumber = UnityEngine.Random.Range(1, 5);
        // This next line should go into an array along with height, vector to centre.m Also they dont have to be vectors but you could combine to make into a single vector.
        var roomWidth = new Vector2(6, 0);
        var roomHeight = new Vector2(0, 6);
        if (directionNumber == 1)
        {
            direction = "left";
            vect2 = new Vector2(-4, 0);
        }
        else if (directionNumber == 2)
        {
            direction = "right";
            vect2 = new Vector2(3,0);
        }
        else if (directionNumber == 3)
        {
            direction = "top";
            vect2 = new Vector2(0, 3);
        }
        else if (directionNumber == 4)
        {
            direction = "bottom";
            vect2 = new Vector2(0, -4);
        }

        Debug.Log(direction);
        spawnHallwayRandom(vect2, direction, roomWidth, roomHeight);
    }

    void spawnHallwayRandom(Vector2 vect2, string direction, Vector2 roomWidth, Vector2 roomHeight)
    {
        var randomHallwayCount = UnityEngine.Random.Range(3, 7);
        var currentVect = new Vector2(0, 0);

        if (direction == "top")
        {
            for (var i = 0; i < randomHallwayCount; i++)
            {
                var verticalHallway = Instantiate(vCorridor, new Vector2(0, vect2.y + i), Quaternion.identity, gameObject.transform);
                currentVect = new Vector2(0, vect2.y + randomHallwayCount);
                Debug.Log(currentVect);
            }
        }
        else if (direction == "bottom")
        {
            for (var i = 0; i < randomHallwayCount; i++)
            {
                var verticalHallway = Instantiate(vCorridor, new Vector2(0, vect2.y - i), Quaternion.identity, gameObject.transform);
                currentVect = new Vector2(0, vect2.y - randomHallwayCount);
                Debug.Log(currentVect);
            }
        }
        else if (direction == "left")
        {
            for (var i = 0; i < randomHallwayCount; i++)
            {
                var horizontalHallway = Instantiate(hCorridor, new Vector2(vect2.x - i, 0), Quaternion.identity, gameObject.transform);
                currentVect = new Vector2(vect2.x - randomHallwayCount, 0);
                Debug.Log(currentVect);
            }
        }
        else
        {
            for (var i = 0; i < randomHallwayCount; i++)
            {
                var horizontalHallway = Instantiate(hCorridor, new Vector2(vect2.x + i, 0), Quaternion.identity, gameObject.transform);
                currentVect = new Vector2(vect2.x + randomHallwayCount, 0);
                Debug.Log(currentVect);
            }
        }

        spawnRoomRandom(currentVect, vect2, direction, roomWidth, roomHeight);

    }

    // SO when left is called, because the vectors aren't similar (room is split so 3 and 4 are the vect2 vectors), the room only needs to spawn 2to the right rather than 4 as 6 (room width - vect2.x = 2)


    void spawnRoomRandom(Vector2 currentVect, Vector2 vect2, string direction, Vector2 roomWidth, Vector2 roomHeight)
    {
        //spawn a room at currentVect + direction to centre of room
        Debug.Log("The Current coordinate is" + currentVect + " . the room should spawn " + vect2 + " away from current, so will end up at" + new Vector2(currentVect.x + vect2.x, currentVect.y + vect2.y));
        if (direction == "right")
        {
            var spawnedRoom = Instantiate(room, new Vector2(currentVect.x + (roomWidth.x - vect2.x), currentVect.y + vect2.y), Quaternion.identity, gameObject.transform);
        }
        else if(direction == "top")
        {
            // 6 - vect 2 = roomWidth - original vector
            var spawnedRoom = Instantiate(room, new Vector2(currentVect.x + vect2.x , currentVect.y + (roomHeight.y - vect2.y)), Quaternion.identity, gameObject.transform);
        }
        else if(direction == "bottom")
        {
            var spawnedRoom = Instantiate(room, new Vector2(currentVect.x + vect2.x, currentVect.y - (roomHeight.y + vect2.y)), Quaternion.identity, gameObject.transform);
            Debug.Log("Room spawned at y= " + (currentVect.y - (roomHeight.y + vect2.y)));
        }
        else if(direction == "left")
        {
            var spawnedRoom = Instantiate(room, new Vector2(currentVect.x - (roomWidth.x + vect2.x), currentVect.y + vect2.y), Quaternion.identity, gameObject.transform);
            Debug.Log("Room spawned at x=" + (currentVect.x - (roomWidth.x + vect2.x)));
        }
    }

    void getRoomInfo(int roomNumber)
    {
        //Take data from array and return it
    }

}

//Right so how its gonna work
/* Take starting vector (currently just default set to [0,0] + vect2)
 * Spawn a random hallway in a random direction of random length and add that to current vector
 * Get a random rooms information from an array? Or find a way of making a folder with room assets in it and assigning each one a variable when it is randomly selected. 
 * Using this information, make way from end of hallway to where the new hall should spawn
 * Instantitate a room here
 * Make sure there are no collisions with already made sections
 * Set current vector to new room centre
 * Repeat for many rooms


//Future versions
/*
 - Add backtracking
 - Make hallways not only in one direction
 - Easy way to add new rooms if not already
 - Add a dificulty choice and seed?
 - Add different themes
 - Speedrun timer?
*/

