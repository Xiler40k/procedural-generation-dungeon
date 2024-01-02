using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generation : MonoBehaviour
{
    public GameObject startingRoom;
    public GameObject parent; //not used
    public GameObject vCorridor; 
    public GameObject hCorridor;
    private int recursions = 0;
    private int targetRecursions = 3; // this is number of desired rooms

    // Start is called before the first frame update
    void Start()
    {
        var spawnRoom = Instantiate(startingRoom, new Vector2(0, 0), Quaternion.identity, gameObject.transform); //doesn't need to be a variable
        var startingVect = new Vector2(0, 8);
        //in this starting case, -1 prompts "top" to instead rig the alogithm to go to top instead of avoid it.
        chooseHallwayRandom(startingVect, "top", -1);
        startingVect = new Vector2(-14, 0);
        chooseHallwayRandom(startingVect, "left", -1);
        startingVect = new Vector2(0, -7);
        chooseHallwayRandom(startingVect, "bottom", -1);
    }

    //removed update{} method

    void chooseHallwayRandom(Vector2 currentVect, string previousDirection, int previousRoomNumber)
    {
        var direction = "";
        // This next line should go into an array along with height, vector to centre.m Also they dont have to be vectors but you could combine to make into a single vector.
        var dirChosen = false; //don't think I need it?
        var dirToExit = new Vector2(0,0);
        Debug.Log("Begining direction selection. Current vect is:" + currentVect);

        int directionN = 0;

        while (dirChosen == false)
        {
            var directionNumber = UnityEngine.Random.Range(1, 5);

            if (previousRoomNumber == -1)
            {
                direction = previousDirection;
                dirChosen = true;
                recursions = 0;
                break;
            }
            else if (directionNumber == 1 && previousDirection != "left")
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

        //if -1 is passed, then the default first room is chosen
        if (previousRoomNumber != -1)
        {
            //selects room 0 and gets all relevant information
            var dirToExitList = getRoomInfo(previousRoomNumber - 1);
            //takes the newly stated list and gets the relevant peice of information from it. 
            dirToExit = dirToExitList[directionN];
            //call method, passing in currentVect and the 
            closeExits(currentVect, dirToExitList, directionN, previousDirection);
        }
        else
        {
            dirToExit = new Vector2(0, 0);
        }

        Debug.Log(direction);
        spawnHallwayRandom(currentVect, dirToExit, direction);
    }

    void closeExits(Vector2 currentVect, List<Vector2> dirToExitList, int directionN, string previousDirection)
    {
        var exitPrefab = Resources.Load<GameObject>("Exits/Exit");
        if (directionN != 1 && previousDirection != "left" )
        {
            var vectToClose = currentVect + dirToExitList[1] + new Vector2(-1, 0);
            exitPrefab = Resources.Load<GameObject>("Exits/Exit1");
            Instantiate(exitPrefab, vectToClose, Quaternion.identity, gameObject.transform);
        }
        if (directionN != 2 && previousDirection != "right")
        {
            var vectToClose = currentVect + dirToExitList[2] + new Vector2(1, 0);
            exitPrefab = Resources.Load<GameObject>("Exits/Exit2");
            Instantiate(exitPrefab, vectToClose, Quaternion.identity, gameObject.transform);
        }
        if (directionN != 3 && previousDirection != "bottom")
        {
            var vectToClose = currentVect + dirToExitList[3] + new Vector2(0, -1);
            exitPrefab = Resources.Load<GameObject>("Exits/Exit3");
            Instantiate(exitPrefab, vectToClose, Quaternion.identity, gameObject.transform);
        }
        if (directionN != 4 && previousDirection != "top")
        {
            var vectToClose = currentVect + dirToExitList[4] + new Vector2(0, 1);
            exitPrefab = Resources.Load<GameObject>("Exits/Exit4");
            Instantiate(exitPrefab, vectToClose, Quaternion.identity, gameObject.transform);
        }
    }

    void spawnHallwayRandom(Vector2 currentVect, Vector2 dirToExit, string direction)
    {
        var randomHallwayCount = UnityEngine.Random.Range(2, 7);
        currentVect += new Vector2(dirToExit.x, dirToExit.y);
        Debug.Log("Hallways should start spawning at " + currentVect);

        if (direction == "top" || direction == "bottom")
        {
            for (var i = 0; i < randomHallwayCount; i++)
            {
                var Hallway = Instantiate(vCorridor, new Vector2(currentVect.x, currentVect.y + ((direction == "top" ? 1 : -1) * i)), Quaternion.identity, gameObject.transform); 
            }
            currentVect.y += (randomHallwayCount - 1) * (direction == "top" ? 1 : -1);
        }
        else
        {
            for (var i = 0; i < randomHallwayCount; i++)
            {
                var Hallway = Instantiate(hCorridor, new Vector2(currentVect.x + ((direction == "right" ? 1 : -1) * i), currentVect.y), Quaternion.identity, gameObject.transform);
            }
            currentVect.x += (randomHallwayCount - 1) * (direction == "right" ? 1 : -1);
        }

        Debug.Log("CurrentVect after hallway: " + currentVect);

        spawnRoomRandom(currentVect, dirToExit, direction);

    }

    // SO when left is called, because the vectors aren't similar (room is split so 3 and 4 are the dirToExit vectors), the room only needs to spawn 2to the right rather than 4 as 6 (room width - dirToExit.x = 2)


    void spawnRoomRandom(Vector2 currentVect, Vector2 dirToExit, string direction)
    {
        int roomNumber = chooseRoomRandom();
        GameObject roomVar = Resources.Load<GameObject>("Rooms/Room" + roomNumber);
        //gets all relevant room Info
        var roomInformation = getRoomInfo(roomNumber - 1);
        //get roomDimensions
        var roomDimensions = roomInformation[0];

        //spawn a room at currentVect + direction to centre of room
        //Debug.Log("The Current coordinate is" + currentVect + " . the room should spawn " + dirToExit + " away from current, so will end up at" + new Vector2(currentVect.x + dirToExit.x, currentVect.y + dirToExit.y));
        if (direction == "right")
        {
            currentVect += new Vector2(-(roomInformation[2]).x, -(roomInformation[2]).y);
            var spawnedRoom = Instantiate(roomVar, currentVect, Quaternion.identity, gameObject.transform);
        }
        else if (direction == "left")
        {
            currentVect += new Vector2(-(roomInformation[1]).x, -(roomInformation[1]).y);
            var spawnedRoom = Instantiate(roomVar, currentVect, Quaternion.identity, gameObject.transform);
        }
        else if (direction == "top")
        {
            // 6 - vect 2 = roomWidth - original vector
            currentVect += new Vector2(-(roomInformation[4]).x, -(roomInformation[4]).y);
            var spawnedRoom = Instantiate(roomVar, currentVect, Quaternion.identity, gameObject.transform);
        }
        else if (direction == "bottom")
        {
            currentVect += new Vector2(-(roomInformation[3]).x, -(roomInformation[3]).y);
            var spawnedRoom = Instantiate(roomVar, currentVect, Quaternion.identity, gameObject.transform);
        }

        Debug.Log("The CurrentVect of the the most recently installed room is " + currentVect);

        //call chooseHallwayRandom
        incrementRecursionCounter(currentVect, direction, roomNumber);
        //currentVect += getRoomInfo((direction == "right" || direction == "left") || (direction == "top" || direction == "bottom") ? ((direction == "right") ? 1 : 2) : ((direction == "top") ? 3 : 4))
    }

    int chooseRoomRandom()
    {
        var roomNumber = UnityEngine.Random.Range(1, 4);
        return roomNumber;
    }


    //Create an array of Vector2's using the array which is pre-made and return it from here
    List<Vector2> getRoomInfo(int roomNumber)
    {
        //array of rooms
        Vector2[,] roomArray = new Vector2[,]
        {
            //room dimensions, dirToRight, dirToLeft, dirToTop, dirToBottom
            {new Vector2(6, 6),new Vector2(3, 0),new Vector2(-4, 0),new Vector2(0, 3),new Vector2(0, -4)},
            {new Vector2(8, 8),new Vector2(4, 0),new Vector2(-5, 0),new Vector2(0, 4),new Vector2(0, -5)},
            {new Vector2(12,14), new Vector2(5, 1), new Vector2(-8, -1), new Vector2(-1, 6), new Vector2(-1, -9)}
        };

        List<Vector2> roomInfoList = new List<Vector2>();

        for (var i = 0; i < 5; i++)
        {
            roomInfoList.Add(roomArray[roomNumber, i]);
        }

        return roomInfoList;
    }


    void incrementRecursionCounter(Vector2 currentVect, string direction, int previousRoomNumber)
    {
        Debug.Log("recursion " + recursions + " has been completed");
        recursions++;
        if (recursions >= targetRecursions)
        {
            Debug.Log("Algorithm stopping. Should be this many rooms: " + (targetRecursions + 1));
            stopRecursions();
        }
        else
        {
            Debug.Log("Room " + (recursions) + "/" + (targetRecursions) + "spawned");
            chooseHallwayRandom(currentVect, direction, previousRoomNumber);
        }
    }

    void stopRecursions()
    {
        recursions = targetRecursions + 1000;
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

