using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class LeaderBoardController : MonoBehaviour
{
    [SerializeField] private GameObject scoreCell;
    [SerializeField] private Transform content;

    public void CreateCell(ScoreInfo scoreInfo)
    {
        var scoreCellObject = Instantiate(scoreCell, content);
        var scoreCellController = scoreCellObject.GetComponent<ScoreCellController>();
        scoreCellController.SetCellInfo(scoreInfo);
    }
}
