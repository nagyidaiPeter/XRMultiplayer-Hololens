using Assets.Scripts.SERVER;

using hololensMulti;

using hololensMultiplayer;
using hololensMultiplayer.Models;

using Lidgren.Network;

using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Zenject;

public class LocalPlayer : MonoBehaviour
{
    public Transform qrPos;

    [Inject]
    private DataManager dataManager;

    [Inject]
    private NetClient netclientclient;

    private Client client;

    public Transform RH, LH;

    void Start()
    {
        dataManager.LocalPlayer = new PlayerData();
        dataManager.LocalPlayer.playerObject = gameObject;

        client = FindObjectOfType<Client>(true);
    }


    void Update()
    {
        if (netclientclient.ConnectionStatus == NetConnectionStatus.Connected && dataManager.LocalPlayer != null && dataManager.LocalPlayer.playerObject.activeSelf)
        {
            PlayerTransform playerTransform = new PlayerTransform();
            playerTransform.SenderID = dataManager.LocalPlayer.ID;

            playerTransform.Pos = qrPos.parent.InverseTransformPoint(transform.position);
            playerTransform.Rot = transform.localRotation;
            playerTransform.QrOffset = qrPos.parent.eulerAngles;

            if (RH != null)
            {
                playerTransform.RHPos = transform.InverseTransformPoint(RH.position);
                playerTransform.RHRot = RH.localRotation;
            }

            if (LH != null)
            {
                playerTransform.LHPos = transform.InverseTransformPoint(LH.position);
                playerTransform.LHRot = LH.localRotation;
            }

            client.MessageProcessors[MessageTypes.PlayerTransform].AddOutMessage(playerTransform);
        }
    }
}
