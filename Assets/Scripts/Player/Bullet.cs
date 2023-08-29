using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Bullet : MonoBehaviour
{
    public int damageAmount = 10;
    public int ownerViewID;
    private Animator animator;

    Rigidbody2D rb;
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(DestroyBulletTime(4f));
    }

    IEnumerator DestroyBulletTime(float sec)
    {
        yield return new WaitForSeconds(sec);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
       
        // Активируем анимацию эффекта вспышки при попадании в объект
        if (animator != null)
        {
            rb.velocity = new Vector2(0.0f, 0.0f);
            animator.SetTrigger("StartEffect");
            GetComponent<Collider2D>().enabled = false;
        }
        StartCoroutine(DestroyBulletTime(1f));
    }

}
