using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainPanelController : MonoBehaviour
{
    public void OnClickSinglePlayButton()
    {
        GameManager.Instance.ChangeToGameScene(GameManager.GameType.SinglePlayer);
    }

    public void OnClickDualPlayButton()
    {
       GameManager.Instance.ChangeToGameScene(GameManager.GameType.DualPlayer);
    }

    public void OnClickSettingsButton()
    {
        GameManager.Instance.OpenSettingsPanel();
    }

    public void OnClickLeaderboardButton()
    {
        //NetworkManager.Instance.GetScore();
        GameManager.Instance.OpenLeaderboardPanel();
        StartCoroutine(NetworkManager.Instance.GetLeaderboard(
            ranks => 
            {
                foreach (var rank in ranks.scores)
                {
                    Debug.Log($"닉네임: {rank.nickname}, 점수: {rank.score}");
                }
            }, 
            () =>
            {
                Debug.LogError("랭킹 가져오기 실패: ");
            }));
    }
}
