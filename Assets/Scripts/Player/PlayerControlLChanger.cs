using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class PlayerControlLChanger : MonoBehaviour
{
    private PlayerController playerController;
    private bool _controllMode = false;

    public static event Action<bool> ChangeJoyStickMode;
    PhotonView view;

    private void Start()
    {
        view = GetComponent<PhotonView>();
        if (view.IsMine) // Убедитесь, что объект принадлежит текущему игроку
        {
            playerController = GetComponent<PlayerController>();
        }
        PlayerUI.ChangePlayerControllerMode += ChangeControllMode;

    }


    public void ChangeControllMode()
    {
        if (playerController != null)
        {
            _controllMode = !_controllMode;
            playerController.controllMode = _controllMode;

            ChangeJoyStickMode?.Invoke(_controllMode);
        }
    }
}
