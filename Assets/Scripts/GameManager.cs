using System.Collections;
using UnityEngine;
using Photon.Pun;
using System;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviourPunCallbacks
{
    public SpawnManager spawnManager;
    public PlayerUI playerUI;
    public static GameManager instance;

    public TMP_Text TestText; // текст вывода информации
    public GameObject StartText; // текст вывода информации
    [SerializeField] private ScrollRect uiScrollRect; // авто-скролл консоли вниз
    public Dictionary<int, PlayerData> playerDataDictionary = new Dictionary<int, PlayerData>();


    private bool gameStarted = false;


    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

        Application.logMessageReceived += LogCallback; // для вывода дебагов в консоль

        // Получение никнейма игрока
        PhotonNetwork.NickName = PlayerPrefs.GetString("PlayerNickname");
        // Вывод сообщения о подключении игрока
        Debug.Log("Игрок: " + PhotonNetwork.NickName + " подключился");

        // Проверка для старта игры
        if (PhotonNetwork.CurrentRoom.PlayerCount > 1 && !gameStarted)
        {
            Debug.Log("Достигнуто требуемое количество игроков, начинаем игру.");
            gameStarted = true;
            AllPlayersReady();
        }
    }


    [PunRPC]
    private void OnPlayerLost()
    {
        int alivePlayersCount = playerDataDictionary.Count(kv => kv.Value.isAlive);

        if (alivePlayersCount <= 1)
        {
            PlayerData winner = playerDataDictionary.Values.FirstOrDefault(data => data.isAlive);

            if (winner != null)
            {
                // Отправьте сигнал о победе победителя всем остальным игрокам
                photonView.RPC("OnPlayerWin", RpcTarget.All, winner.photonView.Owner.NickName, winner.coins);
            }
        }
    }

    [PunRPC]
    private void OnPlayerWin(string winnerNickname, int winnerCoins)
    {
       playerUI.ShowWinnerPanel(winnerNickname, winnerCoins);
    }


    private void AllPlayersReady()
    {
        Debug.Log("Все игроки готовы, запускаю метод спавна RPC");
        spawnManager.photonView.RPC("StartGameRPC", RpcTarget.All);
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Debug.Log("Игрок " + PlayerPrefs.GetString("PlayerNickname") + " присоединился к комнате.");
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        Debug.Log("Игрок " + PlayerPrefs.GetString("PlayerNickname") + " покинул комнату.");
    }



    private void LogCallback(string message, string stackTrace, LogType type)
    {
        TestText.text += $": {message} \n";
        StartCoroutine(ScrollDown());
    }
    IEnumerator ScrollDown()
    {
        yield return null;  // Wait for the end of the current frame to ensure the layout has updated.
        uiScrollRect.verticalNormalizedPosition = 0f;  // Scroll to the bottom.
    }

}
