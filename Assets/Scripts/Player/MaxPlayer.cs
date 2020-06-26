using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

/// <summary>
/// 現在の盤面で最も石をとれる場所に置く
/// </summary>

namespace Reversi
{
    public class MaxPlayer : BasePlayer
    {
        public override GameTree Play(GameTree tree)
        {
            // 最大の取得数の中からランダムにする
            Dictionary<int, List<GameTree>> dict = new Dictionary<int, List<GameTree>>();
            int max_value = -1;

            foreach(var node in tree.GetEnableMoveNodes())
            {
                int value = ReversiUtils.GetObtainStones(tree.Board, node.PrevPos, tree.StoneType).Count;

                if (value > max_value)
                {
                    dict.Add(value, new List<GameTree>());
                    max_value = value;
                }

                if (value == max_value)
                {
                    dict[value].Add(node);
                }
            }

            int n = dict[max_value].Count;
            return dict[max_value][Random.Range(0, n)];
            
        }

        public override string ToString()
        {
            return "MaxGet";
        }
    }
} // namespace Reversi
