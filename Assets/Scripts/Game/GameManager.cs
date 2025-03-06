using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject confirmPanel;
    [SerializeField] private GameObject signupPanel;
    [SerializeField] private GameObject signinPanel;
    [SerializeField] private GameObject leaderboardPanel;
    
    private BlockController _blockController;
    private GameUIController _gameUIController;
    private Canvas _canvas;
    
    public enum PlayerType { None, PlayerA, PlayerB }
    private PlayerType[,] _board;

    private enum TurnType {PlayerA, PlayerB}

    public enum GameResult
    {
        None,   // 게임 진행중
        Win,    // 플레이어 승
        Lose,   // 플레이어 패
        Draw    // 비김 
    }
    
    public enum GameType{SinglePlayer, DualPlayer}
    private GameType _gameType;

    private void Start()
    {
        // 로그인 
        NetworkManager.Instance.GetScore();
    }

    public void ChangeToGameScene(GameType gameType)
    {
        _gameType = gameType;
        SceneManager.LoadScene("Game");
    }

    public void ChangeToMainScene()
    {
        SceneManager.LoadScene("Main");
    }

    public void OpenSettingsPanel()
    {
        if (_canvas != null)
        {
            var settingsPanelObject = Instantiate(settingsPanel, _canvas.transform);
            settingsPanelObject.GetComponent<PanelController>().Show();
        }
    }

    public void OpenConfirmPanel(string message, ConfirmPanelController.OnConfirmButtonClick onConfirmButtonClick)
    {
        if (_canvas != null)
        {
            var confirmPanelObject = Instantiate(confirmPanel, _canvas.transform);
            confirmPanelObject.GetComponent<ConfirmPanelController>().Show(message, onConfirmButtonClick);
        }
    }

    public void OpenSigninPanel()
    {
        if (_canvas != null)
        {
            var signinPanelObject = Instantiate(signinPanel, _canvas.transform);
        }
    }

    public void OpenSignupPanel()
    {
        if (_canvas != null)
        {
            var signupPanelObject = Instantiate(signupPanel, _canvas.transform);
            
        }
    }

    public void OpenLeaderboardPanel()
    {
        if (_canvas != null)
        {
            var leaderboardPanelObject = Instantiate(leaderboardPanel, _canvas.transform);
            
            StartCoroutine(NetworkManager.Instance.GetLeaderboard(
                ranks => 
                {
                    foreach (var rank in ranks.scores)
                    {
                        //Debug.Log($"닉네임: {rank.nickname}, 점수: {rank.score}");
                        var leaderboardController = leaderboardPanelObject.GetComponent<LeaderBoardController>();
                        leaderboardController.CreateCell(rank);
                    }
                }, 
                () =>
                {
                    Debug.LogError("랭킹 가져오기 실패: ");
                }));
        }
    }

    /// <summary>
    /// 게임 시작
    /// </summary>
    private void StartGame()
    {
        // _board 초기화
        _board = new PlayerType[3, 3];
        
        // 블록 초기화
        _blockController.InitBlocks();
        
        // GameUI 초기화
        _gameUIController.SetGameUIMode(GameUIController.GameUIMode.Init);

        SetTurn(TurnType.PlayerA);
    }

    /// <summary>
    /// 게임 오버시 호출되는 함수
    /// gameResult에 따라 결과 출력
    /// 
    /// </summary>
    /// <param name="gameResult">win, lose, draw</param>
    public void EndGame(GameResult gameResult)
    {
        // 게임오버 표시
        _gameUIController.SetGameUIMode(GameUIController.GameUIMode.GameOver);
        
        // TODO: 나중에 구현!!
        
        switch (gameResult)
        {
            case GameResult.Win:
                Debug.Log("Player A Wins!");
                break;
            case GameResult.Lose:
                Debug.Log("Player B Wins!");
                break;
            case GameResult.Draw:
                Debug.Log("Draw!");
                break;
        }
    }

    /// <summary>
    /// _board에 값을 할당하는 함수
    /// </summary>
    /// <param name="playerType">할당하고자 하는 플레이어타입</param>
    /// <param name="row">Row</param>
    /// <param name="col">Col</param>
    /// <returns>False가 반환되면 할당할 수 없음, True는 할당이 완료됨</returns>
    private bool SetNewBoardValue(PlayerType playerType, int row, int col)
    {
        if(_board[row, col] != PlayerType.None) return false;
        
        if (playerType == PlayerType.PlayerA)
        {
            _board[row, col] = playerType;
            _blockController.PlaceMarker(Block.MarkerType.O, row, col);
            return true;
        }
        else if (playerType == PlayerType.PlayerB)
        {
            _board[row, col] = playerType;
            _blockController.PlaceMarker(Block.MarkerType.X, row, col);
            return true;
        }

        return false;
    }
    private void SetTurn(TurnType turnType)
    {
        switch (turnType)
        {
            case TurnType.PlayerA:
                _gameUIController.SetGameUIMode(GameUIController.GameUIMode.TurnA);
                _blockController.onBlockClickedDelegate = (row, col) =>
                {
                    if (SetNewBoardValue(PlayerType.PlayerA, row, col))
                    {
                        var gameResult = CheckGameResult();
                        if (gameResult == GameResult.None)
                        {
                            SetTurn(TurnType.PlayerB);
                        }
                        else
                            EndGame(gameResult);
                    }
                    else
                    {
                        // TODO : 이미 있는 곳을 터치했을 때 처리
                    }
                };
                
                break;
            case TurnType.PlayerB:
                _gameUIController.SetGameUIMode(GameUIController.GameUIMode.TurnB);

                if (_gameType == GameType.SinglePlayer)
                {
                    var result = MinimaxAIController.GetBestMove(_board);
                    if (result.HasValue)
                    {
                        if (SetNewBoardValue(PlayerType.PlayerB, result.Value.row, result.Value.col))
                        {
                            var gameResult = CheckGameResult();
                            if (gameResult == GameResult.None)
                            {
                                SetTurn(TurnType.PlayerA);
                            }
                            else
                                EndGame(gameResult);
                        }
                        else
                        {
                            // TODO : 이미 있는 곳을 터치했을 때 처리
                        }
                    }
                }
                else if (_gameType == GameType.DualPlayer)
                {
                    _blockController.onBlockClickedDelegate = (row, col) =>
                    {
                        if (SetNewBoardValue(PlayerType.PlayerB, row, col))
                        {
                            var gameResult = CheckGameResult();
                            if (gameResult == GameResult.None)
                            {
                                SetTurn(TurnType.PlayerA);
                            }
                            else
                                EndGame(gameResult);
                        }
                        else
                        {
                            // TODO : 이미 있는 곳을 터치했을 때 처리
                        }
                    };
                }
                
                break;
        }
    }

    /// <summary>
    /// 게임 결과 확인 함수
    /// </summary>
    /// <returns>플레이어 기준 결과</returns>
    private GameResult CheckGameResult()
    {
        if (CheckGameWin(PlayerType.PlayerA))
        {
            StartCoroutine(NetworkManager.Instance.AddScore(10, () =>
            {
                
            }, () =>
            {

            }));
            return GameResult.Win;
        }

        if (CheckGameWin(PlayerType.PlayerB)) { return GameResult.Lose; }

        if (MinimaxAIController.IsAllBlockPlaced(_board)) { return GameResult.Draw; }

        return GameResult.None;
    }
    
    // 게임의 승패를 판단하는 함수
    private bool CheckGameWin(PlayerType playerType)
    {
        // 행 확인
        for (var row = 0; row < _board.GetLength(0); row++)
        {
            if(_board[row,0] == playerType && _board[row, 1] == playerType && _board[row,2] == playerType)
            {
                (int, int)[] blocks = { (row, 0), (row, 1), (row, 2) };
                _blockController.SetBlockColor(playerType, blocks);
                return true;
            }
        }

        // 열 확인
        for (var col = 0; col < _board.GetLength(1); col++)
        {
            if (_board[0, col] == playerType && _board[1, col] == playerType && _board[2, col] == playerType)
            {
                (int, int)[] blocks = { (0, col), (1, col), (2, col) };
                _blockController.SetBlockColor(playerType, blocks);
                return true;
            }
        }

        // 대각선 확인
        if (_board[0, 0] == playerType && _board[1, 1] == playerType && _board[2, 2] == playerType)
        {
            (int, int)[] blocks = { (0, 0), (1, 1), (2, 2) };
            _blockController.SetBlockColor(playerType, blocks);
            return true;
        }
        if (_board[0, 2] == playerType && _board[1, 1] == playerType && _board[2, 0] == playerType)
        {
            (int, int)[] blocks = { (0, 2), (1, 1), (2, 0) };
            _blockController.SetBlockColor(playerType, blocks);
            return true;
        }
        
        return false;
    }

    protected override void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Game")
        {
            _blockController = FindObjectOfType<BlockController>();
            _gameUIController = FindObjectOfType<GameUIController>();
            
            // 게임 시작  (게임씬에 들어왔을 때만 실행 되도록)
            StartGame();
        }
        
        _canvas = FindObjectOfType<Canvas>();
    }
}
