using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour
{
    GameObject playerAvatar;

    string playerName;
    int playerAvatarIndex;
    Color32 progressBarColor;

    Vector3 avatarStartPosition;
    Vector3 avatarEndPosition;

    void Start()
    {
        playerAvatar = gameObject.transform.Find("PlayerAvatar").gameObject;
        avatarStartPosition = playerAvatar.GetComponent<RectTransform>().localPosition;
        avatarEndPosition = new Vector3(816, avatarStartPosition.y, avatarStartPosition.z);
    }

    public void Initialize(int playerAvatarIndex, Color32 progressBarColor, string playerName)
    {
        this.playerAvatarIndex = playerAvatarIndex;
        this.progressBarColor = progressBarColor;
        this.playerName = playerName;

        // Change Avatar
        GameObject playerAvatar = gameObject.transform.Find("PlayerAvatar").gameObject;
        string indexStr = "";
        if (playerAvatarIndex < 10)
        {
            indexStr = "0" + playerAvatarIndex.ToString();
        }
        else
        {
            indexStr = playerAvatarIndex.ToString();
        }
        playerAvatar.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/AnimalAvatars/Avatar_" + indexStr);

        // Change path color
        GameObject playerPathProgress = gameObject.transform.Find("PlayerPathProgress").gameObject;
        playerPathProgress.GetComponent<Image>().color = progressBarColor;
    }

    void Update()
    {
        
    }

    public void MovePlayer(float percentageMove)
    {
        Vector3 currentPosition = playerAvatar.GetComponent<RectTransform>().localPosition;
        float newXPosition = currentPosition.x + (avatarEndPosition.x - avatarStartPosition.x) * percentageMove;
        Vector3 newPosition = new Vector3(newXPosition, avatarStartPosition.y, avatarStartPosition.z);
        playerAvatar.GetComponent<RectTransform>().localPosition = newPosition;
    }
}
