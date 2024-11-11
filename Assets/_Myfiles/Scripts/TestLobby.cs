using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies.Models;
using Unity.Services.Lobbies;
using UnityEngine;
using System.Threading.Tasks;
using System;

public class TestLobby : MonoBehaviour
{

    public static TestLobby Instance { get; private set; }


    private Lobby _hostLobby;
    private Lobby _joinedLobby;
    private float _hearbeatTimer;
    private float _lobbyUpdateTimer;
    private string playerName = "Ashton";

    private async void Start()
    {
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log($"Signed in {AuthenticationService.Instance.PlayerId}");
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    private void Update()
    {
        HandleLobbyHeartBeatAsync();
        HandleLobbyPollforUpdateAsync();
    }

    private async void HandleLobbyHeartBeatAsync()
    {
        if( _hostLobby != null )
        {
            _hearbeatTimer -= Time.deltaTime;
            if( _hearbeatTimer < 0f)
            {
                float heartbeatTimerMax = 15;
                _hearbeatTimer = heartbeatTimerMax;

                await LobbyService.Instance.SendHeartbeatPingAsync(_hostLobby.Id);
            }
        }
    }

    private async void HandleLobbyPollforUpdateAsync()
    {
        if (_joinedLobby != null)
        {
            _lobbyUpdateTimer -= Time.deltaTime;
            if (_lobbyUpdateTimer < 0f)
            {
                float lobbyUpdateTimerMax = 1.1f;
                _lobbyUpdateTimer = lobbyUpdateTimerMax;

                Lobby lobby = await LobbyService.Instance.GetLobbyAsync(_hostLobby.Id);
                _joinedLobby = lobby;   
            }
        }
    }

    private async void CreateLobby()
    {
        try
        {
            string lobbyName = "MyLobby";
            int maxPlayers = 4;

            CreateLobbyOptions lobbyOptions = new CreateLobbyOptions()
            {
                Player = GetPlayer(),
                Data = new Dictionary<string, DataObject>
                {
                    {"GameMode", new DataObject(DataObject.VisibilityOptions.Public, "TeamDeathMatch") }
                }
            };

            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers,lobbyOptions);
            
            _hostLobby = lobby;
            _joinedLobby = _hostLobby;

            Debug.Log($"Created Lobby! {lobby.Name} {lobby.MaxPlayers}");
            PrintPlayers(_hostLobby);
        }
        catch(LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }
    private async void ListLobbiesAsync()
    {
        try
        {

            QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync();

        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }
    private async void JoinLobbyByCode(string lobbyCode)
    {
        try
        {
            JoinLobbyByCodeOptions lobbyOption = new JoinLobbyByCodeOptions()
            {
                Player = GetPlayer()
            };
            Lobby lobby = await Lobbies.Instance.JoinLobbyByCodeAsync(lobbyCode, lobbyOption);
            _joinedLobby = lobby;

            Debug.Log("joined Lobby with code " + lobbyCode);
        }
        catch(LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }
    private Player GetPlayer()
    {
        return new Player
        {
            Data = new Dictionary<string, PlayerDataObject>
               {
                   {"PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, playerName)  }
               }
        };
    }

    private void PrintPlayers(Lobby lobby)
    {
        Debug.Log("Players in lobby" + lobby.Name);
        foreach(Player player in lobby.Players)
        {
            Debug.Log(player.Id + "" + player.Data["PlayerName"].Value);
        }
    }
    
    private async void UpdateLobbyGameMode(string gameMode)
    {
        try
        {
            _hostLobby = await Lobbies.Instance.UpdateLobbyAsync(_hostLobby.Id, new UpdateLobbyOptions
            {
                Data = new Dictionary<string, DataObject>
                {
                    {"GameMode", new DataObject(DataObject.VisibilityOptions.Public, gameMode) }
                }
            });

            _joinedLobby = _hostLobby;
        }
        catch(LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    private async void UpdatePlayerNameAsync(string newPlayerName)
    {
        try
        {
            playerName = newPlayerName;
            await LobbyService.Instance.UpdatePlayerAsync(_joinedLobby.Id, AuthenticationService.Instance.PlayerId, new UpdatePlayerOptions
            {
                Data = new Dictionary<string, PlayerDataObject>
                {
                    {"PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, playerName)  }
                }
            });
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }

    private async void LeaveLobby()
    {
        try
        {
            await LobbyService.Instance.RemovePlayerAsync(_joinedLobby.Id, AuthenticationService.Instance.PlayerId);
        }
        catch(Exception e)
        {
            Debug.Log(e);
        }
    }
}
