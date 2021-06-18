using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    private List<RoomInfo> roomList = new List<RoomInfo>();
    static private string typeOfPlayer = "";
    private string enemyTypeOfPlayer = "";

    public Button heroesButton;
    public Button monstersButton;
    public Button cancelQueueButton;
    public Button backButton;
    public GameObject queueText;
    public Button exitButton;
    public enum QueueState
    {
        Queuing,
        Queued,
        Starting,
        Cancelled,
    }

    // Type of lobby
    private TypedLobby sqlLobby = new TypedLobby("customSqlLobby", LobbyType.SqlLobby);

    // Sql column that will be used to match the players (Hero player vs Monster player)
    public const string TYPE_PROP_KEY = "C0";

    public void FindGameAs(string typeOfPlayer)
    {
        // When looking for a game, diable queue and menu buttons
        changeStateQueueButtons(false);

        // Set the type of player
        NetworkManager.typeOfPlayer = typeOfPlayer;

        // Set the type of units that the enemy will control
        if (typeOfPlayer == "Heroes")
        {
            enemyTypeOfPlayer = "Monsters";
        }
        else
        {
            enemyTypeOfPlayer = "Heroes";
        }

        // Connect to the server
        ConnectedToServer();
    }

    void ConnectedToServer()
    {
        // Change the queue display text
        UpdateQueueText(QueueState.Queuing);
        
        // Connect to photon Network
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
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
        base.OnJoinedRoom();

        // If there is 2 players, load the game
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            // Update the queue display text
            UpdateQueueText(QueueState.Starting);

            // Disable cancel queue button
            cancelQueueButton.gameObject.SetActive(false);

            // Load the scene
            LoadLevel("Match");
        }
        else
        {
            // Update the queue display text
            UpdateQueueText(QueueState.Queued);

            // Enable cancel queue button
            cancelQueueButton.gameObject.SetActive(true);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);

        // If there is 2 players, load the game and hide the room
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            // Update the queue display text
            UpdateQueueText(QueueState.Starting);

            // Disable cancel queue button
            cancelQueueButton.gameObject.SetActive(false);

            PhotonNetwork.CurrentRoom.IsVisible = false;
            PhotonNetwork.CurrentRoom.IsOpen = false;

            // Load scene
            LoadLevel("Match");
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause); 
    }

    public void LoadLevel(string levelName)
    {
        // Load the game
        PhotonNetwork.LoadLevel(levelName);
    }

    static public string GetTypeOfPlayer()
    {
        // Return the type of player
        return typeOfPlayer;
    }

    private void UpdateQueueText(QueueState queueState)
    {
        // If the player is in any state of the queue, activate the text and update it
        if (queueState == QueueState.Queued)
        {
            queueText.GetComponent<TextMesh>().text = "Queued as " + GetTypeOfPlayer();
            queueText.SetActive(true);
        }
        else if (queueState == QueueState.Queuing)
        {
            queueText.GetComponent<TextMesh>().text = "Queuing";
            queueText.SetActive(true);
        }
        else if (queueState == QueueState.Starting)
        {
            queueText.GetComponent<TextMesh>().text = "Starting match";
            queueText.SetActive(true);
        }
        else // If he is not in queue, disable the text
        {
            queueText.GetComponent<TextMesh>().text = "";
            queueText.SetActive(false);
        }
    }

    private void changeStateQueueButtons(bool active)
    {
        // Enable or disable the main menu buttons
        heroesButton.gameObject.SetActive(active);
        monstersButton.gameObject.SetActive(active);
        backButton.gameObject.SetActive(active);
        exitButton.gameObject.SetActive(active);
    }

    public void CancelQueue()
    {
        // Disconnect from the server
        PhotonNetwork.Disconnect();

        // Update the queue display text
        UpdateQueueText(QueueState.Cancelled);

        // Disabble the cancel queue button
        cancelQueueButton.gameObject.SetActive(false);

        // Enable the main menu buttons
        changeStateQueueButtons(true);
    }
}
