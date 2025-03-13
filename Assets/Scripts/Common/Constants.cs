public class Constants
{
    public const string ServerURL = "https://immense-scallop-hynje-e7187e78.koyeb.app";
    public const string GameServerURL = "ws://immense-scallop-hynje-e7187e78.koyeb.app";
    
    public const string TestServerURL = "http://localhost:3000";
    public const string TestGameServerURL = "ws://localhost:3000";
    
    public enum MultiplayManagerState
    {
        CreateRoom,
        JoinRoom,
        StartGame,
        ExitRoom,
        EndGame
    }
    
    public enum PlayerType { None, PlayerA, PlayerB }
    public enum GameType{SinglePlayer, DualPlayer, MultiPlayer}
}