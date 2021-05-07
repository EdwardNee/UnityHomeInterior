using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManagerUnity : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Text logText;

    // Start is called before the first frame update
    void Start()
    {
        //Ќик пользовател€.
        PhotonNetwork.NickName = $"User{Random.Range(1, 1000)}";
        PhotonNetwork.AutomaticallySyncScene = true;
        //¬ерси€ игры, чтобы не было возможности заходить игрокам с разными верси€ми.
        PhotonNetwork.GameVersion = "1";
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to master");
        Log("Connected to master");
    }

    private void Log(string message)
    {
        Debug.Log(message);
        logText.text += $"\n{message}";
    }

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(null, new Photon.Realtime.RoomOptions { MaxPlayers = 2 });
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinedRoom()
    {
        Log("Joined the room");
    }
}
