using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public PlayerData playerData;
    public Slider healthSlider;
    private Camera mainCamera;
    public Transform target;


    private void Start()
    {
        mainCamera = Camera.main;

        PlayerData.SetPlayerHealth += UpdateHealth;

        if (playerData != null)
        {
            UpdateHealth(playerData.currentHealth);
            playerData.UpdateDataUI();
        }

    }


    // Метод для обновления полоски здоровья
    public void UpdateHealth(float currentHealth)
    {
        // Установим текущее здоровье в Slider
        healthSlider.value = currentHealth;
    }

    // Метод для корректировки позиции Slider
    private void Update()
    {
        transform.rotation = mainCamera.transform.rotation;
        transform.position = target.transform.position;
    }
}
