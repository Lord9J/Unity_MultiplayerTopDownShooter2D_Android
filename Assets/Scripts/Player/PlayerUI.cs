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
    public GameObject RestartBtn;

    public static event Action ChangePlayerControllerMode;

    private void Awake()
    {
        PlayerControlLChanger.ChangeJoyStickMode += ChangeJoyStickMode;
    }

    public void SendPlayerUIStatus(bool status)
    {
        if (status)
        {
            Debug.Log(PhotonNetwork.NickName + " подписался на события");
            // подписка на события из PlayerData
            PlayerData.SetPlayerHealth += UpdateHealth;
            PlayerData.SetPlayerCoins += UpdateCoins;
        }
        else
        {
            Debug.Log(PhotonNetwork.NickName + " отписался");
            // отписка от событий
            PlayerData.SetPlayerHealth -= UpdateHealth;
            PlayerData.SetPlayerCoins -= UpdateCoins;
        }
    }

    public void ShowWinnerPanel(string winnerNickname, int winnerCoins)
    {
        Debug.Log(winnerNickname + " победил! Количество монет: " + winnerCoins);

        if (PhotonNetwork.IsMasterClient) // показать кнопку перезапуска мастеру клиенту
        {
            RestartBtn.SetActive(true);
        }

        finishCoinsText.text = winnerCoins.ToString();
        playerNameText.text = winnerNickname; // Имя победителя

        // Показать экран победы
        FinishTable.SetActive(true);

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

    public void RestartGameBtn()
    {
        GameManager.instance.RestartGame();
    }

    
    public void ExitGameBtn()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("Lobby");
    }

}
