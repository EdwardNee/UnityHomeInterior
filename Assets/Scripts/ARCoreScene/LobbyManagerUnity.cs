using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ��������-����� �������� � ������������� � �������.
/// </summary>
public class LobbyManagerUnity : MonoBehaviourPunCallbacks
{
    //������������ �� ����������� ��� ���.
    private static bool isNetwork = false;

    //����� �����������.
    [SerializeField]
    private Text logText;

    /// <summary>
    /// �������� ������������ �� ����������� ��� ���
    /// </summary>
    public static bool IsNetwork
    {
        get
        {
            return isNetwork;
        }
        set
        {
            isNetwork = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //��� ������������.
        PhotonNetwork.NickName = $"User{Random.Range(1, 1000)}";
        PhotonNetwork.AutomaticallySyncScene = true;
        //������ ����, ����� �� ���� ����������� �������� ������� � ������� ��������.
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
        IsNetwork = true;
        Log("Joined the room");
    }
}
