using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml;
using UnityEngine;

/// <summary>
/// モンテカルロ木探索AI
/// オセロ固有の知識がなくても強い
/// </summary>

namespace Reversi
{
    // 探索木のノード 盤面を拡張する
    public class Node
    {
        // 現在の盤面
        public GameTree GameTree { private set; get; }
        // 親ノード
        public Node Parent { private set; get; }
        // 子ノード
        public List<Node> Childs { private set; get; }
        // まだ訪れていない子ノード
        public Queue<Node> NotVisitChilds { private set; get; }
        public int NotVisitNum { private set; get; }
        public int ChildNum { private set; get; }

        // 勝利数
        public float WinNum { private set; get; }
        // 訪問数
        public int VisitNum { private set; get; }

        // UCTのパラメータ
        private readonly float c;

        // 未訪問のノード探索を遅延評価するのに使う
        private bool child_inited_ = false;

        public Node(GameTree tree, Node parent)
        {
            GameTree = tree;
            Parent = parent;
            Childs = new List<Node>();
            WinNum = 0;
            VisitNum = 0;
            c = Mathf.Sqrt(2);
            ChildNum = 0;
            NotVisitNum = tree.GetEnableMoveNodes().Count;
            child_inited_ = false;
        }

        // 未訪問の子ノード
        private void InitNotVisitChilds()
        {
            // 子ノードの順番をランダムに決める
            int n = NotVisitNum;
            List<int> order = new List<int>(n);
            for (int i = 0; i < n; ++i) order.Add(i);
            for (int i = 0; i < n; ++i)
            {
                int tail = n - i - 1;
                int j = Random.Range(0, tail + 1);
                int tmp = order[tail];
                order[tail] = order[j];
                order[j] = tmp;
            }
            // 順番通りにキューに入れる
            var trees = GameTree.GetEnableMoveNodes();
            NotVisitChilds = new Queue<Node>();
            //string s = "";
            for (int i = 0; i < n; ++i)
            {
                //s += order[i].ToString();
                NotVisitChilds.Enqueue(new Node(trees[order[i]], this));
            }
            //Debug.Log(s);
        }

        // 次に進む子ノードを選択する
        public Node SelectChildNode()
        {
            // float型だから誤差が出そう 同じ計算処理だから大丈夫か
            Dictionary<float, List<Node>> dict = new Dictionary<float, List<Node>>();

            float top_value = -1000f;
            foreach(var child in Childs)
            {
                // UCTに従う
                // w/n + c√(log(t)/n)
                float value = child.WinNum / child.VisitNum +
                    c * Mathf.Sqrt(Mathf.Log(VisitNum) / child.VisitNum);
                if(value > top_value)
                {
                    top_value = value;
                    dict.Add(value, new List<Node>());
                    dict[value].Add(child);
                }
                else if(value == top_value)
                {
                    dict[value].Add(child);
                }
            }

            int n = dict[top_value].Count;
            return dict[top_value][Random.Range(0, n)];
        }

        // まで訪問していないランダムの子ノードを返す
        public Node ExpandChildNode()
        {
            if (!child_inited_)
            {
                child_inited_ = true;
                InitNotVisitChilds();
            }
            --NotVisitNum;
            ++ChildNum;
            var ret = NotVisitChilds.Dequeue();
            Childs.Add(ret);
            return ret;
        }

        // 実際にシミュレーションする 勝ち 1 引き分け 0.5 負け 0
        public float SimulateRandomPlay(eStoneType player)
        {
            GameTree node = GameTree;
            int n = node.GetEnableMoveNodes().Count;
            while (n != 0)
            {
                node = new GameTree(node.GetEnableMoveNodes()[Random.Range(0, n)]);
                n = node.GetEnableMoveNodes().Count;
            }

            int ret = ReversiUtils.JudgeResult(node.Board);
            if (player == eStoneType.White) ret *= -1;
            return ret / 2f + 0.5f;
        }

        // 結果を逆伝播させる
        public void BackPropagate(float value)
        {
            var node = this;
            while(node != null)
            {
                node.WinNum += value;
                ++node.VisitNum;
                node = node.Parent;
            }
        }
    }

    public class MCTSPlayer : BasePlayer
    {
        [SerializeField]
        private int trial_num_ = 100;

        public override GameTree Play(GameTree tree)
        {
            // ノード生成
            Node root = new Node(tree, null);

            for(int i = 0; i < trial_num_; ++i)
            {
                var node = root;

                // 現在見てるノードの子ノードをすべて訪れてるなら，進めるところまで進む
                while(node.NotVisitNum == 0 && node.ChildNum != 0)
                {
                    node = node.SelectChildNode();
                }

                // まだ訪れていない子ノードがあるならそれに進む
                if(node.NotVisitNum != 0)
                {
                    node = node.ExpandChildNode();
                }

                // シミュレーションさせて逆伝播させる
                node.BackPropagate(node.SimulateRandomPlay(tree.StoneType));
            }

            // なるほどなぁ

            // 最も訪問数の多いノードへと進む AlphaGoがそう
            Dictionary<int, List<Node>> dict = new Dictionary<int, List<Node>>();
            int top_value = -1000;
            foreach(var child in root.Childs)
            {
                int value = child.VisitNum;
                if(value > top_value)
                {
                    top_value = value;
                    dict.Add(value, new List<Node>());
                }
                if(value == top_value)
                {
                    dict[value].Add(child);
                }
            }

            int n = dict[top_value].Count;
            var ret = dict[top_value][Random.Range(0, n)];
            //Debug.Log(ret.WinNum + " " + ret.VisitNum + " " + ret.WinNum / ret.VisitNum);
            return ret.GameTree;
        }

        public override string ToString()
        {
            return "MCTS";
        }
    }
} // namespace Reversi