using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameController : MonoBehaviour
{
    private Calculator Calculator;
    private CenterMsg CenterMsg;
    private Fps Fps;
    private GameEnd GameEnd;
    private Mouse Mouse;
    private QuestionMaker QuestionMaker;
    private Timer Timer;
    private Title Title;
    private UI UI;

    private string gameMode;  // "TRIAL" or "PRACTICE"
    protected static int qLv;
    protected static int qNo;
    private int qNoInLv;
    private int pass;
    private int restPass;
    private int plusTime;
    protected static string equation;  // 想定解答 e.g. "2+3*5=17"
    protected static List<int> qContentList = new List<int>();  // e.g. {2, 2, 2, 6, 6}
    private string inputFormula;
    private float answerTime;
    private string result;
    protected static int score;
    protected static int addScore;

    void Awake()
    {
        Calculator = this.gameObject.AddComponent<Calculator>();
        CenterMsg = this.gameObject.AddComponent<CenterMsg>();
        Fps = this.gameObject.AddComponent<Fps>();
        GameEnd = this.gameObject.AddComponent<GameEnd>();
        Mouse = this.gameObject.AddComponent<Mouse>();
        QuestionMaker = this.gameObject.AddComponent<QuestionMaker>();
        Timer = this.gameObject.AddComponent<Timer>();
        Title = this.gameObject.AddComponent<Title>();
        UI = this.gameObject.AddComponent<UI>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // 本ゲームのメイン処理実行
        StartCoroutine(MainProcess());
    }

    // Update is called once per frame
    void Update()
    {

    }

    // 本ゲームのメイン処理
    private IEnumerator MainProcess()
    {
        // タイトル
        gameMode = Title.MainProcess();

        // 背景表示

        // BGM再生
        if (gameMode == "TRIAL")
        {

        }
        else
        {

        }

        // 変数初期化
        CenterMsg.Show(msg: "LOADING...");
        if (gameMode == "TRIAL")
        {
            qLv = 0;
            Timer.Set(60f);
        }
        else
        {
            Timer.Set(0f);
        }
        Timer.Display(true);
        qNo = 0;
        pass = 0;
        plusTime = 0;
        yield return new WaitForSeconds(2f);

        // 問題事前作成（必要？）

        // SEメモリロード

        // 3秒カウントダウン
        CenterMsg.Show(msg: "3");
        yield return new WaitForSeconds(1f);
        CenterMsg.Show(msg: "2");
        yield return new WaitForSeconds(1f);
        CenterMsg.Show(msg: "1");
        yield return new WaitForSeconds(1f);

        // スタート
        Debug.Log("START");
        CenterMsg.Show(msg: "START", showTime: 1f);
        while (true)
        {
            // Debug.Log("LOOP");
            // yield return null;

            // 問題レベル+1（TRIALなら）
            if (gameMode == "TRIAL")
            {
                qLv += 1;
                Debug.Log($"qLv: {qLv.ToString()}");
            }

            // レベルに応じた背景に更新

            // パス回数やレベル内の正解数を初期化
            restPass = 5;
            qNoInLv = 0;

            // 同一問題Lv.内
            while (true)
            {
                // Debug.Log("LoopInLv");
                // yield return null;

                // 問題番号+1（レベル内番号、QNo.）
                qNoInLv += 1;
                qNo += 1;

                // 入力値、途中計算値初期化
                result = "";
                inputFormula = "";

                // 問題更新（事前に作成した問題リストから取ってくる）
                equation = QuestionMaker.Make();  // e.g. "2+3*5=17"

                // 入力候補更新
                // inputCandidate = QuestionMaker.

                // 解答開始時点での残り/経過時刻保存
                answerTime = Timer.Get();

                // タイマーリスタート
                if (gameMode == "TRIAL")
                {
                    Timer.StartCount("DOWN");
                }
                else
                {
                    Timer.StartCount("UP");
                }

                // UI更新（問題更新）
                UI.QuestionUpdate();

                // 【解答入力待ち、正解なら続行】
                while (result == "")
                {
                    result = Mouse.MainProcess();
                    yield return null;
                }

                // タイマーストップ
                Timer.StopCount();

                // 結果処理　正解なら解答時間から時間追加、スコア計算、AC表示とエフェクト　パスなら残り回数減算、PASS表示、正解例表示（追加wait1s）
                if (result == "ACCEPTED")
                {
                    CenterMsg.Show(msg: "ACCEPTED", showTime: 1f, color: "green");

                    // 解答時間算出
                    answerTime -= Timer.Get();
                    if (gameMode == "PRACTICE")
                    {
                        answerTime *= -1;
                    }

                    // Lv.5のみ正解数が11以上になると残り時間が増えなくなる
                    if (qNoInLv + restPass <= 15)
                    {
                        Timer.Add((float)(qLv * 5));
                    }

                    // スコア加算
                    addScore = (int)(((float)qLv * 100f) * ((float)qLv * 10f + answerTime) / ((float)qLv * 10f + answerTime * 2f));
                    score += addScore;
                    UI.AddScore();
                }
                else if (result == "PASS")
                {
                    CenterMsg.Show(msg: "PASS", showTime: 2f, color: "yellow");

                    // パス回数カウント（TRIALモードなら残り回数-1）
                    if (gameMode == "TRIAL")
                    {
                        restPass -= 1;
                    }
                    pass += 1;

                    // パス時のUI表示
                    UI.Pass();
                }
                else if (result == "FINISH")
                {
                    break;
                }

                // レベル内で15問以上になってたら問題追加

                // 問題レベルアップ判定、アップならbreak
                if (gameMode == "TRIAL" && qNoInLv + restPass >= 15 && qLv <= 4)
                {
                    break;
                }
            }
            // 終了フラグが立ってたらbreak
            if (result == "FINISH")
            {
                CenterMsg.Show(msg: "FINISH", showTime: 3f, color: "red");
                yield return new WaitForSeconds(3f);
                break;
            }

            // 問題レベルアップ演出
            CenterMsg.Show(msg: "LEVEL UP", showTime: 1f, color: "pink");
        }

        // GameEnd処理
        GameEnd.MainProcess();

        yield break;
    }
}
