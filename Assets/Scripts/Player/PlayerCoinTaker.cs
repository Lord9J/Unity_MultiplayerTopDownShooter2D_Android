using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerCoinTaker : MonoBehaviourPunCallbacks
{
    private PlayerData playerData;

    private void Start()
    {
        playerData = GetComponent<PlayerData>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Coin")
        {
            CollectCoin(other.gameObject);
            photonView.RPC("DestroyCoinRPC", RpcTarget.All, other.gameObject.name);
        }
    }

    [PunRPC]
    private void DestroyCoinRPC(string coinName)
    {
        // Найти объект монеты по имени и уничтожить его
        GameObject coinToDestroy = GameObject.Find(coinName);
        if (coinToDestroy != null)
        {
            Destroy(coinToDestroy);
        }
    }

    private void CollectCoin(GameObject coin)
    {
        if (playerData != null)
        {
            playerData.coins++;
            playerData.UpdateDataUI();
        }
    }

    // if (photonView.IsMine)
    //     {
    //         if (other.gameObject.tag == "Coin")
    //         {
    //             PlayerData playerData;
    //             if (GameManager.instance.playerDataDictionary.TryGetValue(photonView.ViewID, out playerData))
    //             {
    //                 playerData.coins++;
    //                 playerData.UpdateDataUI();
    //             }

    //             if (photonView.IsMine)
    //             {
    //                 PhotonNetwork.Destroy(other.gameObject.gameObject);
    //             }
    //         }
    //     }

    // private void CollectCoin(GameObject coin)
    // {
    //     PlayerData playerData = GetComponent<PlayerData>(); // Получаем компонент PlayerData

    //     if (playerData != null)
    //     {
    //         playerData.coins++;
    //         playerData.UpdateDataUI();
    //         PhotonNetwork.Destroy(coin.gameObject);
    //     }
    // }
}
