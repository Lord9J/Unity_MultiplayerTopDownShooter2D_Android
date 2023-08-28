using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerHitManager : MonoBehaviourPunCallbacks
{
    public PlayerData playerData;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            Bullet bullet = other.GetComponent<Bullet>();

            // Проверяем, не является ли текущий игрок владельцем пули
            if (bullet.ownerViewID != photonView.ViewID)
            {
                if (playerData != null)
                {
                    playerData.TakeDamage(bullet.damageAmount);
                    playerData.UpdateDataUI();
                }
            }
        }
    }



    // private void OnTriggerEnter2D(Collider2D other)
    // {
    //     // if (other.gameObject.CompareTag("Bullet"))
    //     // {
    //     //     Bullet bullet = other.GetComponent<Bullet>();

    //     //     // Проверяем, не является ли текущий игрок владельцем пули
    //     //     if (bullet.ownerViewID != photonView.ViewID)
    //     //     {
    //     //         PlayerData playerData = GameManager.instance.GetPlayerData(photonView.ViewID);
    //     //         if (playerData != null)
    //     //         {
    //     //             playerData.TakeDamage(bullet.damageAmount);
    //     //             playerData.UpdateDataUI();
    //     //         }
    //     //     }
    //     // }
    // }
}
