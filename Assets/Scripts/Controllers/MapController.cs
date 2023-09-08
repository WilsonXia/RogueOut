using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    PlayerController playerController;
    MapData mapData;

    public void SetUp(PlayerController p, MapData m)
    {
        playerController = p;
        mapData = m;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerController.Pressed())
        {
            switch (playerController.ControllerDirection)
            {
                case ControllerDirection.Down:
                    // Go down from current room

                    if (mapData.CurrentRoom.down != null)
                        mapData.MoveRoom(-2);
                    break;
                case ControllerDirection.Up:
                    // Go up from current room
                    
                        if (mapData.CurrentRoom.up != null)
                            mapData.MoveRoom(2);
                    break;
                case ControllerDirection.Left:
                    // Go left
                    
                        if (mapData.CurrentRoom.left != null)
                            mapData.MoveRoom(-1);
                    break;
                case ControllerDirection.Right:
                    // Go right
                    
                        if (mapData.CurrentRoom.right != null)
                            mapData.MoveRoom(1);
                    break;
                default:
                    break;
            }
        }
        switch (playerController.ControllerButton)
        {
            case ControllerButton.A:
                mapData.SelectRoom();
                break;
            case ControllerButton.B:
                break;
            default:
                break;
        }
    }
}
