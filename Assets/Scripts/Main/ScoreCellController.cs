using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreCellController : MonoBehaviour
{
    [SerializeField] private TMP_Text nicknameText;
    [SerializeField] private TMP_Text scoreText;

    public void SetCellInfo(ScoreInfo scoreInfo)
    {
        nicknameText.text = scoreInfo.nickname;
        scoreText.text = scoreInfo.score.ToString();
    }
}
