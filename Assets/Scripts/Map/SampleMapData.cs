using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleMapData : MapData
{
    // Guaranteed Rooms - Quantity
    int treasureRooms;
    int eventRooms;
    

    protected override void SetUp() 
    {
        mapName = "Slime Dungeon";
        numberOfRooms = 6;
        treasureRooms = 1;
        eventRooms = 2;
        mapGenerator = GetComponent<MapGenerator>();
    }

    public override void GenerateMap()
    {
        GameObject[] roomsMade = new GameObject[numberOfRooms + 1];
        for(int i = 0; i < numberOfRooms + 1; i++)
        {
            if(i == 0)
            {
                roomsMade[i] = mapGenerator.CreateRoom(RoomType.Start);
            }
            else if (i <= 2)
            {
                roomsMade[i] = mapGenerator.CreateRoom(RoomType.Enemy);
            }
            else
            {
                float chance = Random.value;
                if (treasureRooms > 0 && chance < 0.3f)
                {
                    roomsMade[i] = mapGenerator.CreateRoom(RoomType.Treasure);
                    treasureRooms--;
                }
                else if (eventRooms > 0)
                {
                    roomsMade[i] = mapGenerator.CreateRoom(RoomType.Event);
                    eventRooms--;
                }
                else
                {
                    roomsMade[i] = mapGenerator.CreateRoom(RoomType.Enemy);
                }
            }
            roomsMade[i].transform.position = new Vector3(0, 0);
            roomsMade[i].name = roomsMade[i].name + i;
        }
        mapGenerator.GenerateMap(roomsMade);
    }
}
