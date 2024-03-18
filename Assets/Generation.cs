using System;
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
    private int targetRecursions = 6; // this is desired number of rooms per path iteration. 
    private int[] targetRecursionsArray = new int[3] { 3, 3, 5 } ;
    HashGrid hashTable;
    Backtrack backtrack;
    seedScript SeedScript;
    bool isBacktracking = false;
    public Vector2 storedExitCoords;
    public int allDirectionsTriedCounter = 0;
    bool isFirstIteration = true;
    private int pathNumber = 1;

    // Start is called before the first frame update

    void Awake()
    {
        PlayerPrefs.SetInt("keysCollected", 0);

        //based on what difficulty the user chose, set the targetrecursionsarray
        if (PlayerPrefs.GetString("difficulty") == "medium")
        {
            targetRecursionsArray = new int[] { 3, 5, 6 };
            Debug.Log("Medium mode chosen");

        } else if (PlayerPrefs.GetString("difficulty") == "hard")
        {
            targetRecursionsArray = new int[] { 6, 6, 6 };
            Debug.Log("Hard mode chosen");
        }

        SeedScript = GameObject.Find("SeedSystem").GetComponent<seedScript>();
        if (PlayerPrefs.GetInt("seed") != 0)
        {
            SeedScript.setSeed(PlayerPrefs.GetInt("seed"));
            Debug.Log("Seed set to " + PlayerPrefs.GetInt("seed"));
        } else {
            SeedScript.randomSeed();
        }


        if (PlayerPrefs.GetInt("generateDungeon") == 1)
        {
            generateDungeon();
            PlayerPrefs.SetInt("generateDungeon", 0);
        }

    }
    public void generateDungeon()
    {
        PlayerPrefs.SetInt("keysCollected", 0);

        hashTable = GameObject.FindGameObjectWithTag("HashTable").GetComponent<HashGrid>(); //not sure If I need anymore
        hashTable.testing(1);
        backtrack = GameObject.FindGameObjectWithTag("Backtrack").GetComponent<Backtrack>(); //not sure I need
        backtrack.testing(2);


        //add starting room to hashTable should be 26, 12 but I think it may exclude walls
        hashTable.addRoom(new Vector2(0, 0), new Vector2(26, 12));
        //hallway, right, (12,0), length 14
        hashTable.addHallway(new Vector2(12, 0), "right", 14);
        //room, centre (39, 0) Room Dimensions: (24, 24)
        hashTable.addRoom(new Vector2(39, 0), new Vector2(24, 24));


        var spawnRoom = Instantiate(startingRoom, new Vector2(0, 0), Quaternion.identity, gameObject.transform); //doesn't need to be a variable
        var startingVect = new Vector2(0, 8);

        //in this starting case, -1 prompts "up" to instead rig the alogithm to go to up instead of avoid it.
        //temp add a box around path 2 entrance to collider.
        hashTable.addRoom(new Vector2(-20, 0), new Vector2(16, 14));
        //resetTriedArray();

        isBacktracking = false;
        pathNumber = 1;
        targetRecursions = targetRecursionsArray[0];
        chooseHallwayRandom(startingVect, "up", -1, isBacktracking, true);
        clearStack();
        //resetTriedArray();
        recursions = 0;

        //remove path 2's entrance collider and add path 3 entrance collider
        hashTable.removeRoom(new Vector2(-20, 0), new Vector2(16, 14));
        hashTable.addRoom(new Vector2(0, -12), new Vector2(14, 16));

        startingVect = new Vector2(-14, 0);
        isBacktracking = false;
        pathNumber = 2;
        targetRecursions = targetRecursionsArray[1];
        chooseHallwayRandom(startingVect, "left", -1, isBacktracking, true);
        clearStack();
        recursions = 0;

        //remove path 3's entrance collider
        hashTable.removeRoom(new Vector2(0, -12), new Vector2(14, 16));

        startingVect = new Vector2(0, -7);
        isBacktracking = false;
        //resetTriedArray();
        pathNumber = 3;
        targetRecursions = targetRecursionsArray[2];
        chooseHallwayRandom(startingVect, "down", -1, isBacktracking, true);
        
    }

    //removed update() method

    public int[] triedArray = new int[4];
    //set all values i array to 0

    void chooseHallwayRandom(Vector2 currentVect, string previousDirection, int previousRoomNumber, bool isBacktracking, bool isFirstIteration)
    {
        //add data to backTracking system (upgrade later to make sure that the same direction is not chosen twice) if not backtracking or rechecking hallways for a room
        backtrack.addBacktrack(currentVect, previousDirection, previousRoomNumber);
        var direction = "";

        isFirstIteration = backtrack.checkFirstIteration(currentVect);
        Debug.Log("Is first iteration: " + isFirstIteration);

        
        // This next line should go into an array along with height, vector to centre.m Also they dont have to be vectors but you could combine to make into a single vector.
        var dirChosen = false; //don't think I need it?
        var dirToExit = new Vector2(0,0);
        Debug.Log("Begining direction selection. Current vect is:" + currentVect);
        Debug.Log("The paramteres for this iteration are: " + backtrack.retrieveInformation(0));
        int directionN = 0;

        var randomBacktrack = UnityEngine.Random.Range(1, 101);
        //checks if all 3 possible directions have been checked OR if random backtrack and its not the first room being spawned.

        if (backtrack.dirDictKeyExists(currentVect) == true)
        {
            triedArray = backtrack.getDictArray(currentVect);
        }
        else
        {
            triedArray = new int[5];
        }

        if (((triedArray[0] + triedArray[1] + triedArray[2] + triedArray[3]) >= 3) && isBacktracking == true)
        {
            //If backtracking to a room that has already tried all directions, then every exits shouldn't close????
            Debug.Log("All directions have been tried. No exits should close. Backtracking");
            generationBacktrack(currentVect, previousDirection, previousRoomNumber, false);
            return;
        }
        else if (((triedArray[0] + triedArray[1] + triedArray[2] + triedArray[3]) >= 3))
        {
            Debug.Log("All directions have been tried. Backtracking");
            closeCurrentExits(currentVect, getRoomInfo(previousRoomNumber - 1), previousDirection);
            generationBacktrack(currentVect, previousDirection, previousRoomNumber, false);
            return;
        }

        if ((randomBacktrack < 50 && recursions > 2)) // make not possible if already in backtrack process
        {
            // make not possible if already in backtrack process
            if (!isBacktracking) {
                Debug.Log("Random backtrack chosen");
                closeCurrentExits(currentVect, getRoomInfo(previousRoomNumber - 1), previousDirection);
                generationBacktrack(currentVect, previousDirection, previousRoomNumber, true);
                return;
            }
        }

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
            else if (directionNumber == 1 && previousDirection != "left" && triedArray[0] != 1)
            {
                direction = "right";
                directionN = 1;
                dirChosen = true;
                break;
            }
            else if (directionNumber == 2 && previousDirection != "right" && triedArray[1] != 1)
            {
                direction = "left";
                directionN = 2;
                dirChosen = true;
                break;
            }
            else if (directionNumber == 3 && previousDirection != "down" && triedArray[2] != 1)
            {
                direction = "up";
                directionN = 3;
                dirChosen = true;
                break;
            }
            else if (directionNumber == 4 && previousDirection != "up" && triedArray[3] != 1)
            {
                direction = "down";
                directionN = 4;
                dirChosen = true;
                break;
            }
        }
        Debug.Log("Direction chosen is " + direction);
        backtrack.writeDirectionToDictionary(currentVect, direction);

        //if -1 is passed, then the default first room is chosen
        if (previousRoomNumber != -1)
        {
            //selects room 0 and gets all relevant information
            var dirToExitList = getRoomInfo(previousRoomNumber - 1);
            //takes the newly stated list and gets the relevant peice of information from it. 
            dirToExit = dirToExitList[directionN];
            //call method, passing in currentVect and the other stuff.
            if (isBacktracking == true)
            {
                var vectToAdd = new Vector2(0, 0);
                if (directionN == 1) {
                    vectToAdd = new Vector2(-1, 0);
                } else if (directionN == 2) {
                    vectToAdd = new Vector2(1, 0);
                } else if (directionN == 3) {
                    vectToAdd = new Vector2(0, -1);
                } else if (directionN == 4) {
                    vectToAdd = new Vector2(0, 1);
                }

                //DeleteExitAt(currentVect + dirToExit + vectToAdd);
                storedExitCoords = currentVect + dirToExit + vectToAdd;
                Debug.Log("Exit added to delete later" + storedExitCoords);
                addObjectToStack(null);
                addObjectToStack(null);
                addObjectToStack(null);
            } else if (isFirstIteration == true || isBacktracking == false) {
                //only runs if not backtracking (e.g. first room) or if no other directions have been tried
                //Fixes bug that happened when a room tried a second direction but wasn't considered to be 'backtracking'
                closeExits(currentVect, dirToExitList, directionN, previousDirection);
            }
        }
        else
        {
            dirToExit = new Vector2(0, 0);
        }


        var randomHallwayCount = UnityEngine.Random.Range(2, 7);

        
        checkHallway(currentVect, dirToExit, direction, directionN, randomHallwayCount, previousDirection, previousRoomNumber);

        //spawnHallwayRandom(currentVect, dirToExit, direction);
    }

    void generationBacktrack(Vector2 currentVect, string previousDirection, int previousRoomNumber, bool isRandom)
    {
        System.Tuple<Vector2, string, int> backInfo;
        if (isRandom) {
            backInfo = backtrack.retrieveInformation(UnityEngine.Random.Range(1, recursions-1));
        } else {
            backInfo = backtrack.retrieveInformation(1);
        }
        isBacktracking = true;
        chooseHallwayRandom(backInfo.Item1, backInfo.Item2, backInfo.Item3, isBacktracking, isFirstIteration);
    }

    void checkHallway(Vector2 currentVect, Vector2 dirToExit, string direction, int directionN, int randomHallwayCount, string previousDirection, int previousRoomNumber)
    {
        
        var collisionCheck = hashTable.checkHallwaySpace(currentVect, direction, randomHallwayCount, dirToExit);
        if (collisionCheck == true)
        {
            deleteExits();
            //var backInfo = backtrack.retrieveInformation(0);
            triedArray[directionN - 1] = 1;
            //now while the 4th parameter (isBacktracking) is true, this si only to prevent another copy of the data being added to the backtrack system.
            isBacktracking = false;
            if (backtrack.checkFirstIteration(currentVect) == true)
            {
                backtrack.roomHasntSpawned(currentVect);
            }
            chooseHallwayRandom(currentVect, previousDirection, previousRoomNumber, isBacktracking, isFirstIteration);
        }
        else
        {
            spawnHallwayRandom(currentVect, dirToExit, direction, randomHallwayCount);
        }
    }

    void closeExits(Vector2 currentVect, List<Vector2> dirToExitList, int directionN, string previousDirection)
    {
        Debug.Log("Closing exits");
        var exitPrefab = Resources.Load<GameObject>("Exits/Exit");
        if (directionN != 1 && previousDirection != "left" )
        {
            var vectToClose = currentVect + dirToExitList[1] + new Vector2(-1, 0);
            exitPrefab = Resources.Load<GameObject>("Exits/Exit1");
            var exit = Instantiate(exitPrefab, vectToClose, Quaternion.identity, gameObject.transform);
            addObjectToStack(exit);
            
            backtrack.addExitObject(vectToClose, exit);
        }
        if (directionN != 2 && previousDirection != "right")
        {
            var vectToClose = currentVect + dirToExitList[2] + new Vector2(1, 0);
            exitPrefab = Resources.Load<GameObject>("Exits/Exit2");
            var exit = Instantiate(exitPrefab, vectToClose, Quaternion.identity, gameObject.transform);
            addObjectToStack(exit);

            backtrack.addExitObject(vectToClose, exit);
        }
        if (directionN != 3 && previousDirection != "down")
        {
            var vectToClose = currentVect + dirToExitList[3] + new Vector2(0, -1);
            exitPrefab = Resources.Load<GameObject>("Exits/Exit3");
            var exit = Instantiate(exitPrefab, vectToClose, Quaternion.identity, gameObject.transform);
            addObjectToStack(exit);

            backtrack.addExitObject(vectToClose, exit);
        }
        if (directionN != 4 && previousDirection != "up")
        {
            var vectToClose = currentVect + dirToExitList[4] + new Vector2(0, 1);
            exitPrefab = Resources.Load<GameObject>("Exits/Exit4");
            var exit = Instantiate(exitPrefab, vectToClose, Quaternion.identity, gameObject.transform);
            addObjectToStack(exit);

            backtrack.addExitObject(vectToClose, exit);
        }
    }

    void spawnHallwayRandom(Vector2 currentVect, Vector2 dirToExit, string direction, int randomHallwayCount)
    {

        hashTable.addHallway(currentVect, direction, randomHallwayCount);

        currentVect += new Vector2(dirToExit.x, dirToExit.y);
        Debug.Log("Hallways should start spawning at " + currentVect);

        if (direction == "up" || direction == "down")
        {
            for (var i = 0; i < randomHallwayCount; i++)
            {
                var Hallway = Instantiate(vCorridor, new Vector2(currentVect.x, currentVect.y + ((direction == "up" ? 1 : -1) * i)), Quaternion.identity, gameObject.transform);
                addObjectToStack(Hallway);
            }
            currentVect.y += (randomHallwayCount - 1) * (direction == "up" ? 1 : -1);
        }
        else
        {
            for (var i = 0; i < randomHallwayCount; i++)
            {
                var Hallway = Instantiate(hCorridor, new Vector2(currentVect.x + ((direction == "right" ? 1 : -1) * i), currentVect.y), Quaternion.identity, gameObject.transform);
                addObjectToStack(Hallway);
            }
            currentVect.x += (randomHallwayCount - 1) * (direction == "right" ? 1 : -1);
        }

        Debug.Log("CurrentVect after hallway: " + currentVect);



        //spawnRoomRandom(currentVect, dirToExit, direction, chooseRoomRandom()); //alternate code that skips the checkRoom method
        checkRoom(currentVect, dirToExit, direction, randomHallwayCount); 
    }

    // so when left is called, because the vectors aren't similar (room is split so 3 and 4 are the dirToExit vectors), the room only needs to spawn 2to the right rather than 4 as 6 (room width - dirToExit.x = 2)

    void checkRoom(Vector2 currentVect, Vector2 dirToExit, string direction, int randomHallwayCount)
    {
        var numberOfRoomsTried = 0;
        while (numberOfRoomsTried < 3)
        {
            var roomNumber = 0;
            if (numberOfRoomsTried == 0 || roomNumber < 3)
            {
                roomNumber = chooseRoomRandom();
            }
            else
            {
                roomNumber = roomNumber - 1;
            }

            var roomInformation = getRoomInfo(roomNumber - 1);
            var roomDimensions = roomInformation[0];

            var dirToCentre = new Vector2(0, 0);
            if (direction == "right")
            {
                dirToCentre = roomInformation[2];
            }
            else if (direction == "left")
            {
                dirToCentre = roomInformation[1];
            }
            else if (direction == "up")
            {
                dirToCentre = roomInformation[4];
            }
            else if (direction == "down")
            {
                dirToCentre = roomInformation[3];
            }
            dirToCentre *= -1;

            //check if room will collide with anything
            var collisionCheck = hashTable.checkRoomSpace(currentVect, roomDimensions, dirToCentre, direction);
            if (collisionCheck == false)
            {
                if (isBacktracking == true)
                {
                    //get info about room exit to open from earlier and open it now.
                    backtrack.removeExitObject(storedExitCoords);
                    Debug.Log("Exit deleted at" + storedExitCoords);
                }
                backtrack.roomHasSpawned(backtrack.retrieveInformation(0).Item1);
                spawnRoomRandom(currentVect, dirToExit, direction, roomNumber);

                return;
            }
            else
            {
                numberOfRoomsTried++;
            }
        }
        //currentVect, dirToExit, direction, randomHallwayCount
        Debug.Log("The size of the stack is " + lastObjectInstantiated.Count);
        deleteHallway(randomHallwayCount);
        Debug.Log("The size of stack after hallway deleted is" + lastObjectInstantiated.Count);
        deleteExits();
        Debug.Log("The size of the stack after exits deleted is " + lastObjectInstantiated.Count);
        var backInfo = backtrack.retrieveInformation(0);
        //stil don't know it this is false or true but false seems to work better D:
        isBacktracking = false;

        backtrack.roomHasntSpawned(backtrack.retrieveInformation(0).Item1);


        chooseHallwayRandom(backInfo.Item1, backInfo.Item2, backInfo.Item3, isBacktracking, isFirstIteration);
    }

    void spawnRoomRandom(Vector2 currentVect, Vector2 dirToExit, string direction, int roomNumber)
    {
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
            addObjectToStack(spawnedRoom);
        }
        else if (direction == "left")
        {
            currentVect += new Vector2(-(roomInformation[1]).x, -(roomInformation[1]).y);
            var spawnedRoom = Instantiate(roomVar, currentVect, Quaternion.identity, gameObject.transform);
            addObjectToStack(spawnedRoom);
        }
        else if (direction == "up")
        {
            // 6 - vect 2 = roomWidth - original vector
            currentVect += new Vector2(-(roomInformation[4]).x, -(roomInformation[4]).y);
            var spawnedRoom = Instantiate(roomVar, currentVect, Quaternion.identity, gameObject.transform);
            addObjectToStack(spawnedRoom);
        }
        else if (direction == "down")
        {
            currentVect += new Vector2(-(roomInformation[3]).x, -(roomInformation[3]).y);
            var spawnedRoom = Instantiate(roomVar, currentVect, Quaternion.identity, gameObject.transform);
            addObjectToStack(spawnedRoom);
        }

        Debug.Log("The CurrentVect of the the most recently installed room is " + currentVect);

        hashTable.addRoom(currentVect, roomDimensions);

        incrementRecursionCounter(currentVect, direction, roomNumber);
        //currentVect += getRoomInfo((direction == "right" || direction == "left") || (direction == "up" || direction == "down") ? ((direction == "right") ? 1 : 2) : ((direction == "up") ? 3 : 4))
    }

    int chooseRoomRandom()
    {
        //has to be manually added to increase number of rooms.
        var roomNumber = UnityEngine.Random.Range(1, 4);
        return roomNumber;
    }


    //Create an array of Vector2's using the array which is pre-made and return it from here
    List<Vector2> getRoomInfo(int roomNumber)
    {
        //array of rooms
        Vector2[,] roomArray = new Vector2[,]
        {
            //room dimensions, dirToRight, dirToLeft, dirToUp, dirToDown
            {new Vector2(6, 6),new Vector2(3, 0),new Vector2(-4, 0),new Vector2(0, 3),new Vector2(0, -4)},
            {new Vector2(8, 8),new Vector2(4, 0),new Vector2(-5, 0),new Vector2(0, 4),new Vector2(0, -5)},
            {new Vector2(12,14), new Vector2(6, 2), new Vector2(-7, 0), new Vector2(0, 7), new Vector2(0, -8)}
        };

        List<Vector2> roomInfoList = new List<Vector2>();

        for (var i = 0; i < 5; i++)
        {
            roomInfoList.Add(roomArray[roomNumber, i]);
        }

        return roomInfoList;
    }



    public Stack<GameObject> lastObjectInstantiated = new Stack<GameObject>();
    void addObjectToStack(GameObject objectAdd)
    {
        lastObjectInstantiated.Push(objectAdd);
    }
    void deleteHallway(int hallwayLength)
    {
        //hallway length + 3 as there are 3 exit covers that will need to be deleted.
        for (var i = 0; i < (hallwayLength); i++)
        {
            GameObject objectDestroy = lastObjectInstantiated.Pop();
            Destroy(objectDestroy);
        }
    }
    void deleteExits()
    {
        for (var i = 0; i < 2; i++)
        {
            GameObject objectDestroy = lastObjectInstantiated.Pop();
            Destroy(objectDestroy);
        }
    }
    void deleteRoom()
    {
        GameObject objectDestroy = lastObjectInstantiated.Pop();
        Destroy(objectDestroy);
    }
    void clearStack()
    {
        lastObjectInstantiated.Clear();
    }
    


    void incrementRecursionCounter(Vector2 currentVect, string direction, int previousRoomNumber)
    {
        Debug.Log("recursion " + recursions + " has been completed");
        recursions++;
        if (recursions >= targetRecursions)
        {
            Debug.Log("Algorithm stopping. Should be this many rooms: " + (targetRecursions));
            
            closeCurrentExits(currentVect, getRoomInfo(previousRoomNumber - 1), direction);

            var key = Resources.Load<GameObject>("Key " + pathNumber);
            var spawnKey = Instantiate(key, currentVect, Quaternion.identity, gameObject.transform);
            Debug.Log("Closing final room exits. Fianl room at: " + currentVect);
            recursions = targetRecursions + 1000;
            stopRecursions();
        }
        else
        {
            Debug.Log("Room " + (recursions) + "/" + (targetRecursions) + "spawned");
            isBacktracking = false;
            chooseHallwayRandom(currentVect, direction, previousRoomNumber, isBacktracking, isFirstIteration);
        }
    }

    void stopRecursions()
    {
        recursions = targetRecursions + 1000;
        int count = hashTable.hashGrid.Count;
        Debug.Log("The number of hashTable entries is " + count);
        backtrack.printAllTuples();
    }

    void closeCurrentExits(Vector2 currentVect, List<Vector2> dirToExitList, string previousDirection)
    {
        var exitPrefab = Resources.Load<GameObject>("Exits/Exit");
        if (previousDirection != "left")
        {
            var vectToClose = currentVect + dirToExitList[1] + new Vector2(-1, 0);
            exitPrefab = Resources.Load<GameObject>("Exits/Exit1");
            var exit = Instantiate(exitPrefab, vectToClose, Quaternion.identity, gameObject.transform);
            // no need for addObjectToStack(exit); as the exit is not part of the path and the objects will not need to be deleted
            backtrack.addExitObject(vectToClose, exit);
        }
        if (previousDirection != "right")
        {
            var vectToClose = currentVect + dirToExitList[2] + new Vector2(1, 0);
            exitPrefab = Resources.Load<GameObject>("Exits/Exit2");
            var exit = Instantiate(exitPrefab, vectToClose, Quaternion.identity, gameObject.transform);
            backtrack.addExitObject(vectToClose, exit);
            
        }
        if (previousDirection != "down")
        {
            var vectToClose = currentVect + dirToExitList[3] + new Vector2(0, -1);
            exitPrefab = Resources.Load<GameObject>("Exits/Exit3");
            var exit = Instantiate(exitPrefab, vectToClose, Quaternion.identity, gameObject.transform);
            backtrack.addExitObject(vectToClose, exit);
            
        }
        if (previousDirection != "up")
        {
            var vectToClose = currentVect + dirToExitList[4] + new Vector2(0, 1);
            exitPrefab = Resources.Load<GameObject>("Exits/Exit4");
            var exit = Instantiate(exitPrefab, vectToClose, Quaternion.identity, gameObject.transform);
            backtrack.addExitObject(vectToClose, exit);
            
        }
    }

    void resetAlgorithm()
    {
        Debug.Log("Algorithm reset");
        //delete all objects, stop the program and restart from the start()
        hashTable.clearHashTable();
        backtrack.clearBacktrack();
        //resetTriedArray();
        clearStack();
        recursions = 0;
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

