using System.Collections;
using UnityEngine;
using Photon.Pun;
using System;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : MonoBehaviourPunCallbacks
{
    public SpawnManager spawnManager;
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

    public void AddPlayerData(PlayerData playerData)
    {
        Debug.Log("В словарь персонажей добавлен "+playerData.playerID);
        playerDataDictionary.Add(playerData.playerID, playerData); // добавить данные персонажа в словарь 
    }
    public void RemovePlayerData(PlayerData playerData)
    {
        Debug.Log("Из словаря персонажей удален "+playerData.playerID);
        playerDataDictionary.Remove(playerData.playerID); // убрать данные персонажа из словаря 
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
