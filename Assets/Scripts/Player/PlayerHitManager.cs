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
        if (photonView.IsMine)
        {
            if (other.gameObject.CompareTag("Bullet"))
            {
                Bullet bullet = other.GetComponent<Bullet>();

                // Проверяем, не является ли текущий игрок владельцем пули и может ли пуля поранить
                if (bullet.ownerViewID != photonView.ViewID )
                {
                    PlayerData damagedPlayerData;
                    if (GameManager.instance.playerDataDictionary.TryGetValue(photonView.ViewID, out damagedPlayerData))
                    {
                        damagedPlayerData.TakeDamage(bullet.damageAmount);
                        damagedPlayerData.UpdateDataUI();
                    }
                }
            }
        }
    }


}
