using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using System;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Weapon weapon;
    public Joystick movementJoystick;
    public Joystick lookAndShootJoystick;
    public bool controllMode=false;
    private Vector3 movement;
    private Animator animator;
    PhotonView view;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        view = GetComponent<PhotonView>();

        movementJoystick = GameObject.Find("MovementJoyStick").GetComponent<DynamicJoystick>();
        lookAndShootJoystick = GameObject.Find("lookAndShootJoystick").GetComponent<DynamicJoystick>();
    }


    void Update()
    {
        if (view.IsMine)
        {
            if (!controllMode) // Режимы клава мыши и мобильного джойстика 
            {
                // Получаем ввод 
                movement.x = movementJoystick.Horizontal;
                movement.y = movementJoystick.Vertical;

                // Получаем ввод от второго джойстика (lookAndShootJoystick)
                Vector2 lookDirection = new Vector2(lookAndShootJoystick.Horizontal, lookAndShootJoystick.Vertical);

                // Поворот персонажа в сторону указанного направления
                if (lookDirection.magnitude > 0.1f)
                {
                    float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
                    rb.rotation = angle;
                }

                // Стрельба при нажатии на второй джойстик (lookAndShootJoystick)
                if (lookDirection.magnitude > 0.1f) // Проверяем, двигается ли джойстик
                {
                    if (animator != null)
                    {
                        animator.SetBool("IsShooting", true);
                    }
                    weapon.Shoot();
                }
                else
                {
                    animator.SetBool("IsShooting", false);
                }
            }
            else
            {
                // Получаем ввод от клавиш WASD
                movement.x = Input.GetAxis("Horizontal");
                movement.y = Input.GetAxis("Vertical");

                // Поворот персонажа в сторону мыши
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 lookDirection = mousePosition - (Vector3)rb.position;

                float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;

                rb.rotation = angle;

                // Стрельба при нажатии левой кнопки мыши
                if (Input.GetButton("Fire1"))
                {
                    if (animator != null)
                    {
                        animator.SetBool("IsShooting", true);
                    }
                    weapon.Shoot();
                }
                if (Input.GetButtonUp("Fire1"))
                {
                    animator.SetBool("IsShooting", false);
                }
            }

            // Выполняем перемещение
            rb.MovePosition(rb.position + (Vector2)(movement * moveSpeed * Time.fixedDeltaTime));
        }

    }

}
