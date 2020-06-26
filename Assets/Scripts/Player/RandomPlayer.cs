using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

namespace Reversi
{
    public class RandomPlayer : BasePlayer
    {
        public override GameTree Play(GameTree tree)
        {
            int cnt = tree.GetEnableMoveNodes().Count;
            return tree.GetEnableMoveNodes()[Random.Range(0, cnt)];
        }

        public override string ToString()
        {
            return "Random";
        }
    }
} // namespace Reversi
