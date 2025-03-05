using System;

public static class MinimaxAIController
{
    public static (int row, int col)? GetBestMove(GameManager.PlayerType[,] board)
    {
        float bestScore = -1000;
        (int row, int col)? bestMove = null;
        
        for (int row = 0; row < board.GetLength(0); row++)
        {
            for (int col = 0; col < board.GetLength(1); col++)
            {
                if (board[row, col] == GameManager.PlayerType.None)
                {
                    board[row, col] = GameManager.PlayerType.PlayerB;
                    var score = DoMinimax(board, 0, false);
                    board[row, col] = GameManager.PlayerType.None;

                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestMove = (row, col);
                    }
                }
            }
        }

        return bestMove;
    }
    private static float DoMinimax(GameManager.PlayerType[,] board, int depth, bool isMaximizing)
    {
        if (CheckGameWin(GameManager.PlayerType.PlayerA, board))
        {
            return -10f + depth;
        }

        if (CheckGameWin(GameManager.PlayerType.PlayerB, board))
        {
            return 10f - depth;
        }

        if (IsAllBlockPlaced(board))
        {
            return 0f;
        }

        if (isMaximizing)
        {
            var bestScore = float.MinValue;
            for (int row = 0; row < board.GetLength(0); row++)
            {
                for (int col = 0; col < board.GetLength(1); col++)
                {
                    if (board[row, col] == GameManager.PlayerType.None)
                    {
                        board[row, col] = GameManager.PlayerType.PlayerB;
                        var score = DoMinimax(board, depth + 1, false);
                        board[row, col] = GameManager.PlayerType.None;
                        bestScore = Math.Max(bestScore, score);
                    }
                }
            }
            return bestScore;
        }
        else
        {
            var bestScore = float.MaxValue;
            for (int row = 0; row < board.GetLength(0); row++)
            {
                for (int col = 0; col < board.GetLength(1); col++)
                {
                    if (board[row, col] == GameManager.PlayerType.None)
                    {
                        board[row, col] = GameManager.PlayerType.PlayerA;
                        var score = DoMinimax(board, depth + 1, true);
                        board[row, col] = GameManager.PlayerType.None;
                        bestScore = Math.Min(bestScore, score);
                    }
                }
            }
            return bestScore;
        }
    }
    
    public static bool IsAllBlockPlaced(GameManager.PlayerType[,] board)
    {
        for (var row = 0; row < board.GetLength(0); row++)
        {
            for (var col = 0; col < board.GetLength(1); col++)
            {
                if(board[row,col] == GameManager.PlayerType.None)
                    return false;
            }
        }
        return true;
    }
    
    private static bool CheckGameWin(GameManager.PlayerType playerType, GameManager.PlayerType[,] board)
    {
        // 행 확인
        for (var row = 0; row < board.GetLength(0); row++)
        {
            if(board[row,0] == playerType && board[row, 1] == playerType && board[row,2] == playerType)
            {
                return true;
            }
        }

        // 열 확인
        for (var col = 0; col < board.GetLength(1); col++)
        {
            if (board[0, col] == playerType && board[1, col] == playerType && board[2, col] == playerType)
            {
                return true;
            }
        }

        // 대각선 확인
        if (board[0, 0] == playerType && board[1, 1] == playerType && board[2, 2] == playerType)
        {
            return true;
        }
        if (board[0, 2] == playerType && board[1, 1] == playerType && board[2, 0] == playerType)
        {
            return true;
        }
        
        return false;
    }
}
