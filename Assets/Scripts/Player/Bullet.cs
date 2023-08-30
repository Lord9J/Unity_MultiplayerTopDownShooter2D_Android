using System.Collections;
using UnityEngine;
using Photon.Pun;
public class Bullet : MonoBehaviourPunCallbacks
{
    public GameObject bulletEffectPrefab;
    public int damageAmount = 10;
    public int ownerViewID;

    private void OnTriggerEnter2D(Collider2D other) // Если пуля столкнулась с другим объектом
    {
        if (other.tag != "Player") // Если это не игрок
        {
            // Создать префаб эффекта попадания пули
            GameObject bulletEffect = PhotonNetwork.Instantiate(bulletEffectPrefab.name, this.gameObject.transform.position, Quaternion.identity);
            bulletEffect.SetActive(true);

            // Начать коронтитул по уничтожению эффекта по истечению времени
            StartCoroutine(DestroyEffectCoroutine(bulletEffect));

            // Уничтожить префаб пули
            PhotonNetwork.Destroy(gameObject);
        }
        else
        {
            StartCoroutine(DestroyBulletCoroutine());
        }


    }

    private IEnumerator DestroyEffectCoroutine(GameObject effect)
    {
        yield return new WaitForSeconds(1f); 
        PhotonNetwork.Destroy(effect);
    }
    
    private IEnumerator DestroyBulletCoroutine()
    {
        yield return new WaitForSeconds(0.1f); 
        PhotonNetwork.Destroy(gameObject);
    }
}
