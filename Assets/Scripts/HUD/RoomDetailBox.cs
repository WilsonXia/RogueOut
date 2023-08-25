using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RoomDetailBox : MonoBehaviour
{
    // Receives a room
    // - Room Name
    // - Room Type
    // - If Enemy,
    //      Draw each encounterable enemy in the details box.
    //      Space out each enemy box

    // Components
    Room room;
    TextMeshProUGUI roomLabel;
    public GameObject roomLabelObject;
    public RectTransform detailsBoxObject;
    public GameObject enemyBoxPrefab;

    // Details
    string roomName;

    // Generation Details
    int numOfEnemies;
    float margin;
    // Start is called before the first frame update
    private void Start()
    {
        roomLabel = roomLabelObject.GetComponent<TextMeshProUGUI>();
        margin = 80f;
    }
    public void SetUp(GameObject r)
    {
        CleanDetails();
        room = r.GetComponent<Room>();

        roomName = room.type.ToString() + " Room";
        roomLabel.text = roomName;
        roomLabel.color = Color.white;
        GetComponent<Image>().color = room.GetComponent<SpriteRenderer>().color;
        switch (room.type)
        {
            case RoomType.Enemy:
                detailsBoxObject.gameObject.SetActive(true);
                EnemyRoom enemyRoom = room.GetComponent<EnemyRoom>();
                numOfEnemies = enemyRoom.numberOfEnemies;
                Vector2 enemyBoxPos = detailsBoxObject.rect.position + new Vector2(detailsBoxObject.rect.width / 2, detailsBoxObject.rect.height / 2);
                float centerFix = (numOfEnemies - 1f) / 2f;
                enemyBoxPos -= new Vector2(margin * centerFix,0);
                foreach (GameObject enemy in enemyRoom.enemyPrefabs)
                {
                    Sprite enemySprite = enemy.GetComponent<SpriteRenderer>().sprite;
                    GameObject box = Instantiate(enemyBoxPrefab, detailsBoxObject.transform);
                    box.transform.position = detailsBoxObject.transform.TransformPoint(enemyBoxPos);
                    box.GetComponent<Image>().sprite = enemySprite;
                    enemyBoxPos += new Vector2(margin, 0);
                }
                break;
            case RoomType.Start:
                roomLabel.color = Color.black;
                goto default;
            default:
                detailsBoxObject.gameObject.SetActive(false);
                break;
        }
    }

    void CleanDetails()
    {
        foreach(Transform child in detailsBoxObject.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
