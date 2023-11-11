using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generation : MonoBehaviour
{
    public GameObject room;
    public GameObject parent;
    public GameObject vCorridor;
    public GameObject hCorridor;
    private int recursions = 0;
    private int targetRecursions = 0;

    // Start is called before the first frame update
    void Start()
    {
        var myNewObject = Instantiate(room, new Vector2(0, 0), Quaternion.identity, gameObject.transform);
        var currentVect = new Vector2(0, 0);
        chooseHallwayRandom(currentVect, "pass");
    }

    //removed update{} method

    void chooseHallwayRandom(Vector2 currentVect, string previousDirection)
    {
        var direction = "";
        // This next line should go into an array along with height, vector to centre.m Also they dont have to be vectors but you could combine to make into a single vector.
        var roomDimensions = new Vector2(6, 6);
        var dirChosen = false;

        int directionN = 0;

        while (dirChosen == false)
        {
            var directionNumber = UnityEngine.Random.Range(1, 5);

            if (directionNumber == 1 && previousDirection != "left")
            {
                direction = "right";
                directionN = 1;
                dirChosen = true;
                break;
            }
            else if (directionNumber == 2 && previousDirection != "right")
            {
                direction = "left";
                directionN = 2;
                dirChosen = true;
                break;
            }
            else if (directionNumber == 3 && previousDirection != "bottom")
            {
                direction = "top";
                directionN = 3;
                dirChosen = true;
                break;
            }
            else if (directionNumber == 4 && previousDirection != "top")
            {
                direction = "bottom";
                directionN = 4;
                dirChosen = true;
                break;
            }
        }


        //selects room 0 and gets all relevant information
        var dirToExitList = getRoomInfo(0);
        //takes the newly stated list and gets the relevant peice of information from it. 
        var dirToExit = dirToExitList[directionN];
        // ____ = string[right, left, top, bottom]
        // could also just add "direction = ____[directionNumber-1]" or smth

        Debug.Log(direction);
        spawnHallwayRandom(currentVect, dirToExit, direction, roomDimensions);
    }

    void spawnHallwayRandom(Vector2 currentVect, Vector2 dirToExit, string direction, Vector2 roomDimensions)
    {
        var randomHallwayCount = UnityEngine.Random.Range(3, 7);
        currentVect += new Vector2(dirToExit.x, dirToExit.y);

        if (direction == "top" || direction == "bottom")
        {
            for (var i = 0; i < randomHallwayCount; i++)
            {
                var Hallway = Instantiate(vCorridor, new Vector2(currentVect.x, dirToExit.y + ((direction == "top" ? 1 : -1) * i)), Quaternion.identity, gameObject.transform);
                currentVect.y += (direction == "top" ? 1 : -1);

                Debug.Log(currentVect);
            }
        }

        if (direction == "right" || direction == "left")
        {
            for (var i = 0; i < randomHallwayCount; i++)
            {

                var Hallway = Instantiate(hCorridor, new Vector2(dirToExit.x + ((direction == "right" ? 1 : -1) * i), currentVect.y), Quaternion.identity, gameObject.transform);
                currentVect.x += (direction == "right" ? 1 : -1);

                Debug.Log(currentVect);
            }
        }

        spawnRoomRandom(currentVect, dirToExit, direction, roomDimensions);

    }

    // SO when left is called, because the vectors aren't similar (room is split so 3 and 4 are the dirToExit vectors), the room only needs to spawn 2to the right rather than 4 as 6 (room width - dirToExit.x = 2)


    void spawnRoomRandom(Vector2 currentVect, Vector2 dirToExit, string direction, Vector2 roomDimensions)
    {
        //spawn a room at currentVect + direction to centre of room
        Debug.Log("The Current coordinate is" + currentVect + " . the room should spawn " + dirToExit + " away from current, so will end up at" + new Vector2(currentVect.x + dirToExit.x, currentVect.y + dirToExit.y));
        if (direction == "right")
        {
            currentVect += new Vector2(roomDimensions.x - dirToExit.x, dirToExit.y);
            var spawnedRoom = Instantiate(room, currentVect, Quaternion.identity, gameObject.transform);
        }
        else if (direction == "left")
        {
            currentVect += new Vector2(-(roomDimensions.x + dirToExit.x), dirToExit.y);
            var spawnedRoom = Instantiate(room, currentVect, Quaternion.identity, gameObject.transform);
        }
        else if (direction == "top")
        {
            // 6 - vect 2 = roomWidth - original vector
            currentVect += new Vector2(dirToExit.x, (roomDimensions.y - dirToExit.y));
            var spawnedRoom = Instantiate(room, currentVect, Quaternion.identity, gameObject.transform);
        }
        else if (direction == "bottom")
        {
            currentVect += new Vector2(dirToExit.x, -(roomDimensions.y + dirToExit.y));
            var spawnedRoom = Instantiate(room, currentVect, Quaternion.identity, gameObject.transform);
        }

        var roomInformation = getRoomInfo(0);
        Debug.Log("room dimensions are: " + roomInformation[0]);

        var previousDirection = direction;
        Debug.Log("The CurrentVect of the the most recently installed room is " + currentVect);

        //call chooseHallwayRandom
        incrementRecursionCounter(currentVect, previousDirection);
        //currentVect += getRoomInfo((direction == "right" || direction == "left") || (direction == "top" || direction == "bottom") ? ((direction == "right") ? 1 : 2) : ((direction == "top") ? 3 : 4))
    }


    //Create an array of Vector2's using the array which is pre-made and return it from here
    List<Vector2> getRoomInfo(int roomNumber)
    {
        //array of rooms
        Vector2[,] roomArray = new Vector2[,]
        {
            //room dimensions, dirToRight, dirToLeft, dirToTop, dirToBottom
            {new Vector2(6, 6),new Vector2(3, 0),new Vector2(-4, 0),new Vector2(0, 3),new Vector2(0, -4)}
        };

        List<Vector2> roomInfoList = new List<Vector2>();

        for (var i = 0; i < 5; i++)
        {
            roomInfoList.Add(roomArray[roomNumber, i]);
        }

        return roomInfoList;
    }


    void incrementRecursionCounter(Vector2 currentVect, string previousDirection)
    {
        recursions += 1;

        if (recursions <= targetRecursions)
        {
            recursions += 1;
            chooseHallwayRandom(currentVect, previousDirection);
        }
        else
        {
            stopRecursions();
        }
    }

    void stopRecursions()
    {
        recursions = targetRecursions;
    }
}

//ERRORS
// ROOMS spawn perfectly unless you go in the same direction twice





//Right so how its gonna work
/* Take starting vector (currently just default set to [0,0] + dirToExit)
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
 - Teleporters
 - Spiderman
*/

