using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapGenerator : MonoBehaviour
{
    MapData mapData;
    ArrowPlacer arrowPlacer;
    // Kinds of Rooms
    public List<GameObject> roomPrefabs;
    // Starting Pos
    public Vector2 startingPosition;
    // Spacing
    public Vector2 spacing;
    public float roomAvoidRadius;
    // Number of Rooms
    int numberOfRooms;
    GameObject[] rooms;
    int[,] adjacencyMatrix;

    public bool useDebug;
    public bool useDefault;

    public GameObject[] Rooms { get { return rooms; } }

    private void Start()
    {
        // Obtain map data
        ObtainMapData();
        arrowPlacer = GetComponent<ArrowPlacer>();
    }
    #region Starting Functions
    void ObtainMapData()
    {
        mapData = GetComponent<MapData>();
        numberOfRooms = mapData.numberOfRooms + 1;
    }
    void GenerateRooms()
    {
        rooms = new GameObject[numberOfRooms];
        rooms[0] = CreateRoom(RoomType.Start);
        for (int i = 1; i < numberOfRooms; i++)
        {
            rooms[i] = CreateRandomRoom();
            rooms[i].transform.position = new Vector3(0, 0);
            rooms[i].name = rooms[i].name + i;
        }
    }
    public void StartGeneration()
    {
        if (useDefault)
        {
            GenerateRooms();
            GenerateMap();
        }
        else
        {
            mapData.GenerateMap();
        }
    }
    public void GenerateMap()
    {
        // Create Edges
        StructureSetup();
        // Send Feedback to MapData
        mapData.ReceiveMap(rooms);
    }
    /// <summary>
    /// Custom generation using MapData
    /// </summary>
    /// <param name="roomsMade"></param>
    public void ReceiveRooms(GameObject[] roomsMade)
    {
        rooms = roomsMade;
        numberOfRooms = rooms.Length;
    }
    #endregion

    #region Edge Creation
    void StructureSetup()
    {
        // Creating Adjacency Matrix
        adjacencyMatrix = new int[numberOfRooms, numberOfRooms];
        for (int i = 0; i < numberOfRooms; i++)
        {
            for (int j = 0; j < numberOfRooms; j++)
            {
                adjacencyMatrix[i, j] = 0;
            }
        }
        // Connect the starter to 2 different rooms
        CreateEdge(0, 1);
        CreateEdge(0, 2);
        // Then set the visited rooms to go with other unvisited rooms
        DistributeRemainingEdges();
    }
    public void ChainStructure()
    {
        // Creating Adjacency Matrix
        adjacencyMatrix = new int[numberOfRooms, numberOfRooms];
        for (int i = 0; i < numberOfRooms; i++)
        {
            for (int j = 0; j < numberOfRooms; j++)
            {
                adjacencyMatrix[i, j] = 0;
            }
        }
        for(int i = 0; i < numberOfRooms; i++)
        {
            //Connect room to next room
            if (i+1 < numberOfRooms)
            {
                CreateEdge(i, i + 1, true);
            }
        }
    }
    void DistributeRemainingEdges()
    {
        int index = 3;
        int step = 3;
        int count = 0;
        do
        {
            DistributeEdges(index, step, 1, 2);
            index += step;
            count++;
        }
        while (rooms.Length - index >= step && count < 2);
        for (int i = 0; i < numberOfRooms; i++)
        {
            Room room = rooms[i].GetComponent<Room>();
            if (!room.visited)
            {
                int randomIndex = Random.Range(3, index);
                CreateEdge(randomIndex, i);
                //Debug.Log("Random Index led to -> " + rooms[randomIndex].name);
            }
        }
    }
    void DistributeEdges(int startingIndex, int totalStep, int edgeA, int edgeB)
    {
        for (int i = startingIndex; i < startingIndex + totalStep; i++)
        {
            if (i % 2 == 0)
            {
                CreateEdge(edgeA, i);
            }
            else
            {
                CreateEdge(edgeB, i);
            }
        }
    }
    void CreateEdge(int roomA, int roomB, bool useRandom = false)
    {
        adjacencyMatrix[roomA, roomB] = 1;
        rooms[roomA].GetComponent<Room>().visited = true;
        rooms[roomB].GetComponent<Room>().visited = true;
        PairRooms(roomA, roomB, useRandom);
    }
    #endregion

    #region Room Creation
    GameObject CreateRandomRoom()
    {
        RoomType rType;
        // Assign a Room based on chance
        float chance = Random.value;
        if (chance <= 0.1f)
        {
            // Treasure Room
            rType = RoomType.Treasure;
        }
        else if (chance <= 0.3f)
        {
            // Event Room
            rType = RoomType.Event;
        }
        else
        {
            // Enemy Room
            rType = RoomType.Enemy;
        }
        return CreateRoom(rType);
    }
    public GameObject CreateRoom(RoomType rType)
    {
        GameObject newRoom;
        switch (rType)
        {
            case RoomType.Enemy:
                newRoom = roomPrefabs[3];
                break;
            case RoomType.Treasure:
                newRoom = roomPrefabs[1];
                break;
            case RoomType.Event:
                newRoom = roomPrefabs[2];
                break;
            default:
                newRoom = roomPrefabs[0];
                newRoom.transform.position = startingPosition;
                break;
        }
        GameObject room = Instantiate(newRoom);
        room.name = string.Format("{0} ", newRoom.GetComponent<Room>().type);

        // Room Specific Changes
        if (room.GetComponent<Room>() is EnemyRoom)
        {
            EnemyRoom eRoom = room.GetComponent<EnemyRoom>();
            eRoom.numberOfEnemies = Random.Range(1, 3 + 1);
            mapData.SendEnemyData(room);
        }
        room.transform.parent = transform;
        return room;
    }
    void DeleteRoom(int index)
    {
        for(int i = 0; i < numberOfRooms; i++)
        {
            adjacencyMatrix[index, i] = 0;
            adjacencyMatrix[i, index] = 0;
        }
        Destroy(rooms[index].gameObject);
    }
    #endregion

    bool IsOverlapping(Room roomToCheck, Vector3 pos)
    {
        bool verdict = false;
        foreach (GameObject room in rooms)
        {
            if (room != null)
            {
                float sqrDistance = Vector2.SqrMagnitude(room.transform.position - pos);
                if (room.GetComponent<Room>() != roomToCheck && sqrDistance < Mathf.Pow(roomAvoidRadius * 2, 2))
                {
                    verdict = true;
                }
            }
        }
        return verdict;
    }

    #region Pairing Functions
    void PairRooms(int roomA, int roomB, bool useRandom)
    {
        if (!useRandom)
        {
            PairRooms(roomA, roomB);
        }
        else
        {
            Room a = rooms[roomA].GetComponent<Room>();
            Room b = rooms[roomB].GetComponent<Room>();
            if (roomA % 2 == 0)
            {
                PairUpDown(a, b);
            }
            else
            {
                PairRightLeft(a, b);
            }
        }
    }
    void PairRooms(int roomA, int roomB)
    {
        Room a = rooms[roomA].GetComponent<Room>();
        Room b = rooms[roomB].GetComponent<Room>();
        bool successfullyPaired = false;
        int errorCount = 0;

        while (!successfullyPaired)
        {
            float chance = Random.value;
            if (chance < 0.25f)
            {
                // Pair Left
                if (a.left == null && !IsOverlapping(a, a.Position + Vector2.left * a.avoidRadius))
                {
                    PairLeftRight(a, b);
                    successfullyPaired = true;
                }
            }
            else if (chance < 0.5f)
            {
                // Pair Up
                if (a.up == null && !IsOverlapping(a, a.Position + Vector2.up * a.avoidRadius))
                {
                    PairUpDown(a, b);
                    successfullyPaired = true;
                }
            }
            else if (chance < 0.75f)
            {
                // Pair Right
                if (a.right == null && !IsOverlapping(a, a.Position + Vector2.right * a.avoidRadius))
                {
                    PairRightLeft(a, b);
                    successfullyPaired = true;
                }
            }
            else
            {
                // Pair Down
                if (a.down == null && !IsOverlapping(a, a.Position + Vector2.down * a.avoidRadius))
                {
                    PairDownUp(a, b);
                    successfullyPaired = true;
                }
            }
            if (errorCount > 5)
            {
                successfullyPaired = true;
                Debug.Log("Looped too long, deleting room");
                DeleteRoom(roomB);
            }
            errorCount++;
        }
    }
    void PairUpDown(Room baseRoom, Room attatchingRoom)
    {
        baseRoom.up = attatchingRoom;
        //attatchingRoom.down = baseRoom;
        attatchingRoom.MoveAbove(baseRoom);

        // Attatch the arrow above the baseRoom
        GameObject arrow = arrowPlacer.PlaceArrowUp(baseRoom.Position + new Vector2(0, spacing.y));
        arrow.transform.parent = baseRoom.transform;
    }
    void PairDownUp(Room baseRoom, Room attatchingRoom)
    {
        baseRoom.down = attatchingRoom;
        //attatchingRoom.up = baseRoom;
        attatchingRoom.MoveBelow(baseRoom);
        // Attatch the arrow below the baseRoom
        GameObject arrow = arrowPlacer.PlaceArrowDown(baseRoom.Position - new Vector2(0, spacing.y));
        arrow.transform.parent = baseRoom.transform;
    }
    void PairLeftRight(Room baseRoom, Room attatchingRoom)
    {
        baseRoom.left = attatchingRoom;
        //attatchingRoom.right = baseRoom;
        attatchingRoom.MoveLeft(baseRoom);
        // Attatch the arrow Left the baseRoom
        GameObject arrow = arrowPlacer.PlaceArrowLeft(baseRoom.Position - new Vector2(spacing.x, 0));
        arrow.transform.parent = baseRoom.transform;
    }
    void PairRightLeft(Room baseRoom, Room attatchingRoom)
    {
        baseRoom.right = attatchingRoom;
        //attatchingRoom.left = baseRoom;
        attatchingRoom.MoveRight(baseRoom);
        // Attatch the arrow Right the baseRoom
        GameObject arrow = arrowPlacer.PlaceArrowRight(baseRoom.Position + new Vector2(spacing.x, 0));
        arrow.transform.parent = baseRoom.transform;
    }
    #endregion
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        if (rooms != null)
        {
            for (int i = 0; i < rooms.Length; i++)
            {
                if(rooms[i] != null)
                {
                    GameObject iObject = rooms[i].gameObject;
                    if (useDebug)
                    {
                        if (IsOverlapping(iObject.GetComponent<Room>(), iObject.transform.position))
                        {
                            Gizmos.color = Color.red;
                        }
                        else
                        {
                            Gizmos.color = Color.yellow;
                        }
                        Gizmos.DrawWireSphere(iObject.transform.position, roomAvoidRadius);
                    }
                    for (int j = 0; j < rooms.Length; j++)
                    {
                        GameObject jObject = rooms[j].gameObject;
                        if (adjacencyMatrix[i, j] == 1)
                        {
                            Gizmos.color = Color.black;
                            Gizmos.DrawLine(iObject.transform.position, jObject.transform.position);
                        }
                    }
                }
            }
        }
    }

    //Vector2 CreateFilteredPosition()
    //{
    //    Vector2 pos;
    //    do
    //    {
    //        pos = GetRandomPosition();
    //    }
    //    while (IsOverlapping(pos));
    //    return pos;
    //}
    //bool IsOverlapping(Vector3 pos)
    //{
    //    bool verdict = false;
    //    foreach(GameObject room in rooms)
    //    {
    //        if(room != null)
    //        {
    //            float sqrDistance = Vector2.SqrMagnitude(room.transform.position - pos);
    //            if (!(sqrDistance < Mathf.Epsilon) && sqrDistance < Mathf.Pow(roomAvoidRadius * 2, 2))
    //            {
    //                verdict = true;
    //                if (useDebug)
    //                    Debug.Log(string.Format("{0} at {1}, {2} \nsqrDistance: {3}  sqrRadiiSum: {4}",
    //                        room.name, pos.x, pos.y, sqrDistance, Mathf.Pow(roomAvoidRadius * 2, 2)));
    //            }
    //        }
    //    }
    //    return verdict;
    //}
    //Vector2 GetRandomPosition()
    //{
    //    float x = Mathf.Round(Random.Range(-4f, 4f));
    //    float y = Mathf.Round(Random.Range(-3f, 3f));
    //    return new Vector2(x, y);
    //}
}
