using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    PlayerController player;
    MapData mapData;

    public void SetUp(PlayerController p, MapData m)
    {
        player = p;
        mapData = m;
    }

    // Update is called once per frame
    void Update()
    {
        switch (player.ControllerDirection)
        {
            case ControllerDirection.Down:
                // Go down from current room
                if (player.Pressed())
                    if (mapData.CurrentRoom.down != null)
                        mapData.MoveRoom(-2);
                break;
            case ControllerDirection.Up:
                // Go up from current room
                if (player.Pressed())
                    if (mapData.CurrentRoom.up != null)
                        mapData.MoveRoom(2);
                break;
            case ControllerDirection.Left:
                // Go left
                if (player.Pressed())
                    if (mapData.CurrentRoom.left != null)
                        mapData.MoveRoom(-1);
                break;
            case ControllerDirection.Right:
                // Go right
                if (player.Pressed())
                    if (mapData.CurrentRoom.right != null)
                        mapData.MoveRoom(1);
                break;
            default:
                break;
        }
        switch (player.ControllerButton)
        {
            case ControllerButton.A:
                break;
            case ControllerButton.B:
                break;
            default:
                break;
        }
    }
}
