using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Отвечает за соединение к комнатам Photon.
/// </summary>
public class NetworkManager : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoomOnJoinedRoom");
        //Текущий игрок комнату.
        //SceneManager.LoadScene(0);
    }

    /// <summary>
    /// Срабатывает при нажатии на кнопку Выйти из комнаты.
    /// </summary>
    public void LeaveRoom()
    {
        GetButtonsAnimator animScript = GetButtonsAnimator.Instace;
        LobbyManagerUnity.IsNetwork = false;
        animScript.create.interactable = !LobbyManagerUnity.IsNetwork;
        animScript.join.interactable = !LobbyManagerUnity.IsNetwork;
        animScript.leave.interactable = LobbyManagerUnity.IsNetwork;
        PhotonNetwork.LeaveRoom();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"Player {newPlayer.NickName} entered the room.");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log($"Player {otherPlayer.NickName} left the room.");
    }
}
