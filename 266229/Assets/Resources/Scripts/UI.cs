using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI : GameController
{
    private Canvas canvas;
    private List<GameObject> inputPanelFrameList = new List<GameObject>();
    private List<GameObject> qContentObjectList = new List<GameObject>();
    private GameObject _inputPanelFrame;
    private GameObject _qContentObject;
    private GameObject addScoreObject;
    private TMP_Text qLvText;
    private TMP_Text qNoText;
    private TMP_Text scoreText;
    private TMP_Text addScoreText;

    private float posX;

    void Awake()
    {
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        addScoreObject = GameObject.Find("AddScore");
        qLvText = GameObject.Find("QLv").GetComponent<TMP_Text>();
        qNoText = GameObject.Find("QNo").GetComponent<TMP_Text>();
        scoreText = GameObject.Find("Score").GetComponent<TMP_Text>();
        addScoreText = GameObject.Find("AddScore").GetComponent<TMP_Text>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // 入力パネルフレーム
    private void _InputPanelFrame()
    {
        // 初期化
        for (int i = 0; i <= inputPanelFrameList.Count - 1; i++)
        {
            Debug.Log($"Destroy:{i}");
            Destroy(inputPanelFrameList[i]);
            inputPanelFrameList.Clear();
        }

        // 生成：数字用を(Lv.+1)個、演算子用をLv.個（演算子用は薄い色で）
        posX = 415f - (float)GameController.qLv * 90f;
        for (int i = 1; i <= GameController.qLv * 2 + 1; i++)
        {
            posX += 90f;
            _inputPanelFrame = Instantiate(Resources.Load("Prefabs/InputPanelFrame"), canvas.transform) as GameObject;
            _inputPanelFrame.transform.position = new Vector3(posX, 360f, 0f);
            if (i % 2 == 0)
            {
                _inputPanelFrame.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1.0f);
            }
            inputPanelFrameList.Add(_inputPanelFrame);
        }
    }

    // 問題Lv.
    private void _QLv()
    {
        qLvText.text = $"Lv. {GameController.qLv}";
    }

    // 問題番号
    private void _QNo()
    {
        qNoText.text = $"Q. {GameController.qNo}";
    }

    // 問題内容
    private void _QContent()
    {
        // 初期化
        for (int i = 0; i <= qContentObjectList.Count - 1; i++)
        {
            Destroy(qContentObjectList[i]);
            qContentObjectList.Clear();
        }

        // 内容リスト生成
        _MakeQContentList();

        // 表示
        posX = 280f - (float)GameController.qLv * 60f;
        for (int i = 0; i <= GameController.qLv; i++)
        {
            posX += 120f;
            _qContentObject = Instantiate(Resources.Load("Prefabs/QContent"), canvas.transform) as GameObject;
            _qContentObject.transform.position = new Vector3(posX, 540f, 0f);
            _qContentObject.GetComponent<TMP_Text>().text = GameController.qContentList[i].ToString();
            qContentObjectList.Add(_qContentObject);
        }

        // TODO: 数字使用状況（アンダーバー）表示
    }

    // 問題内容リスト作成
    private void _MakeQContentList()
    {
        // 初期化
        GameController.qContentList.Clear();

        // 等式から数字を取り出す
        for (int i = 0; i <= GameController.qLv; i++)
        {
            GameController.qContentList.Add(int.Parse(GameController.equation.Substring(i * 2, 1)));
        }

        // 昇順ソートして完成
        GameController.qContentList.Sort();
    }

    // スコア

    // リセットボタン

    // フィニッシュボタン

    // パスボタン

    // 入力数字、入力演算子、右三角形、ターゲット内容

    // 途中演算結果、演算結果、入力例

    // スタンバイ
    public void Standby()
    {

    }

    // 問題更新
    public void QuestionUpdate()
    {
        _InputPanelFrame();
        _QLv();
        _QNo();
        _QContent();
    }

    // 入力
    public void Input()
    {
        _QContent();
    }

    // スコア加算
    public void AddScore()
    {
        scoreText.text = $"SCORE: {GameController.score.ToString()}";
        StartCoroutine(_AddScore());
    }

    // パス情報更新・入力例表示
    public void Pass()
    {

    }

    // スコア加算演出
    private IEnumerator _AddScore()
    {
        addScoreText.text = $"+{GameController.addScore.ToString()}";
        for (int i = 1; i <= 60; i++)
        {
            addScoreObject.transform.position += new Vector3(0f, 60f, 0f);
            yield return new WaitForEndOfFrame();
        }
        addScoreText.text = "";
        addScoreObject.transform.position += new Vector3(0f, -60f, 0f);
        // TODO: 円描画
        yield break;
    }
}
