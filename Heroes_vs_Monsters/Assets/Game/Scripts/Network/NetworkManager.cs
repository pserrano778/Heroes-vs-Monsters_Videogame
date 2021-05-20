using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    private List<RoomInfo> roomList = new List<RoomInfo>();
    // Start is called before the first frame update
    void Start()
    {
        ConnectedToServer();
    }

    void ConnectedToServer()
    {
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("Try Connect to Server...");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connect To Server.");
        

        base.OnConnectedToMaster();
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        roomOptions.IsVisible = true;
        roomOptions.IsOpen = true;
        PhotonNetwork.JoinLobby(TypedLobby.Default);
        TypedLobby lobby = PhotonNetwork.CurrentLobby;
        
        string room = "";
        print(roomList);

        if (roomList.Count > 0 && roomList[roomList.Count-1].PlayerCount == 1)
        {
            room = "Room" + (PhotonNetwork.CountOfRooms);
        }
        else
        {
            room = "Room" + (PhotonNetwork.CountOfRooms + 1);
        }
        print("contador: " + roomList.Count);
        PhotonNetwork.JoinOrCreateRoom(room, roomOptions, TypedLobby.Default);
        
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("joined a Room");
        base.OnJoinedRoom();
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            LoadLevel("Test");
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("A new player has joined.");
        base.OnPlayerEnteredRoom(newPlayer);
        if(PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            LoadLevel("Test");
        }
    }

    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);
        
        this.roomList = roomList;
        print(" dsds: " + roomList.Count);
    }
}
