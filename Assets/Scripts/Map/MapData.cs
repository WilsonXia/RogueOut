using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public GameObject[] rooms;
    public GameObject currentRoom;
    GameObject selectedRoom;
    Color originalRoomColor;
    
    public AudioSource audioSource;
    protected VisualManager visualManager;
    protected MapGenerator mapGenerator;
    public bool changeFlag;

    public static MapData instance;

    // Properties
    public Room CurrentRoom { get { return currentRoom.GetComponent<Room>(); } }
    public Room SelectedRoom { get { return selectedRoom.GetComponent<Room>(); } }

    protected void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    protected virtual void Start()
    {
        audioSource = GetComponent<AudioSource>();
        
        if (GameData.instance.map == null)
        {
            visualManager = GetComponent<VisualManager>();
            GetComponent<MapController>().SetUp(GameData.instance.PlayerController, this);
            SetUp();

            mapGenerator.StartGeneration();
            GameData.instance.map = this;
        }
        DontDestroyOnLoad(gameObject);
    }
    protected void Update()
    {
        if (selectedRoom != null)
        {
            visualManager.SingleSelectedAnimation(selectedRoom);
        }
    }

    protected virtual void SetUp() { }

    public virtual void GenerateMap() { }

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
        audioSource.Play();
        if(selectedRoom != null)
        {
            selectedRoom.GetComponent<SpriteRenderer>().color = originalRoomColor;
        }
        switch (direction)
        {
            case 2:
                selectedRoom = CurrentRoom.up.gameObject;
                break;
            case 1:
                selectedRoom = CurrentRoom.right.gameObject;
                break;
            case -2:
                selectedRoom = CurrentRoom.down.gameObject;
                break;
            case -1:
                selectedRoom = CurrentRoom.left.gameObject;
                break;
        }
        changeFlag = true;
        originalRoomColor = selectedRoom.GetComponent<SpriteRenderer>().color;
    }
    public void SelectRoom()
    {
        currentRoom = selectedRoom;
        Room room = currentRoom.GetComponent<Room>();
        // Transition scenes based on room type
        switch (room.type)
        {
            case RoomType.Enemy:
                GameData.instance.ChangeState(GameState.Battle);
                SceneManager.LoadScene("Battle");
                break;
            default:
                break;
        }
    }
    #endregion
}
