using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;
using Photon.Pun;
using ExitGames.Client.Photon;

public class PlayerUI : MonoBehaviour
{
    [Header("Данные игрока")]
    public TMP_Text coinsText;
    public Slider healthSlider;
    public GameObject lookAndShootJoystick;
    public GameObject movementJoyStick;

    [Header("Экран результатов игры")]
    public TMP_Text finishCoinsText;
    public TMP_Text playerNameText;
    public GameObject FinishTable;

    public static event Action ChangePlayerControllerMode;

    private void Start()
    {
        PlayerData.SetPlayerHealth += UpdateHealth;
        PlayerData.SetPlayerCoins += UpdateCoins;

        PlayerControlLChanger.ChangeJoyStickMode += ChangeJoyStickMode;
    }

    private void OnEventReceived(EventData eventData)
    {
        // if (eventData.Code == GameManager.EventCode_PlayerWin)
        // {
        //     object[] content = (object[])eventData.CustomData;
        //     int winnerViewID = (int)content[0];

        //     if (GameManager.instance.GetPlayerData(winnerViewID) is PlayerData winnerPlayerData)
        //     {
        //         finishCoinsText.text = winnerPlayerData.coins.ToString();
        //         playerNameText.text = winnerPlayerData.name; // Имя победителя

        //         // Показать экран победы
        //         FinishTable.SetActive(true);
        //     }
        // }
    }

    private void UpdateHealth(float hp)
    {
        healthSlider.value = hp;
    }
    private void UpdateCoins(int coins)
    {
        coinsText.text = coins.ToString();
    }

    private void ChangeJoyStickMode(bool mode)
    {
        lookAndShootJoystick.SetActive(!mode);
        movementJoyStick.SetActive(!mode);
    }

    public void ChangeControllMode()
    {
        ChangePlayerControllerMode?.Invoke();
    }

    public void ExitGameBtn()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("Lobby");
    }

}
