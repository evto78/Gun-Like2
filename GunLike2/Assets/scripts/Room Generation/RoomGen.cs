using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGen : MonoBehaviour
{
    public GameObject startRoom;
    public GameObject endRoom;
    GameObject finalEndRoom;
    public List<GameObject> rooms;
    public List<GameObject> openRooms;

    List<GameObject> generatedRooms;
    List<GameObject> sideDoors;

    public int mainRooms;
    int mainRoomsMade;
    public int sideRooms;
    int sideRoomsMade;
    List<int> prevRoomsId;

    int attemptsMade;
    public int maxAttempts;

    public bool done;

    // Start is called before the first frame update
    void Start()
    {
        done = false;

        mainRoomsMade = 0;
        sideRoomsMade = 0;

        sideDoors = new List<GameObject>();
        prevRoomsId = new List<int>();
        generatedRooms = new List<GameObject>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && !done) 
        {
            Generate();
        }
    }

    public void Generate()
    {
        attemptsMade = 1;

        GenerateFirst();

        while (CheckForOverlap() && attemptsMade <= maxAttempts)
        {
            attemptsMade++;
            foreach (GameObject genRoom in generatedRooms)
            {
                Destroy(genRoom);
            }
            generatedRooms.Clear();
            prevRoomsId.Clear();
            sideDoors.Clear();
            mainRoomsMade = 0;
            sideRoomsMade = 0;

            GenerateFirst();
        }

        for(int i = 0; i < sideDoors.Count; i++)
        {
            Destroy(sideDoors[i]);
        }
        sideDoors.Clear();

        GenerateOpen();

        if(attemptsMade >= maxAttempts) { Debug.Log("Could not find a layout in " + attemptsMade + " Attempts."); }
        else { Debug.Log("Done after " + attemptsMade + " Attempts."); }

        done = true;
    }

    bool CheckForOverlap()
    {
        List<BoxCollider> colliders = new List<BoxCollider>();
        BoxCollider checkingCollider;
        for (int i = 0; i < generatedRooms.Count; i++)
        {
            colliders.Add(generatedRooms[i].GetComponent<Room>().interCollider);
        }
        colliders.Add(startRoom.GetComponent<Room>().interCollider);
        for (int i = 0; i < colliders.Count; i++)
        {
            checkingCollider = colliders[i];
            for(int y = 0; y < colliders.Count; y++)
            {
                if(i != y)
                {
                    Physics.SyncTransforms();
                    if (colliders[i].bounds.Intersects(colliders[y].bounds))
                    {
                        return true;
                    }
                }
            }
        }   

        return false;
    }

    void GenerateFirst()
    {
        int rand = Random.Range(0,rooms.Count);

        //spawn the room
        GameObject spawnedRoom = Instantiate(rooms[rand]);
        spawnedRoom.name = "MAIN"+mainRoomsMade;
        spawnedRoom.transform.parent = startRoom.GetComponent<Room>().doors[0].transform;
        spawnedRoom.transform.localPosition = Vector3.zero;
        spawnedRoom.transform.localEulerAngles = Vector3.zero;
        spawnedRoom.GetComponent<Room>().isMainPath = true;
        prevRoomsId.Add(rand);
        //orient the room
        rand = Random.Range(0, spawnedRoom.GetComponent<Room>().doors.Count);
        spawnedRoom.transform.GetChild(0).Translate(-spawnedRoom.GetComponent<Room>().doors[rand].transform.localPosition);
        spawnedRoom.transform.Rotate(0f, -spawnedRoom.GetComponent<Room>().doors[rand].transform.localEulerAngles.y, 0f);
        spawnedRoom.transform.Rotate(0f, 180f, 0f);
        Destroy(spawnedRoom.GetComponent<Room>().doors[rand]);
        spawnedRoom.GetComponent<Room>().doors.Remove(spawnedRoom.GetComponent<Room>().doors[rand]);
        spawnedRoom.transform.parent = null;

        //add the room to the lists
        generatedRooms.Add(spawnedRoom);

        mainRoomsMade++;
        GenerateMain(spawnedRoom);
    }
    void GenerateMain(GameObject prevRoom)
    {
        int rand = Random.Range(0, rooms.Count);
        //if we allready used this room, use the next unused room instead
        if (prevRoomsId.Contains(rand))
        {
            for (int i = 0; i < rooms.Count; i++)
            {
                if (prevRoomsId.Contains(rand))
                {
                    rand++;
                    if (rand >= rooms.Count) { rand = 0; }
                }
            }
        }
        
        //spawn the room
        GameObject spawnedRoom = Instantiate(rooms[rand]);
        spawnedRoom.name = "MAIN" + mainRoomsMade;
        spawnedRoom.transform.parent = prevRoom.GetComponent<Room>().doors[0].transform;
        spawnedRoom.transform.localPosition = Vector3.zero;
        spawnedRoom.transform.localEulerAngles = Vector3.zero;
        spawnedRoom.GetComponent<Room>().isMainPath = true;
        prevRoomsId.Add(rand);
        Destroy(prevRoom.GetComponent<Room>().doors[0]);
        prevRoom.GetComponent<Room>().doors.Remove(prevRoom.GetComponent<Room>().doors[0]);
        //orient the room
        rand = Random.Range(0, spawnedRoom.GetComponent<Room>().doors.Count);
        spawnedRoom.transform.GetChild(0).Translate(-spawnedRoom.GetComponent<Room>().doors[rand].transform.localPosition);
        spawnedRoom.transform.Rotate(0f, -spawnedRoom.GetComponent<Room>().doors[rand].transform.localEulerAngles.y, 0f);
        spawnedRoom.transform.Rotate(0f, 180f, 0f);
        Destroy(spawnedRoom.GetComponent<Room>().doors[rand]);
        spawnedRoom.GetComponent<Room>().doors.Remove(spawnedRoom.GetComponent<Room>().doors[rand]);
        spawnedRoom.transform.parent = null;

        //add the room to the list
        generatedRooms.Add(spawnedRoom);

        mainRoomsMade++;
        if (mainRoomsMade < mainRooms) { GenerateMain(spawnedRoom); }
        else
        {
            //spawn the room
            GameObject spawnedEndRoom = Instantiate(endRoom);
            spawnedEndRoom.transform.parent = spawnedRoom.GetComponent<Room>().doors[0].transform;
            spawnedEndRoom.transform.localPosition = Vector3.zero;
            spawnedEndRoom.transform.localEulerAngles = Vector3.zero;
            spawnedEndRoom.GetComponent<Room>().isMainPath = true;
            Destroy(spawnedRoom.GetComponent<Room>().doors[0]);
            spawnedRoom.GetComponent<Room>().doors.Remove(spawnedRoom.GetComponent<Room>().doors[0]);
            //orient the room
            rand = Random.Range(0, spawnedEndRoom.GetComponent<Room>().doors.Count);
            spawnedEndRoom.transform.Translate(spawnedEndRoom.GetComponent<Room>().doors[rand].transform.localPosition);
            spawnedEndRoom.transform.Rotate(0f, -spawnedEndRoom.GetComponent<Room>().doors[rand].transform.localEulerAngles.y, 0f);
            spawnedEndRoom.transform.Rotate(0f, 180f, 0f);
            Destroy(spawnedEndRoom.GetComponent<Room>().doors[rand]);
            spawnedEndRoom.GetComponent<Room>().doors.Remove(spawnedEndRoom.GetComponent<Room>().doors[rand]);
            spawnedEndRoom.transform.parent = null;

            finalEndRoom = spawnedEndRoom;

            //add the room to the list
            generatedRooms.Add(spawnedEndRoom);

            //begin generateing side rooms
            GenerateSideSetup();
        }
    }
    void GenerateSideSetup()
    {
        //collect potential side doors
        foreach(GameObject room in generatedRooms)
        {
            if (room.GetComponent<Room>().doors.Count > 0)
            {
                for (int i = 0; i < room.GetComponent<Room>().doors.Count; i++)
                {
                    GameObject doorFound = room.GetComponent<Room>().doors[i];
                    sideDoors.Add(doorFound);
                }
            }
        }

        if(sideRooms > 0)
        {
            GenerateSide(sideDoors[Random.Range(0, sideDoors.Count)]);
        }
    }
    void GenerateSide(GameObject prevDoor)
    {
        int rand = Random.Range(0, rooms.Count);
        //if we allready used this room, use the next unused room instead
        if (prevRoomsId.Contains(rand))
        {
            for (int i = 0; i < rooms.Count; i++)
            {
                if (prevRoomsId.Contains(rand))
                {
                    rand++;
                    if (rand >= rooms.Count) { rand = 0; }
                }
            }
        }

        //spawn the room
        GameObject spawnedRoom = Instantiate(rooms[rand]);
        spawnedRoom.name = "SIDE" + sideRoomsMade;
        spawnedRoom.transform.parent = prevDoor.transform;
        spawnedRoom.transform.localPosition = Vector3.zero;
        spawnedRoom.transform.localEulerAngles = Vector3.zero;
        spawnedRoom.GetComponent<Room>().isMainPath = false;
        prevRoomsId.Add(rand);
        Destroy(prevDoor);
        sideDoors.Remove(prevDoor);
        //orient the room
        rand = Random.Range(0, spawnedRoom.GetComponent<Room>().doors.Count);
        spawnedRoom.transform.GetChild(0).Translate(-spawnedRoom.GetComponent<Room>().doors[rand].transform.localPosition);
        spawnedRoom.transform.Rotate(0f, -spawnedRoom.GetComponent<Room>().doors[rand].transform.localEulerAngles.y, 0f);
        spawnedRoom.transform.Rotate(0f, 180f, 0f);
        Destroy(spawnedRoom.GetComponent<Room>().doors[rand]);
        spawnedRoom.GetComponent<Room>().doors.Remove(spawnedRoom.GetComponent<Room>().doors[rand]);
        spawnedRoom.transform.parent = null;

        //add the room to the list
        generatedRooms.Add(spawnedRoom);

        //add the rooms doors (if any) to the potential side doors
        if(spawnedRoom.GetComponent<Room>().doors.Count > 0)
        {
            for (int i = 0; i < spawnedRoom.GetComponent<Room>().doors.Count; i++)
            {
                sideDoors.Add(spawnedRoom.GetComponent<Room>().doors[i]);
            }
        }
        

        sideRoomsMade++;
        if (sideRoomsMade < sideRooms) 
        { 
            if(sideDoors.Count > 0) { GenerateSide(sideDoors[Random.Range(0, sideDoors.Count)]); }
        }
    }

    void GenerateOpen()
    {
        int rand = Random.Range(0, openRooms.Count);

        //spawn the room
        GameObject spawnedRoom = Instantiate(openRooms[rand]);
        spawnedRoom.name = "OPEN"+rand;
        spawnedRoom.transform.parent = finalEndRoom.transform;
        spawnedRoom.transform.localPosition = Vector3.zero;
        spawnedRoom.transform.localEulerAngles = Vector3.zero;
        spawnedRoom.GetComponent<Room>().isMainPath = true;
        //orient the room
        rand = Random.Range(0, spawnedRoom.GetComponent<Room>().doors.Count);
        spawnedRoom.transform.GetChild(0).Translate(-spawnedRoom.GetComponent<Room>().doors[rand].transform.localPosition);
        spawnedRoom.transform.Rotate(0f, spawnedRoom.GetComponent<Room>().doors[rand].transform.localEulerAngles.y, 0f);
        spawnedRoom.transform.Rotate(0f, 180f, 0f);
        Destroy(spawnedRoom.GetComponent<Room>().doors[rand]);
        spawnedRoom.GetComponent<Room>().doors.Remove(spawnedRoom.GetComponent<Room>().doors[rand]);
        spawnedRoom.transform.parent = null;

        //spawn next START room
        //spawn the room
        GameObject spawnedStartRoom = Instantiate(startRoom);
        spawnedStartRoom.name = "START";
        spawnedStartRoom.transform.parent = spawnedRoom.GetComponent<Room>().doors[0].transform;
        spawnedStartRoom.transform.localPosition = Vector3.zero;
        spawnedStartRoom.transform.localEulerAngles = Vector3.zero;
        spawnedStartRoom.GetComponent<Room>().isMainPath = true;
        //orient the room
        Destroy(spawnedRoom.GetComponent<Room>().doors[0]);
        spawnedRoom.GetComponent<Room>().doors.Remove(spawnedRoom.GetComponent<Room>().doors[0]);
        spawnedStartRoom.transform.parent = null;

    }
}
