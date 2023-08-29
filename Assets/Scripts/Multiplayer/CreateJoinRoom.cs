using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class CreateJoinRoom : MonoBehaviourPunCallbacks
{
    public TMP_InputField createInput;
    public TMP_InputField joinInput;
    public TMP_InputField playerNickNameInput;

    private void Start()
    {
        if (playerNickNameInput.text == "")
        {
            string randomName = "Player" + Random.Range(1, 9999);
            playerNickNameInput.text = randomName;
        }

    }

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(createInput.text);
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(joinInput.text);
    }

    public override void OnJoinedRoom()
    {
        PlayerPrefs.SetString("PlayerNickname", playerNickNameInput.text);
        PhotonNetwork.LoadLevel("Game");
    }
}
