using System.Collections;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    public SpawnManager spawnManager;
    public PlayerUI playerUI;
    public static GameManager instance;

    public TMP_Text TestText; // текст вывода информации
    [SerializeField] private ScrollRect uiScrollRect; // авто-скролл консоли вниз
    public Dictionary<int, PlayerData> playerDataDictionary = new Dictionary<int, PlayerData>();
    public bool gameReady = false;
    public bool gameStarted = false;


    private void Awake()
    {
        if (instance == null) instance = this;

        Application.logMessageReceived += LogCallback; // для вывода дебагов в консоль

        // Получение никнейма игрока
        PhotonNetwork.NickName = PlayerPrefs.GetString("PlayerNickname");

        // Вывод сообщения о подключении игрока
        Debug.Log("Игрок: " + PhotonNetwork.NickName + " подключился");

        if (!PhotonNetwork.IsMasterClient) // Если это не мастер
            photonView.RPC("IsGameStarted", RpcTarget.MasterClient); // Спрашиваем у мастера, началась ли игра

        // Проверка для старта игры
        if (PhotonNetwork.CurrentRoom.PlayerCount > 1 && !gameReady)
        {
            photonView.RPC("GameReadyRPC", RpcTarget.All); // Отправить всем сигнал, что собрано достаточно игроков
        }

    }

    [PunRPC] // Метод, вызываемый у всех, если в комнате достаточно игроков
    private void GameReadyRPC()
    {
        gameReady = true;
        playerUI.StartText.text = "Можно начать игру";
    }

    [PunRPC] // Спрятать стартовое окно
    private void HideStartGameTable()
    {
        playerUI.ShowHideStartGameTable(false);
    }

    public void StartGame() // Метод запуска игры, запускает мастер клиент от PlayerUI кнопкой
    {
        gameStarted = true;

        photonView.RPC("SetGameStartedBool", RpcTarget.All);
        photonView.RPC("HideStartGameTable", RpcTarget.All);
        spawnManager.photonView.RPC("StartGameRPC", RpcTarget.All);
    }



    [PunRPC] // Если кто из персонажей игроков проиграл
    private void OnPlayerLost()
    {
        int alivePlayersCount = playerDataDictionary.Count(kv => kv.Value.isAlive);

        Debug.Log("Живых игроков =" + alivePlayersCount);

        if (alivePlayersCount <= 1) // Проверка на победителя, если остался один персонаж
        {
            PlayerData winner = playerDataDictionary.Values.FirstOrDefault(data => data.isAlive);

            if (winner != null)
            {
                // Отправьте сигнал о победе победителя всем остальным игрокам
                photonView.RPC("OnPlayerWin", RpcTarget.All, winner.photonView.Owner.NickName, winner.coins);
            }
        }
    }

    [PunRPC] // Метод для всех, что этот персонаж победил
    private void OnPlayerWin(string winnerNickname, int winnerCoins)
    {
        playerUI.ShowWinnerPanel(winnerNickname, winnerCoins);
    }

    public void SendPlayerUIStatus(bool status) 
    {
        playerUI.SendPlayerUIStatus(status);
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Debug.Log("Игрок " + PlayerPrefs.GetString("PlayerNickname") + " присоединился к комнате.");
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        Debug.Log("Игрок " + PlayerPrefs.GetString("PlayerNickname") + " покинул комнату.");
    }

    public void LeaveRoom()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("ForceKickAll", RpcTarget.All);
        }
        else
        {
            PhotonNetwork.LeaveRoom();
            SceneManager.LoadScene("Lobby");
        }
    }

    [PunRPC] // Мастер получает это
    public void ForceKickAll()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("Lobby");
    }


    [PunRPC] // Мастер получает это
    public void IsGameStarted()
    {
        if (gameStarted) photonView.RPC("SetGameStartedBool", RpcTarget.All);
    }


    [PunRPC] // Мастер отправляет это всем
    public void SetGameStartedBool()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            gameStarted = true;
            // Вывести меню начала игры что уже началась игра
            playerUI.IsGameStarted();
        }

    }




    // Для отладки
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
