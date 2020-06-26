using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml;
using UnityEngine;

/// <summary>
/// いまはMiniMaxだけど，AlphaBetaとの差は計算時間だけなので時間あるときやる
/// </summary>

namespace Reversi
{
    public class AlphaBetaPlayer : BasePlayer
    {
        // 何手先まで読むか
        [SerializeField]
        private int depth_ = 3;

        //private int MiniMax(int hand, int depth, eStoneType type, List<eStoneType> boards, ReadOnlyCollection<int> evalutions)
        //{
        //    if (depth == 0)
        //    {
        //        // 手を評価する
        //        // 盤面をすすめる
        //        List<eStoneType> new_board = new List<eStoneType>(boards);

        //        List<int> get_stones = ReversiUtils.GetObtainStones(new_board, hand, type);
        //        int value = 0;
        //        foreach (var pos in get_stones) value += evalutions[pos];
        //        return value;

        //        // return evalutions[hand];
        //    }

        //    // 置ける手を取得
        //    var enable_hands = ReversiUtils.GetEnableHands(boards, type);

        //    int inf = 100000000;

        //    // 大きくするか小さくするか
        //    bool target_max = ((depth_ - depth) % 2 == 0);
        //    //Debug.Log((target_max) ? "大きくしたい" : "小さくしたい");

        //    // もう置けない
        //    if (enable_hands.Count == 0) return (target_max) ? -inf : inf;

        //    int best_value = (target_max) ? -inf : inf;

        //    foreach (var pos in enable_hands) 
        //    {
        //        // 盤面をすすめる
        //        List<eStoneType> new_board = new List<eStoneType>(boards);

        //        List<int> get_stones = ReversiUtils.GetObtainStones(new_board, pos, type);

        //        foreach (var index in get_stones)
        //        {
        //            new_board[index] = type;
        //        }

        //        eStoneType next_type = eStoneType.White;
        //        if (type == eStoneType.White) next_type = eStoneType.Black;

        //        int val = MiniMax(pos, depth - 1, next_type, new_board, evalutions);

        //        if (target_max)
        //        {
        //            if(val >= best_value)
        //            {
        //                best_value = val;
        //            }
        //        }
        //        else
        //        {
        //            if(-val <= best_value)
        //            {
        //                best_value = -val;
        //            }
        //        }
        //    }

        //    //Debug.Log(((target_max) ? "大きくしたい" : "小さくしたい") + " => " + best_value);
        //    return best_value; 
        //}

        //public override int Play(eStoneType type, List<int> hands, List<eStoneType> boards, ReadOnlyCollection<int> evalutions)
        //{
        //    // 完全にMinimaxの再帰関数で回す予定だったけど

        //    int best_value = -10000000;

        //    // 最大の評価値からランダムにする
        //    Dictionary<int, List<int>> dicts = new Dictionary<int, List<int>>();

        //    // 今ある手を全探索
        //    foreach(var pos in hands)
        //    {
        //        // 盤面をすすめる
        //        List<eStoneType> new_board = new List<eStoneType>(boards);

        //        List<int> get_stones = ReversiUtils.GetObtainStones(new_board, pos, type);

        //        foreach (var index in get_stones)
        //        {
        //            new_board[index] = type;
        //        }

        //        eStoneType next_type = eStoneType.White;
        //        if (type == eStoneType.White) next_type = eStoneType.Black;

        //        int val = MiniMax(pos, depth_ - 1, next_type, new_board, evalutions);

        //        if(val > best_value)
        //        {
        //            best_value = val;
        //            dicts.Add(val, new List<int>());
        //            dicts[val].Add(pos);
        //        }
        //        else if (val == best_value)
        //        {
        //            dicts[val].Add(pos);
        //        }
        //    }

        //    int n = dicts[best_value].Count;
        //    return dicts[best_value][Random.Range(0, n)];
        //}

        public override GameTree Play(GameTree tree)
        {
            return tree.GetEnableMoveNodes()[0];
        }

        public override string ToString()
        {
            return "MiniMax";
        }
    }
} // namespace Reversi