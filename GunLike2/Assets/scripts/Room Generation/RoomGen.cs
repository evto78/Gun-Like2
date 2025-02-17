using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGen : MonoBehaviour
{
    public GameObject startRoom;
    public GameObject endRoom;
    public List<GameObject> rooms;

    public int mainRooms;
    int mainRoomsMade;
    public int sideRooms;
    int sideRoomsMade;
    // Start is called before the first frame update
    void Start()
    {
        GenerateFirst();
    }
    public void GenerateFirst()
    {
        int rand = Random.Range(0,rooms.Count);

        //spawn the room
        GameObject spawnedRoom = Instantiate(rooms[rand]);
        spawnedRoom.transform.parent = startRoom.GetComponent<Room>().doors[0].transform;
        spawnedRoom.transform.localPosition = Vector3.zero;
        spawnedRoom.transform.localEulerAngles = Vector3.zero;
        spawnedRoom.GetComponent<Room>().isMainPath = true;
        //orient the room
        rand = Random.Range(0, spawnedRoom.GetComponent<Room>().doors.Count);
        spawnedRoom.transform.GetChild(0).Translate(-spawnedRoom.GetComponent<Room>().doors[rand].transform.localPosition);
        spawnedRoom.transform.Rotate(0f, -spawnedRoom.GetComponent<Room>().doors[rand].transform.localEulerAngles.y, 0f);
        spawnedRoom.transform.Rotate(0f, 180f, 0f);
        spawnedRoom.GetComponent<Room>().doors.Remove(spawnedRoom.GetComponent<Room>().doors[rand]);
        spawnedRoom.transform.parent = null;

        GenerateMain(spawnedRoom);
    }
    void GenerateMain(GameObject prevRoom)
    {
        int rand = Random.Range(0, rooms.Count);

        //spawn the room
        GameObject spawnedRoom = Instantiate(rooms[rand]);
        spawnedRoom.transform.parent = prevRoom.GetComponent<Room>().doors[0].transform;
        spawnedRoom.transform.localPosition = Vector3.zero;
        spawnedRoom.transform.localEulerAngles = Vector3.zero;
        spawnedRoom.GetComponent<Room>().isMainPath = true;
        //orient the room
        rand = Random.Range(0, spawnedRoom.GetComponent<Room>().doors.Count);
        spawnedRoom.transform.GetChild(0).Translate(-spawnedRoom.GetComponent<Room>().doors[rand].transform.localPosition);
        spawnedRoom.transform.Rotate(0f, -spawnedRoom.GetComponent<Room>().doors[rand].transform.localEulerAngles.y, 0f);
        spawnedRoom.transform.Rotate(0f, 180f, 0f);
        spawnedRoom.GetComponent<Room>().doors.Remove(spawnedRoom.GetComponent<Room>().doors[rand]);
        spawnedRoom.transform.parent = null;

        mainRoomsMade++;
        if (mainRoomsMade <= mainRooms) { GenerateMain(spawnedRoom); }
        else
        {
            //spawn the room
            GameObject spawnedEndRoom = Instantiate(endRoom);
            spawnedEndRoom.transform.parent = spawnedRoom.GetComponent<Room>().doors[0].transform;
            spawnedEndRoom.transform.localPosition = Vector3.zero;
            spawnedEndRoom.transform.localEulerAngles = Vector3.zero;
            spawnedEndRoom.GetComponent<Room>().isMainPath = true;
            //orient the room
            rand = Random.Range(0, spawnedEndRoom.GetComponent<Room>().doors.Count);
            spawnedEndRoom.transform.GetChild(0).Translate(-spawnedEndRoom.GetComponent<Room>().doors[rand].transform.localPosition);
            spawnedEndRoom.transform.Rotate(0f, -spawnedEndRoom.GetComponent<Room>().doors[rand].transform.localEulerAngles.y, 0f);
            spawnedEndRoom.transform.Rotate(0f, 180f, 0f);
            spawnedEndRoom.GetComponent<Room>().doors.Remove(spawnedEndRoom.GetComponent<Room>().doors[rand]);
            spawnedEndRoom.transform.parent = null;
        }
    }
}
