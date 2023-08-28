using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Weapon : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float fireRate = 2f; // скорострельность
    public float bulletSpeed = 15f; // начальная скорость пули
    private float lastFireTime = 0f;

    public PhotonView photonView;

    public void Shoot()
    {
        // прошло ли достаточно времени для следующего выстрела
        if (Time.time - lastFireTime >= 1f / fireRate)
        {
            GameObject bulletGO = PhotonNetwork.Instantiate(bulletPrefab.name, firePoint.position, firePoint.rotation);
            Rigidbody2D bulletRb = bulletGO.GetComponent<Rigidbody2D>();
            Bullet bullet = bulletGO.GetComponent<Bullet>();

            // Устанавливаем владельца пули
            bullet.ownerViewID = photonView.ViewID;

            if (bulletRb != null)
            {
                bulletRb.velocity = bulletSpeed * firePoint.up;
            }
            lastFireTime = Time.time;
        }
    }
}
