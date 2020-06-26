using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

namespace Reversi
{
    public class HumanPlayer : BasePlayer
    {
        public override GameTree Play(GameTree tree)
        {
            // クリックされて有効な手ならそれを返す
            if (Input.GetMouseButtonDown(0))
            {
                // くそ実装
                GameTree ret = CalcNextNode(tree);

                return ret;
            }
            else return null;
        }

        private GameTree CalcNextNode(GameTree tree)
        {
            // パスしかない場合
            if (tree.GetEnableMoveNodes().Count == 1)
            {
                GameTree head_tree = tree.GetEnableMoveNodes()[0];
                if (head_tree.PrevPos == -1)
                {
                    return head_tree;
                }
            }

            // マウスの座標はどこか
            Vector3 pos = Input.mousePosition;
            pos.z = 10f;
            pos = Camera.main.ScreenToWorldPoint(pos);

            // 1.05fずつ
            pos.x += 1.05f * 4.5f;
            pos.y += 1.05f * 4.5f;

            int x = (int)(pos.x / 1.05f + 0.01f);
            int y = (int)(pos.y / 1.05f + 0.01f);
            int p = y * 8 + x;

            foreach(var node in tree.GetEnableMoveNodes())
            {
                if(p == node.PrevPos)
                {
                    // 置ける
                    return node;
                }
            }

            return null;
        }

        public override string ToString()
        {
            return "Human";
        }
    }
} // namespace Reversi
