using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.AI;

namespace Reversi
{
    public class ReversiUtils
    {
        private static int[] dx = { 1, 0, -1, 0, 1, 1, -1, -1 };
        private static int[] dy = { 0, 1, 0, -1, 1, -1, 1, -1 };

        // 評価関数
        private static ReadOnlyCollection<int> kEvaluations = System.Array.AsReadOnly(new int[] {
         30, -12,  0, -1, -1,  0, -12,  30,
        -12, -15, -3, -3, -3, -3, -15, -12,
          0,  -3,  0, -1, -1,  0,  -3,   0,
         -1,  -3, -1, -1, -1, -1,  -3,  -1,
         -1,  -3, -1, -1, -1, -1,  -3,  -1,
          0,  -3,  0, -1, -1,  0,  -3,   0,
        -12, -15, -3, -3, -3, -3, -15, -12,
         30, -12,  0, -1, -1,  0, -12,  30,
        });

        // 座標から盤面に対応するインデックスを得る
        public static int GetChipIndex(int x, int y)
        {
            return y * 8 + x;
        }

        // 次の石のタイプを取得する
        public static eStoneType NextStone(eStoneType type)
        {
            return (type == eStoneType.Black) ? eStoneType.White : eStoneType.Black;
        }

        // 盤面から移行できる次のノードを取得する
        public static List<GameTree> GetEnableMoveNodes(GameTree tree)
        {
            List<GameTree> res = new List<GameTree>();

            var enable_pos = GetEnableHands(tree.Board, tree.StoneType);

            foreach (var pos in enable_pos)
            {
                var get_stones = GetObtainStones(tree.Board, pos, tree.StoneType);
                Board board = new Board(tree.Board);
                foreach (var p in get_stones)
                {
                    board[p] = tree.StoneType;
                }
                res.Add(new GameTree(board, NextStone(tree.StoneType), pos, false));
            }

            // 置くところがなかった
            if (res.Count == 0 && !tree.PrevPassed) res.Add(new GameTree(new Board(tree.Board), NextStone(tree.StoneType), -1, true));

            return res;
        }

        // 現在の盤面から置くことのできる手を求める
        public static List<int> GetEnableHands(Board board, eStoneType next_type)
        {
            List<int> ret = new List<int>();

            for (int y = 0; y < 8; ++y)
            {
                for (int x = 0; x < 8; ++x)
                {
                    if (board[x, y] != eStoneType.None) continue;

                    bool can = false;

                    for (int i = 0; i < 8; ++i)
                    {
                        if (can) break;

                        int ty = y + dy[i];
                        int tx = x + dx[i];

                        if (ty < 0 || ty >= 8) continue;
                        if (tx < 0 || tx >= 8) continue;

                        if (board[tx, ty] == eStoneType.None || board[tx, ty] == next_type) continue;

                        int j = 2;
                        while (!can)
                        {
                            int tty = y + j * dy[i];
                            int ttx = x + j * dx[i];

                            if (tty < 0 || tty >= 8) break;
                            if (ttx < 0 || ttx >= 8) break;
                            if (board[ttx, tty] == eStoneType.None) break;
                            if (board[ttx, tty] == next_type) can = true;
                            ++j;
                        }
                    }

                    if (can) ret.Add(GetChipIndex(x, y)); // 置ける
                }
            }

            return ret;
        }

        // 盤面に石を置いて得られる石の座標を取得する
        public static List<int> GetObtainStones(Board board, int pos, eStoneType type)
        {
            int x = pos % 8;
            int y = pos / 8;

            List<int> get_stones_ = new List<int>();

            for (int i = 0; i < 8; ++i)
            {

                bool can = false;
                List<int> new_pos = new List<int>();

                int j = 1;
                while (!can)
                {
                    int tx = x + j * dx[i];
                    int ty = y + j * dy[i];

                    if (tx < 0 || tx >= 8) break;
                    if (ty < 0 || ty >= 8) break;
                    int index = GetChipIndex(tx, ty);
                    if (board[index] == eStoneType.None) break;
                    if (board[index] == type)
                    {
                        can = true;
                        break;
                    }
                    // 置く
                    new_pos.Add(index);
                    ++j;
                }

                if (can)
                {
                    foreach (var p in new_pos)
                    {
                        get_stones_.Add(p);
                    }
                }
            }

            // 自分自身
            get_stones_.Add(pos);

            return get_stones_;
        }

        // 現在の盤面の評価値を計算する
        public static List<int> CalcScore(Board board)
        {
            List<int> ret = new List<int>(2);
            ret.Add(0);
            ret.Add(0);

            // 0が黒の評価値
            // 1が白の評価値

            for(int i = 0; i < 64; ++i)
            {
                int type = (int)board[i];
                if (type == (int)eStoneType.None) continue;

                ret[type - 1] += kEvaluations[i];
            }

            return ret;
        }
    }
}