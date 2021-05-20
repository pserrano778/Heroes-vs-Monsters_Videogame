using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    private List<RoomInfo> roomList = new List<RoomInfo>();
    private string typeOfPlayer = "";
    private string enemyTypeOfPlayer = "";

    // Type of lobby
    private TypedLobby sqlLobby = new TypedLobby("customSqlLobby", LobbyType.SqlLobby);

    // Sql column that will be used to match the players (Hero player vs Monster player)
    public const string TYPE_PROP_KEY = "C0";

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.JoinLobby();
    }

    public void FindGameAs(string typeOfPlayer)
    {
        this.typeOfPlayer = typeOfPlayer;

        // Set the type of units that the enemy will control
        if (typeOfPlayer == "Heroes")
        {
            enemyTypeOfPlayer = "Monsters";
        }
        else
        {
            enemyTypeOfPlayer = "Heroes";
        }

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

        // Try to Join to a random Room
        JoinRandomRoom();
    }

    private void JoinRandomRoom()
    {
        // Set the type of unit that the player will control
        string sqlLobbyFilter = "C0 = " + "'" + typeOfPlayer + "'";

        // Try to find a room that match the filter
        PhotonNetwork.JoinRandomRoom(null, 0, MatchmakingMode.FillRoom, sqlLobby, sqlLobbyFilter);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        // If the player hasn't found any room, create a new one
        Debug.Log("Creating a Room");
        CreateRoom();

    }
    private void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();

        // Set basic options
        roomOptions.MaxPlayers = 2;
        roomOptions.IsVisible = true;
        roomOptions.IsOpen = true;

        // Set custom Options (type of enemy units that the player will face)
        roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable { { TYPE_PROP_KEY, enemyTypeOfPlayer } };

        // Set the sql column in the lobby
        roomOptions.CustomRoomPropertiesForLobby = new string[]{ TYPE_PROP_KEY };

        // create the room
        PhotonNetwork.CreateRoom(null, roomOptions, sqlLobby);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("joined a Room");
        base.OnJoinedRoom();
        print(PhotonNetwork.CurrentRoom.CustomProperties.ToString());

        // If there is 2 players, load the game
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            LoadLevel("Test");
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("A new player has joined.");
        base.OnPlayerEnteredRoom(newPlayer);

        // If there is 2 players, load the game and hide the room
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            PhotonNetwork.CurrentRoom.IsVisible = false;
            PhotonNetwork.CurrentRoom.IsOpen = false;
            LoadLevel("Test");
        }
    }

    public void LoadLevel(string levelName)
    {
        // Load the game
        SceneManager.LoadScene(levelName);
    }
}
