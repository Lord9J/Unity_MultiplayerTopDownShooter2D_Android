using UnityEngine;
using Photon.Pun;

public class PlayerCoinTaker : MonoBehaviourPunCallbacks
{
    private PlayerData playerData;

    private void Start()
    {
        playerData = GetComponent<PlayerData>();
    }
    
    // Если игрок столкунлся с объектом
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Coin") // проверить объект на монету
        {
            if (playerData != null)
            {
                playerData.coins++;
                playerData.UpdateDataUI();
                other.GetComponent<Collider2D>().enabled = false;
            }

            PhotonView coinView = other.gameObject.GetPhotonView();

            if (coinView.IsMine) // Вызов метода на уничтожение монеты
            {
                photonView.RPC("DestroyCoinRPC", RpcTarget.All, coinView.ViewID);
            }
        }
    }


    [PunRPC]
    private void DestroyCoinRPC(int coinViewID) // метод уничтожения монеты
    {
        if (PhotonNetwork.IsMasterClient)
        {
            GameObject coinObject = PhotonView.Find(coinViewID)?.gameObject;
            if (coinObject != null)
            {
                PhotonNetwork.Destroy(coinObject);
            }
        }
    }

}
