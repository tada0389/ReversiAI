using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml;
using UnityEngine;

/// <summary>
/// いまはMiniMaxだけど，AlphaBetaとの差は計算時間だけなので時間あるときやる
/// (ゲームAIの性能をまず見たいので)
/// </summary>

namespace Reversi
{
    public class AlphaBetaPlayer : BasePlayer
    {
        // 何手先まで読むか
        [SerializeField]
        private int depth_ = 3;

        public GameTree MiniMax(GameTree tree, eStoneType player, int depth)
        {
            if (depth == 0)
            {
                // 自分自身を返す
                return tree;
            }
            else if (tree.GetEnableMoveNodes().Count == 0) // ゲーム終了
            {
                return tree;
            }


            // 本来置きたい手を置く場合は最も評価値の高い手を
            // 相手の手を置く場合は最も低い手を選ぶ

            bool to_max = (player == tree.StoneType);

            int top_value = 0;
            const int inf = (int)(1e9 + 7);
            if (to_max) top_value = -inf;
            else top_value = inf;

            Dictionary<int, List<GameTree>> dict = new Dictionary<int, List<GameTree>>();

            foreach (var node in tree.GetEnableMoveNodes())
            {
                int value = MiniMax(node, player, depth - 1).GetScoreDiff();
                if (player == eStoneType.White) value *= -1;

                if (to_max)
                {
                    if (value > top_value)
                    {
                        top_value = value;
                        dict.Add(value, new List<GameTree>());
                    }
                    if (value == top_value)
                    {
                        dict[value].Add(node);
                    }
                }
                else
                {
                    if (value < top_value)
                    {
                        top_value = value;
                        dict.Add(value, new List<GameTree>());
                    }
                    if (value == top_value)
                    {
                        dict[value].Add(node);
                    }
                }
            }

            // 最も良いやつからランダム
            int n = dict[top_value].Count;
            return dict[top_value][Random.Range(0, n)];
        }

        public override GameTree Play(GameTree tree)
        {
            return MiniMax(tree, tree.StoneType, depth_);
        }

        public override string ToString()
        {
            return "MiniMax";
        }
    }
} // namespace Reversi