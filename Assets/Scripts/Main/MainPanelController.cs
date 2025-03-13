using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainPanelController : MonoBehaviour
{
    public void OnClickSinglePlayButton()
    {
        GameManager.Instance.ChangeToGameScene(Constants.GameType.SinglePlayer);
    }

    public void OnClickDualPlayButton()
    {
       GameManager.Instance.ChangeToGameScene(Constants.GameType.DualPlayer);
    }

    public void OnClickMultiPlayButton()
    {
        GameManager.Instance.ChangeToGameScene(Constants.GameType.MultiPlayer);
    }

    public void OnClickSettingsButton()
    {
        GameManager.Instance.OpenSettingsPanel();
    }

    public void OnClickLeaderboardButton()
    {
        GameManager.Instance.OpenLeaderboardPanel();
    }

    public void OnClickSignOutButton()
    {
        GameManager.Instance.OnSignOut();
    }
}
