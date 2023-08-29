using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;

public class PlayerData : MonoBehaviourPunCallbacks, IPunObservable
{
    public int playerID; // Уникальный ID игрока
    public int maxHealth = 100;
    public int currentHealth;
    public int coins;
    public bool isAlive;

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

    public void UpdateDataUI()
    {
        SetPlayerHealth?.Invoke(currentHealth);
        SetPlayerCoins?.Invoke(coins);
    }

    public void TakeDamage(int damage)
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