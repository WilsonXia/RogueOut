using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapData : MonoBehaviour
{
    // Floor Data
    // - Generation Rules
    // - Types of Enemies you might encounter
    // - Boss Enemy
    // - Types of Events you might encounter
    // - Treasure
    // - Map Details (Rooms, guaranteed floor encounters maybe)

    public string mapName;
    public int numberOfRooms;
    public List<GameObject> encounterableEnemies;
    //public List<Room> roomParts;

    public PlayerController player;
    public GameObject[] rooms;
    public GameObject currentRoom;
    Color originalRoomColor;

    public RoomDetailBox rddetailBox;
    protected VisualManager visualManager;
    // Custom Generation
    protected MapGenerator mapGenerator;

    // Properties
    public Room CurrentRoom { get { return currentRoom.GetComponent<Room>(); } }

    protected virtual void Start()
    {
        player = GetComponent<PlayerController>();
        visualManager = GetComponent<VisualManager>();
        GetComponent<MapController>().SetUp(player, this);
        SetUp();

        mapGenerator.StartGeneration();
        rddetailBox.SetUp(currentRoom);
    }

    protected virtual void SetUp() { }

    public virtual void GenerateMap() { }

    protected void Update()
    {
        if(currentRoom != null)
            visualManager.SingleSelectedAnimation(currentRoom);
    }

    public void ReceiveMap(GameObject[] rooms)
    {
        this.rooms = rooms;
        currentRoom = rooms[0];
        originalRoomColor = currentRoom.GetComponent<SpriteRenderer>().color;
    }
    public void SendEnemyData(GameObject room)
    {
        List<GameObject> returnedEnemies = new List<GameObject>();
        int numOfEnemies = room.GetComponent<EnemyRoom>().numberOfEnemies;
        for (int i = 0; i < numOfEnemies; i++)
        {
            int index = Random.Range(0, encounterableEnemies.Count);
            returnedEnemies.Add(encounterableEnemies[index]);
        }
        room.GetComponent<EnemyRoom>().SetEnemies(returnedEnemies);
    }

    #region Map Movement
    public void MoveRoom(int direction)
    {
        // Reset the color of the current room
        // change room
        // Save color
        if(originalRoomColor != null)
        {
            currentRoom.GetComponent<SpriteRenderer>().color = originalRoomColor;
        }
        switch (direction)
        {
            case 2:
                currentRoom = CurrentRoom.up.gameObject;
                break;
            case 1:
                currentRoom = CurrentRoom.right.gameObject;
                break;
            case -2:
                currentRoom = CurrentRoom.down.gameObject;
                break;
            case -1:
                currentRoom = CurrentRoom.left.gameObject;
                break;
        }
        rddetailBox.SetUp(currentRoom);
        originalRoomColor = currentRoom.GetComponent<SpriteRenderer>().color;
    }
    #endregion
}
