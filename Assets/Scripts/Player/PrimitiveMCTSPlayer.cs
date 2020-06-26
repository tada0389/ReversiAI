﻿using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml;
using UnityEngine;

/// <summary>
/// 原始モンテカルロ木探索AI
/// オセロ固有の知識がなくても強い
/// </summary>

namespace Reversi
{
    public class PrimitiveMCTSPlayer : BasePlayer
    {
        [SerializeField]
        private int trial_num_ = 100;

        private int SimulateRandomPlay(GameTree tree, eStoneType player)
        {
            return 0;
        }

        public override GameTree Play(GameTree tree)
        {
            int top_value = -1;

            Dictionary<int, List<GameTree>> dict = new Dictionary<int, List<GameTree>>();

            // 勝った数で比較
            foreach(var node in tree.GetEnableMoveNodes())
            {
                int value = 0;
                for (int i = 0; i < trial_num_; ++i) 
                    value += SimulateRandomPlay(node, tree.StoneType);

                if(value > top_value)
                {
                    top_value = value;
                    dict.Add(value, new List<GameTree>());
                }
                if(value == top_value)
                {
                    dict[value].Add(node);
                }
            }

            int n = dict[top_value].Count;
            return dict[top_value][Random.Range(0, n)];
        }

        public override string ToString()
        {
            return "PrimitiveMCTS";
        }
    }
} // namespace Reversi