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

    [Header("Экран для начала игры")]
    public GameObject StartGameTable;
    public GameObject StartGameBtnObject;
    public GameObject ExitGameBtnObject;
    public TMP_Text StartText; // текст вывода информации
    public static event Action ChangePlayerControllerMode;

    private void Awake()
    {
        PlayerControlLChanger.ChangeJoyStickMode += ChangeJoyStickMode;

        // Если это мастер, показать кнопку начала игры
        if (PhotonNetwork.IsMasterClient) StartGameBtnObject.SetActive(true);
        else StartGameBtnObject.SetActive(false);
    }

    public void SendPlayerUIStatus(bool status)
    {
        if (status)
        {
            // подписка на события из PlayerData
            PlayerData.SetPlayerHealth += UpdateHealth;
            PlayerData.SetPlayerCoins += UpdateCoins;
        }
        else
        {
            // отписка от событий
            PlayerData.SetPlayerHealth -= UpdateHealth;
            PlayerData.SetPlayerCoins -= UpdateCoins;
        }
    }

    public void ShowWinnerPanel(string winnerNickname, int winnerCoins)
    {
        Debug.Log(winnerNickname + " победил! Количество монет: " + winnerCoins);


        finishCoinsText.text = winnerCoins.ToString();
        playerNameText.text = winnerNickname; // Имя победителя

        // Показать экран победы
        FinishTable.SetActive(true);
    }

    public void ShowHideStartGameTable(bool status)
    {
        StartGameTable.SetActive(status);
    }

    public void IsGameStarted()
    {
        StartText.text = "Игра уже началась";
        ExitGameBtnObject.SetActive(true);
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

    public void StartGameBtn()
    {
        if (!GameManager.instance.gameReady) StartText.text = "Недостаточно игроков для начала игры";
        else
            GameManager.instance.StartGame();
    }


    public void ExitGameBtn()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("Lobby");
    }

}
