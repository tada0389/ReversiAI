using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 盤面情報からオブジェクトを配置するクラス
/// </summary>

namespace Reversi
{
    public class BoardRenderer : MonoBehaviour
    {
        // 石の色
        [SerializeField]
        private Color front_color_ = Color.black;
        [SerializeField]
        private Color back_color_ = Color.white;

        [SerializeField]
        private Color board_color_ = Color.green;

        // 次に石が置ける場所たち
        [SerializeField]
        private Color selected_color_ = Color.red;

        [SerializeField]
        private SpriteRenderer stone_prefab_;

        [SerializeField]
        private SpriteRenderer board_prefab_;

        [SerializeField]
        private float board_distance_ = 1f;

        private List<SpriteRenderer> boards_;
        private List<SpriteRenderer> stones_;

        [SerializeField]
        private Text black_text_;
        [SerializeField]
        private Text white_text_;

        private int black_cnt_;
        private int white_cnt_;

        [SerializeField]
        private Text black_player_text_;
        [SerializeField]
        private Text white_player_text_;

        [SerializeField]
        private Text black_win_text_;
        [SerializeField]
        private Text white_win_text_;

        private int black_win_num_;
        private int white_win_num_;

        private void Awake()
        {
            black_cnt_ = 0;
            white_cnt_ = 0;
            black_win_num_ = 0;
            white_win_num_ = 0;
            boards_ = new List<SpriteRenderer>(64);
            stones_ = new List<SpriteRenderer>(64);
            CreateBoard();
        }

        public void ShowPlayerNames(List<BasePlayer> players)
        {
            black_player_text_.text = players[0].ToString();
            white_player_text_.text = players[1].ToString();
        }

        public void ProcResult()
        {
            if(black_cnt_ < white_cnt_)
            {
                ++white_win_num_;
                white_win_text_.text = white_win_num_.ToString();
            }
            else if(black_cnt_ > white_cnt_)
            {
                ++black_win_num_;
                black_win_text_.text = black_win_num_.ToString();
            }
        }

        // 盤面を更新する
        public void ProcBoard(GameTree tree)
        {
            black_cnt_ = 0;
            white_cnt_ = 0;

            // 石の色を変える
            for (int y = 0; y < 8; ++y)
            {
                for(int x = 0; x < 8; ++x)
                {
                    int index = y * 8 + x;

                    switch (tree.Board[x, y])
                    {
                        case eStoneType.None:
                            stones_[index].gameObject.SetActive(false);
                            break;
                        case eStoneType.Black:
                            ++black_cnt_;
                            stones_[index].gameObject.SetActive(true);
                            stones_[index].color = front_color_;
                            break;
                        case eStoneType.White:
                            ++white_cnt_;
                            stones_[index].gameObject.SetActive(true);
                            stones_[index].color = back_color_;
                            break;
                        default:
                            break;
                    }
                }
            }

            // 次に選択できるマスを半透明で載せる
            var enable_pos = ReversiUtils.GetEnableHands(tree.Board, tree.StoneType);

            foreach(var pos in enable_pos)
            {
                stones_[pos].gameObject.SetActive(true);
                Color color = front_color_;
                color.a = 0.4f;
                if (tree.StoneType == eStoneType.White)
                {
                    color = back_color_;
                    color.a = 0.55f;
                }
                stones_[pos].color = color;
            }

            // 石のテキストを更新
            black_text_.text = black_cnt_.ToString();
            white_text_.text = white_cnt_.ToString();
        }

        //// 石を置く 置く座標と石のタイプを渡す ほんとはクラス作ってまとめて渡したかったけどいいや
        //public void SetStoneSprite(List<int> positions, List<eStoneType> types)
        //{
        //    UnityEngine.Assertions.Assert.IsTrue(positions.Count == types.Count, "送られてきたリストのサイズがおかしい");

        //    for(int i = 0, n = positions.Count; i < n; ++i)
        //    {
        //        var type = types[i];
        //        var pos = positions[i];

        //        var stone = stones_[pos];
        //        if (type == eStoneType.Black)
        //        {
        //            if (!stone.gameObject.activeSelf)
        //            {
        //                stone.gameObject.SetActive(true);
        //                ++white_cnt_;
        //            }
        //            stone.color = front_color_;
        //            --white_cnt_;
        //            ++black_cnt_;
        //        }
        //        else if (type == eStoneType.White)
        //        {
        //            if (!stone.gameObject.activeSelf)
        //            {
        //                stone.gameObject.SetActive(true);
        //                ++black_cnt_;
        //            }
        //            stone.color = back_color_;
        //            --black_cnt_;
        //            ++white_cnt_;
        //        }
        //        else if (stone.gameObject.activeSelf) stone.gameObject.SetActive(false);
        //    }

        //    black_text_.text = black_cnt_.ToString();
        //    white_text_.text = white_cnt_.ToString();
        //}

        //// 次に置ける盤面を表示する
        //public void SetSelectedColor(List<int> positions)
        //{
        //    foreach(var pos in positions)
        //    {
        //        boards_[pos].color = selected_color_;
        //    }
        //}

        //// 次に置くことのできる盤面をリセットする
        //public void DismissSelectedColor(List<int> positions)
        //{
        //    foreach (var pos in positions)
        //    {
        //        boards_[pos].color = board_color_;
        //    }
        //}        //// 石を置く 置く座標と石のタイプを渡す ほんとはクラス作ってまとめて渡したかったけどいいや
        //public void SetStoneSprite(List<int> positions, List<eStoneType> types)
        //{
        //    UnityEngine.Assertions.Assert.IsTrue(positions.Count == types.Count, "送られてきたリストのサイズがおかしい");

        //    for(int i = 0, n = positions.Count; i < n; ++i)
        //    {
        //        var type = types[i];
        //        var pos = positions[i];

        //        var stone = stones_[pos];
        //        if (type == eStoneType.Black)
        //        {
        //            if (!stone.gameObject.activeSelf)
        //            {
        //                stone.gameObject.SetActive(true);
        //                ++white_cnt_;
        //            }
        //            stone.color = front_color_;
        //            --white_cnt_;
        //            ++black_cnt_;
        //        }
        //        else if (type == eStoneType.White)
        //        {
        //            if (!stone.gameObject.activeSelf)
        //            {
        //                stone.gameObject.SetActive(true);
        //                ++black_cnt_;
        //            }
        //            stone.color = back_color_;
        //            --black_cnt_;
        //            ++white_cnt_;
        //        }
        //        else if (stone.gameObject.activeSelf) stone.gameObject.SetActive(false);
        //    }

        //    black_text_.text = black_cnt_.ToString();
        //    white_text_.text = white_cnt_.ToString();
        //}

        //// 次に置ける盤面を表示する
        //public void SetSelectedColor(List<int> positions)
        //{
        //    foreach(var pos in positions)
        //    {
        //        boards_[pos].color = selected_color_;
        //    }
        //}

        //// 次に置くことのできる盤面をリセットする
        //public void DismissSelectedColor(List<int> positions)
        //{
        //    foreach (var pos in positions)
        //    {
        //        boards_[pos].color = board_color_;
        //    }
        //}

        // 状態をリセットする
        public void Reset()
        {
            black_cnt_ = 0;
            white_cnt_ = 0;

            for(int i = 0; i < 64; ++i)
            {
                boards_[i].color = board_color_;
                stones_[i].gameObject.SetActive(false);
            }
        }

        private void CreateBoard()
        {
            for (int y = 0; y < 8; ++y)
            {
                for (int x = 0; x < 8; ++x)
                {
                    var board = Instantiate(board_prefab_);
                    var stone = Instantiate(stone_prefab_);

                    float tx = (x - 4) * board_distance_;
                    float ty = (y - 4) * board_distance_;
                    board.transform.position = new Vector3(tx, ty, 0f);
                    stone.transform.position = new Vector3(tx, ty, 0f);

                    board.color = board_color_;
                    stone.gameObject.SetActive(false);

                    boards_.Add(board);
                    stones_.Add(stone);
                }
            }
        }
    }
}