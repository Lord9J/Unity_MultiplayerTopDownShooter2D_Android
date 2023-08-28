using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;
using Photon.Realtime;

public class PlayerData : MonoBehaviourPunCallbacks, IPunObservable
{
    public int playerID; // Уникальный ID игрока
    public int maxHealth = 100;
    public int currentHealth;
    public int coins;
    public bool isAlive;

    public static event Action<float> SetPlayerHealth;
    public static event Action<int> SetPlayerCoins;

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

        UpdateDataUI();
    }

    public void UpdateDataUI()
    {
        if (isAlive)
        {
            SetPlayerHealth?.Invoke(currentHealth);
            SetPlayerCoins?.Invoke(coins);
        }
    }

    public void TakeDamage(int damage)
    {
        if (photonView.IsMine)
        {
            Debug.Log(PhotonNetwork.NickName + " получил " + damage + " урона");

            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                isAlive = false;

                // Уведомляем о проигрыше
                GameManager.instance.photonView.RPC("OnPlayerLost", RpcTarget.All);

                Debug.Log(this.photonView.Owner.NickName + " проиграл");

                PhotonNetwork.Destroy(this.gameObject);
            }
            UpdateDataUI();
        }
    }

    

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
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