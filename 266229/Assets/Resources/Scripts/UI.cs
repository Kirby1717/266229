using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI : GameController
{
    private Canvas canvas;
    private Canvas pointableCanvas;
    private List<GameObject> inputPanelFrameObjectList = new List<GameObject>();
    private List<GameObject> qContentObjectList = new List<GameObject>();
    private List<GameObject> inputValueObjectList = new List<GameObject>();
    private GameObject _inputPanelFrameObject;
    private GameObject _qContentObject;
    private GameObject _inputValueObject;
    private GameObject addScoreObject;
    private GameObject passButtonObject;
    private GameObject passObject;
    private GameObject _rightTriangleObject;
    private GameObject _targetObject;
    private Image passButtonImage;
    private TMP_Text qLvText;
    private TMP_Text qNoText;
    private TMP_Text scoreText;
    private TMP_Text addScoreText;
    private TMP_Text passText;

    private float posX;
    private char _inputValue;

    void Awake()
    {
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        pointableCanvas = GameObject.Find("PointableCanvas").GetComponent<Canvas>();
        addScoreObject = GameObject.Find("AddScore");
        passButtonObject = GameObject.Find("PassButton");
        passObject = GameObject.Find("Pass");
        passButtonImage = GameObject.Find("PassButton").GetComponent<Image>();
        qLvText = GameObject.Find("QLv").GetComponent<TMP_Text>();
        qNoText = GameObject.Find("QNo").GetComponent<TMP_Text>();
        scoreText = GameObject.Find("Score").GetComponent<TMP_Text>();
        addScoreText = GameObject.Find("AddScore").GetComponent<TMP_Text>();
        passText = GameObject.Find("Pass").GetComponent<TMP_Text>();
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
        for (int i = 0; i <= inputPanelFrameObjectList.Count - 1; i++)
        {
            // Debug.Log($"Destroy:{i}");
            Destroy(inputPanelFrameObjectList[i]);
        }
        inputPanelFrameObjectList.Clear();

        // 生成：数字用を(Lv.+1)個、演算子用をLv.個（演算子用は薄い色で）
        posX = 415f - (float)GameController.qLv * 90f;
        for (int i = 1; i <= GameController.qLv * 2 + 1; i++)
        {
            posX += 90f;
            _inputPanelFrameObject = Instantiate(Resources.Load("Prefabs/InputPanelFrame"), pointableCanvas.transform) as GameObject;
            _inputPanelFrameObject.name = $"InputPanelFrame{i}";
            _inputPanelFrameObject.transform.position = new Vector3(posX, 360f, 0f);
            if (i % 2 == 0)
            {
                _inputPanelFrameObject.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1f);
            }
            _inputPanelFrameObject.AddComponent<Pointer>();
            inputPanelFrameObjectList.Add(_inputPanelFrameObject);
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
            // Debug.Log("DestroyQ: " + i.ToString());
            Destroy(qContentObjectList[i]);
        }
        qContentObjectList.Clear();

        // 内容リスト生成
        _MakeQContentList();

        // 表示
        posX = 280f - (float)GameController.qLv * 60f;
        for (int i = 0; i <= GameController.qLv; i++)
        {
            posX += 120f;
            _qContentObject = Instantiate(Resources.Load("Prefabs/Text"), canvas.transform) as GameObject;
            _qContentObject.transform.position = new Vector3(posX, 540f, 0f);
            _qContentObject.GetComponent<TMP_Text>().text = GameController.qContentList[i].ToString();
            _qContentObject.GetComponent<TMP_Text>().fontSize = 120;
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
    private void _Score()
    {
        scoreText.text = $"SCORE: {GameController.score.ToString()}";
    }

    // リセットボタン
    private void _ResetButton()
    {

    }

    // フィニッシュボタン
    private void _FinishButton()
    {

    }

    // パスボタン
    private void _PassButton()
    {
        if (GameController.gameMode == "TRIAL")
        {
            if (GameController.restPass >= 1)
            {
                passButtonImage.color = new Color(1f, 1f, 1f, 1f);
                passText.color = new Color(1f, 1f, 1f, 1f);
            }
            else
            {
                passButtonImage.color = new Color(1f, 1f, 1f, 0.5f);
                passText.color = new Color(1f, 1f, 1f, 0.5f);
            }
            passText.text = $"PASS <size=80>x{GameController.restPass.ToString()}</size>";
        }
        else
        {
            passText.text = "PASS";
        }
    }

    // 入力数字、入力演算子
    private void _InputValue()
    {
        // 初期化
        for (int i = 0; i <= inputValueObjectList.Count - 1; i++)
        {
            Destroy(inputValueObjectList[i]);
        }
        inputValueObjectList.Clear();

        // 表示
        posX = 415f - (float)GameController.qLv * 90f;
        for (int i = 0; i <= GameController.qLv * 2; i++)
        {
            posX += 90f;
            _inputValue = GameController.inputFormula[i];
            if (_inputValue != '_')
            {
                _inputValueObject = Instantiate(Resources.Load("Prefabs/Text"), canvas.transform) as GameObject;
                _inputValueObject.transform.position = new Vector3(posX, 360f, 0f);
                switch (_inputValue)
                {
                    case '+':
                        _inputValue = '＋';
                        break;
                    case '-':
                        _inputValue = '－';
                        break;
                    case '*':
                        _inputValue = '×';
                        break;
                    case '/':
                        _inputValue = '÷';
                        break;
                }
                _inputValueObject.GetComponent<TMP_Text>().text = _inputValue.ToString();
                _inputValueObject.GetComponent<TMP_Text>().fontSize = 144;
                inputValueObjectList.Add(_inputValueObject);
            }
        }
    }

    // 右三角形
    private void _RightTriangle()
    {
        // 存在していなければ生成
        if (!_rightTriangleObject)
        {
            _rightTriangleObject = Instantiate(Resources.Load("Prefabs/RightTriangle"), canvas.transform) as GameObject;
        }

        // 位置調整
        posX = 595f + (float)GameController.qLv * 90f;
        _rightTriangleObject.transform.position = new Vector3(posX, 360f, 0f);
    }

    // ターゲット内容
    private void _Target()
    {
        // 存在していなければ生成
        if (!_targetObject)
        {
            _targetObject = Instantiate(Resources.Load("Prefabs/Text"), canvas.transform) as GameObject;
        }

        // 位置調整
        posX = 730f + (float)GameController.qLv * 90f;
        _targetObject.transform.position = new Vector3(posX, 360f, 0f);

        // サイズ調整
        _targetObject.GetComponent<RectTransform>().sizeDelta = new Vector2(125, 50);

        // テキスト設定
        _targetObject.GetComponent<TMP_Text>().text = GameController.equation.Substring(GameController.qLv * 2 + 2);
        _targetObject.GetComponent<TMP_Text>().fontSize = 144;
    }

    // 途中演算結果、演算結果、入力例
    // TODO: Implement.

    // スタンバイ
    public void Standby()
    {
        _QLv();
        _QNo();
        _Score();
        _ResetButton();
        _FinishButton();
        _PassButton();
    }

    // 問題更新
    public void QuestionUpdate()
    {
        _InputPanelFrame();
        _QLv();
        _QNo();
        _QContent();
        _PassButton();
        _InputValue();
        _RightTriangle();
        _Target();
    }

    // 入力
    public void Input()
    {
        _QContent();
        _InputValue();
    }

    // スコア加算
    public void AddScore()
    {
        _Score();
        StartCoroutine(_AddScore());
    }

    // パス情報更新・入力例表示
    public void Pass()
    {
        _PassButton();
    }

    // スコア加算演出
    private IEnumerator _AddScore()
    {
        addScoreText.text = $"+{GameController.addScore.ToString()}";
        for (int i = 1; i <= 50; i++)
        {
            addScoreObject.transform.position += new Vector3(0f, 1f, 0f);
            yield return new WaitForSeconds(0.02f);
        }
        addScoreText.text = "";
        addScoreObject.transform.position += new Vector3(0f, -50f, 0f);
        // TODO: 円描画
        yield break;
    }
}
