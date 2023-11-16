using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(MainProcess());
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator MainProcess()
    {
        /*
        背景表示
        BGM再生
        変数初期化
        問題作成
        SEメモリロード
        3秒カウントダウン
        スタート
        ==ループ==
            問題レベル+1（TRIALなら）
            レベルに応じた背景に更新
            パス回数とかレベル内の正解数とか初期化
            ==ループ==
                問題番号+1（レベル内番号、QNo.）
                入力値、途中計算値初期化
                問題更新（事前に作成した問題リストから取ってくる）
                入力候補更新
                残り/経過時刻保存
                タイマーリスタート
                UI更新（問題更新）
                【解答入力待ち、正解なら続行】
                タイマーストップ
                結果処理　正解なら解答時間から時間追加、スコア計算、AC表示とエフェクト　パスなら残り回数減算、PASS表示、正解例表示（追加wait1s）
                レベル内で15問以上になってたら問題追加
                問題レベルアップ判定、アップならbreak
            =========
            問題レベルアップ演出
        =========
        */

        yield return new WaitForSeconds(1f);

        yield break;

    }

}
