using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Менеджер лобби отвечает за подключение к серверу Photon.
/// </summary>
public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Text logText;

    // Start is called before the first frame update
    void Start()
    {
        //Ник пользователя.
        PhotonNetwork.NickName = $"User{Random.Range(1, 1000)}";
        PhotonNetwork.AutomaticallySyncScene = true;
        //Версия игры, чтобы не было возможности заходить игрокам с разными версиями.
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

    /// <summary>
    /// Срабатывает на кнопку Создать комнату.
    /// </summary>
    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(null, new Photon.Realtime.RoomOptions { MaxPlayers = 2 });
    }

    /// <summary>
    /// Срабатывает на кнопку Присоединиться к комнате.
    /// </summary>
    public void JoinRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinedRoom()
    {
        Log("Joined the room");
        PhotonNetwork.LoadLevel("ARCoreScene");
    }
}
