using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using SocketIOClient;
using UnityEditor;

public class RoomData
{
    [JsonProperty("roomId")]
    public string roomId { get; set; }
}

public class UserData
{
    [JsonProperty("userId")]
    public string userId { get; set; }
}

public class MessageData
{
    [JsonProperty("nickName")]
    public string nickName { get; set; }
    [JsonProperty("message")]
    public string message { get; set; }
}

public class MoveData
{
    [JsonProperty("position")] 
    public int position { get; set; }
}

public class MultiplayManager : IDisposable
{
    private SocketIOUnity _socket;

    private event Action<Constants.MultiplayManagerState, string> _onMultiplayStateChanged;
    public Action<MessageData> OnReceiveMessage;
    public Action<MoveData> OnOpponentMove;
   
    public MultiplayManager(Action<Constants.MultiplayManagerState, string>  onMultiplayStateChanged)
    {
        _onMultiplayStateChanged = onMultiplayStateChanged;
        
        var uri = new Uri(Constants.GameServerURL);
        _socket = new SocketIOUnity(uri, new SocketIOOptions
        {
            Transport = SocketIOClient.Transport.TransportProtocol.WebSocket
        });
        
        _socket.On("createRoom", CreateRoom);
        _socket.On("joinRoom", JoinRoom);
        _socket.On("startGame", StartGame);
        _socket.On("exitRoom", ExitRoom);
        _socket.On("endGame", EndGame);
        _socket.On("doOpponent", DoOpponent);
        _socket.On("receiveMessage", ReceiveMessage);
        
        _socket.Connect();
    }

    private void CreateRoom(SocketIOResponse response)
    {
        var data = response.GetValue<RoomData>();
        _onMultiplayStateChanged?.Invoke(Constants.MultiplayManagerState.CreateRoom, data.roomId);
    }

    private void JoinRoom(SocketIOResponse response)
    {
        var data = response.GetValue<RoomData>();
        _onMultiplayStateChanged?.Invoke(Constants.MultiplayManagerState.JoinRoom, data.roomId);
    }

    private void StartGame(SocketIOResponse response)
    {
        var data = response.GetValue<RoomData>();
        _onMultiplayStateChanged?.Invoke(Constants.MultiplayManagerState.StartGame, data.roomId);
    }

    private void ExitRoom(SocketIOResponse response)
    {
        _onMultiplayStateChanged?.Invoke(Constants.MultiplayManagerState.ExitRoom, null);
    }

    private void EndGame(SocketIOResponse response)
    {
        _onMultiplayStateChanged?.Invoke(Constants.MultiplayManagerState.EndGame, null);
    }

    // 서버로부터 상대방의 마커 정보를 받기 위한 메서드 
    private void DoOpponent(SocketIOResponse response)
    {
        var data = response.GetValue<MoveData>();
        OnOpponentMove?.Invoke(data);
    }

    // 플레이어의 마커 위치를 서버로 전달하기 위한 메서드 
    public void SendPlayerMove(string roomId, int position)
    {
        _socket.Emit("doPlayer", new { roomId, position });
    }

    private void ReceiveMessage(SocketIOResponse response)
    {
        var data = response.GetValue<MessageData>();
        OnReceiveMessage?.Invoke(data);
    }

    public void LeaveRoom(string roomId)
    {
        _socket.Emit("leaveRoom", new { roomId });
    }

    public void SendMessage(string roomId, string nickName, string message)
    {
        _socket.Emit("sendMessage", new {roomId, nickName, message});
    }

    // private void ReceiveMove(SocketIOResponse response)
    // {
    //     var data = response.GetValue<MoveData>();
    //     OnReceiveMove?.Invoke(data);
    // }

    // public void SendMove(string roomId, GameManager.PlayerType playerType, GameManager.TurnType turnType, int row, int col)
    // {
    //     _socket.Emit("sendMove", new { roomId, playerType, turnType, row, col });
    // }
    
    public void Dispose()
    {
        if (_socket != null)
        {
            _socket.Disconnect();
            _socket.Dispose();
            _socket = null;
        }
    }
}
