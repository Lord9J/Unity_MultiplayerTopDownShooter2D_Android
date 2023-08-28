using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerCoinTaker : MonoBehaviourPunCallbacks
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Coin")
        {
            photonView.RPC("CollectCoinRPC", RpcTarget.All, other.gameObject.GetPhotonView().ViewID);
        }
    }

    [PunRPC]
    private void CollectCoinRPC(int coinViewID)
    {
        PhotonView coinPhotonView = PhotonView.Find(coinViewID);
        if (coinPhotonView != null)
        {
            GameObject coin = coinPhotonView.gameObject;
            CollectCoin(coin);
        }
    }

    private void CollectCoin(GameObject coin)
    {
        PlayerData playerData = GetComponent<PlayerData>();

        if (playerData != null)
        {
            playerData.coins++;
            playerData.UpdateDataUI();
            Destroy(coin.gameObject);
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
