using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject confirmPanel;
    [SerializeField] private GameObject signupPanel;
    [SerializeField] private GameObject signinPanel;
    [SerializeField] private GameObject leaderboardPanel;
    
    private GameUIController _gameUIController;
    private Canvas _canvas;
    
    private Constants.GameType _gameType;
    
    private GameLogic _gameLogic;
    
    private string _roomId;
    private bool _isMyTurn;

    private bool _isOnGame;

    private void Start()
    {
        // 로그인 
        NetworkManager.Instance.GetScore();
    }

    public void ChangeToGameScene(Constants.GameType gameType)
    {
        _gameType = gameType;
        
        SceneManager.LoadScene("Game");
    }

    public void ChangeToMainScene()
    {
        _gameLogic?.Dispose();
        _gameLogic = null;
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

    public void OnSignOut()
    {
        StartCoroutine(NetworkManager.Instance.SignOut(() =>
        {
            OpenSigninPanel();
        }, () =>
        {
            OpenConfirmPanel("로그아웃 실패", () =>
            {
                
            });
        }));
    }

    public void OpenGameOverPanel()
    {
        _gameUIController.SetGameUIMode(GameUIController.GameUIMode.GameOver);
    }
    
    protected override void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Game")
        {
            // 씬에 배치된 오브젝트 찾기 (BlockController, GameUIController)
             var blockController = FindObjectOfType<BlockController>();
            _gameUIController = FindObjectOfType<GameUIController>();
            
            // BlockController 초기화 
            blockController.InitBlocks();
            
            // GameUI 초기화
            _gameUIController.SetGameUIMode(GameUIController.GameUIMode.Init);
            
            // GameLogic 객체 생성 
            _gameLogic = new GameLogic(blockController, _gameType);
        }
        
        _canvas = FindObjectOfType<Canvas>();
    }

    private void OnApplicationQuit()
    {
        _gameLogic?.Dispose();
        _gameLogic = null;
    }
}
