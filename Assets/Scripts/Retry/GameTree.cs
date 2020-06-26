using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲーム中の局面
/// </summary>

namespace Reversi
{
    public class GameTree
    {
        // 盤面の状況
        public Board Board { private set; get; }

        // 次はどっちの手か
        public eStoneType StoneType { private set; get; }

        // 前回どこに置いたか
        public int PrevPos { private set; get; }

        // パスをした回数
        public bool PrevPassed { private set; get; }

        // 次に移行できるノード
        private List<GameTree> enable_move_nodes_;

        // この盤面の評価値 0が黒の評価値 1が白の評価値
        public List<int> Score { private set; get; }
        // 黒と白の相対評価値を返す(黒 - 白)
        public int ScoreDiff => Score[0] - Score[1];

        // 次に移行できるノードを取得する
        public List<GameTree> GetEnableMoveNodes()
        {
            if (enable_move_nodes_ == null) enable_move_nodes_ = ReversiUtils.GetEnableMoveNodes(this);
            return enable_move_nodes_;
        }

        // ゲーム木を構成する
        public GameTree(Board board, eStoneType type, int pos, bool passed)
        {
            Board = new Board(board);
            StoneType = type;
            PrevPos = pos;
            PrevPassed = passed;
            enable_move_nodes_ = null;
            Score = ReversiUtils.CalcScore(Board);
        }

        // ゲーム木を構成する
        public GameTree(GameTree tree)
        {
            Board = new Board(tree.Board);
            StoneType = tree.StoneType;
            PrevPos = tree.PrevPos;
            PrevPassed = tree.PrevPassed;
            enable_move_nodes_ = tree.enable_move_nodes_;
            Score = tree.Score;
        }
    }
}