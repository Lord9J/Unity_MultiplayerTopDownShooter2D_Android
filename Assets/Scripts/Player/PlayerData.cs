using UnityEngine;
using System;
using Photon.Pun;

public class PlayerData : MonoBehaviourPunCallbacks, IPunObservable
{
    public int playerID; // Уникальный ID игрока
    public int maxHealth = 100; // маскимальное возможное здоровье 
    public int currentHealth; // текущиее здоровье 
    public int coins; // текущее количество монет
    public bool isAlive; // жив ли этот персонаж


    // События для обновления интерфейса
    public static event Action<float> SetPlayerHealth;
    public static event Action<int> SetPlayerCoins;
    public static event Action<bool> SendPlayerUIStatus;


    // Инициализация данных игрока
    public void InitializePlayerData()
    {
        playerID = gameObject.GetPhotonView().ViewID; // Устанавливаем ID

        // Назначение случайного цвета
        Color randomColor = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.color = randomColor;

        currentHealth = maxHealth;
        coins = 0;
        isAlive = true;

        GameManager.instance.SendPlayerUIStatus(true); // отправить сигнал на подписку

        UpdateDataUI(); 
    }

    public void UpdateDataUI() // Обновить параметры интерфейса
    {
        SetPlayerHealth?.Invoke(currentHealth);
        SetPlayerCoins?.Invoke(coins);
    }

    public void TakeDamage(int damage) // Персонаж получил урон
    {
        if (photonView.IsMine)
        {
            if (isAlive) 
            {
                currentHealth -= damage;
                UpdateDataUI();
                
                if (currentHealth <= 0) // поражение персонажа
                {
                    GameManager.instance.SendPlayerUIStatus(false); // отправить сигнал на подписку

                    isAlive = false;

                    // Уведомляем о проигрыше
                    GameManager.instance.photonView.RPC("OnPlayerLost", RpcTarget.All);

                    Debug.Log(this.photonView.Owner.NickName + " проиграл");

                    PhotonNetwork.Destroy(this.gameObject);
                }
            }

        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) // синронизация данных
    {
        if (stream.IsWriting) // Отправка данных
        {
            stream.SendNext(currentHealth);
            stream.SendNext(coins);
            stream.SendNext(isAlive);
        }
        else // Получение данных
        {
            currentHealth = (int)stream.ReceiveNext();
            coins = (int)stream.ReceiveNext();
            isAlive = (bool)stream.ReceiveNext();
        }
    }


}