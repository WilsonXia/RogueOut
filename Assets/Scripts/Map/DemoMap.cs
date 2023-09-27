using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoMap : MapData
{
    protected override void SetUp()
    {
        mapName = "Demo Dungeon";
        numberOfRooms = 6;
        mapGenerator = GetComponent<MapGenerator>();
    }

    public override void GenerateMap()
    {
        GameObject[] roomsMade = new GameObject[numberOfRooms + 1];
        for (int i = 0; i < numberOfRooms + 1; i++)
        {
            if (i == 0)
            {
                roomsMade[i] = mapGenerator.CreateRoom(RoomType.Start);
            }
            else
            {
                roomsMade[i] = mapGenerator.CreateRoom(RoomType.Enemy);
            }
            roomsMade[i].name = roomsMade[i].name + i;
        }
        mapGenerator.ReceiveRooms(roomsMade);
        mapGenerator.ChainStructure();
        ReceiveMap(mapGenerator.Rooms);
    }
}
