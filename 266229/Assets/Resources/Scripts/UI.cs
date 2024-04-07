using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI : GameController
{
    private Canvas canvas;
    private Canvas pointableCanvas;
    private List<GameObject> inputPanelFrameObjectList = new List<GameObject>();
    private static List<GameObject> qContentObjectList = new List<GameObject>();
    private static List<GameObject> underbarObjectList = new List<GameObject>();
    private static List<GameObject> inputValueObjectList = new List<GameObject>();
    private static List<GameObject> underValueObjectList = new List<GameObject>();
    private GameObject _inputPanelFrameObject;
    private GameObject _qContentObject;
    private GameObject _underbarObject;
    private GameObject _inputValueObject;
    private GameObject addScoreObject;
    private GameObject passButtonObject;
    private GameObject passObject;
    private GameObject _rightTriangleObject;
    private GameObject _targetObject;
    private GameObject _underValueObject;
    private Image passButtonImage;
    private TMP_Text qLvText;
    private TMP_Text qNoText;
    private TMP_Text scoreText;
    private TMP_Text addScoreText;
    private TMP_Text passText;

    private float posX;
    private int _number;
    private int _count;
    private int _input;
    private int _limit;
    private char _inputValue;
    private char _underValue;
    private string _underTarget;

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
        // Debugger.Db(2);
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log($"U_inputValueObjectList.Count : {inputValueObjectList.Count}");  // debug
    }

    // Objects初期化：Object削除とListクリア
    private void _InitializeObjects(List<GameObject> objectList)
    {
        for (int i = 0; i <= objectList.Count - 1; i++)
        {
            Destroy(objectList[i]);
        }
        objectList.Clear();
    }

    // 入力パネルフレーム
    private void _InputPanelFrame()
    {
        // 初期化
        _InitializeObjects(inputPanelFrameObjectList);

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
        _InitializeObjects(qContentObjectList);
        _InitializeObjects(underbarObjectList);

        // 問題内容リスト、入力数字リスト作成
        _MakeQContentList();
        _MakeInputNumberList();

        // 表示
        posX = 280f - (float)GameController.qLv * 60f;
        _number = 0;
        _count = 0;
        for (int i = 0; i <= GameController.qLv; i++)
        {
            posX += 120f;
            if (_number == GameController.qContentList[i])
            {
                _count += 1;
            }
            else
            {
                _number = GameController.qContentList[i];
                _count = 1;
            }

            _qContentObject = Instantiate(Resources.Load("Prefabs/Text"), canvas.transform) as GameObject;
            _qContentObject.transform.position = new Vector3(posX, 540f, 0f);
            _qContentObject.GetComponent<TMP_Text>().text = _number.ToString();
            _qContentObject.GetComponent<TMP_Text>().fontSize = 120;
            qContentObjectList.Add(_qContentObject);

            _underbarObject = Instantiate(Resources.Load("Prefabs/Square"), canvas.transform) as GameObject;

            // 数字使用状況（アンダーバー）表示
            _input = GameController.inputNumberList.Count(x => x == _number);
            _limit = GameController.qContentList.Count(x => x == _number);
            if (_input <= _limit)
            {
                // 入力許容数以下
                _underbarObject.transform.position = new Vector3(posX, 480f, 0f);
                _underbarObject.GetComponent<RectTransform>().sizeDelta = new Vector2(90, 5);
                if (_count <= _input)
                {
                    _underbarObject.GetComponent<Image>().color = new Color(0f, 1f, 1f, 1f);
                }
                else
                {
                    _underbarObject.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1f);
                }
                underbarObjectList.Add(_underbarObject);
            }
            else if (_count == _limit)
            {
                // 入力許容数超過
                _underbarObject.transform.position = new Vector3(posX - (((float)_limit - 1f) * 60f), 480f, 0f);
                _underbarObject.GetComponent<RectTransform>().sizeDelta = new Vector2(_limit * 120 - 30, 5);
                _underbarObject.GetComponent<Image>().color = new Color(1f, 0.5f, 0f, 1f);
                underbarObjectList.Add(_underbarObject);
            }
        }
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
        GameController.qContentList.Sort();  // e.g. {2, 2, 2, 6, 6}
    }

    // 入力数字リスト作成
    private void _MakeInputNumberList()
    {
        // 初期化
        GameController.inputNumberList.Clear();

        // 入力式から数字を取り出す、_なら0
        for (int i = 0; i <= GameController.qLv; i++)
        {
            if (GameController.inputFormula.Substring(i * 2, 1) == "_")
            {
                GameController.inputNumberList.Add(0);
            }
            else
            {
                GameController.inputNumberList.Add(int.Parse(GameController.inputFormula.Substring(i * 2, 1)));
            }
        }

        // 昇順ソートは一旦しなくて良いだろう
        // GameController.inputNumberList.Sort();  // e.g. {0, 6, 6, 2, 0}
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

    // 演算子の表示文字化、演算子以外はそのまま
    private char _ValueChar(char _valueChar)
    {
        switch (_valueChar)
        {
            case '+':
                return '＋';
            case '-':
                return '－';
            case '*':
                return '×';
            case '/':
                return '÷';
            default:
                return _valueChar;
        }
    }

    // 入力数字、入力演算子
    private void _InputValue()
    {
        // 初期化
        _InitializeObjects(inputValueObjectList);

        // 表示
        posX = 415f - (float)GameController.qLv * 90f;
        // Debug.Log($"inputFormula: {GameController.inputFormula}");  // debug
        for (int i = 0; i <= GameController.qLv * 2; i++)
        {
            posX += 90f;
            _inputValue = GameController.inputFormula[i];
            if (_inputValue != '_')
            {
                _inputValueObject = Instantiate(Resources.Load("Prefabs/Text"), canvas.transform) as GameObject;
                _inputValueObject.transform.position = new Vector3(posX, 360f, 0f);
                _inputValue = _ValueChar(_inputValue);
                _inputValueObject.GetComponent<TMP_Text>().text = _inputValue.ToString();
                _inputValueObject.GetComponent<TMP_Text>().fontSize = 144;
                inputValueObjectList.Add(_inputValueObject);
            }
        }
        // Debug.Log($"A_inputValueObjectList.Count : {inputValueObjectList.Count}");  // debug
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

    // TODO: 途中演算結果、演算結果
    private void _IntermediateCalc()
    {

    }

    // 解答例
    private void _AnswerExample()
    {
        // 初期化
        _InitializeObjects(underValueObjectList);

        // 表示
        posX = 415f - (float)GameController.qLv * 90f;
        for (int i = 0; i <= GameController.qLv * 2; i++)  // イコール以降の計算結果は表示しないことにしている
        {
            _underValueObject = Instantiate(Resources.Load("Prefabs/Text"), canvas.transform) as GameObject;
            if (i <= GameController.qLv * 2 + 1)
            {
                posX += 90f;
                _underValue = GameController.equation[i];
                _underValue = _ValueChar(_underValue);
                _underValueObject.GetComponent<TMP_Text>().text = _underValue.ToString();
            }
            else  // 計算結果＝ターゲット内容、現実装ではこちらは処理されない
            {
                posX += 135f;
                _underTarget = GameController.equation.Substring(i);
                _underValueObject.GetComponent<TMP_Text>().text = _underTarget;
            }
            _underValueObject.transform.position = new Vector3(posX, 225f, 0f);
            _underValueObject.GetComponent<TMP_Text>().fontSize = 90;
            _underValueObject.GetComponent<TMP_Text>().color = new Color(1f, 1f, 0f, 1f);
            underValueObjectList.Add(_underValueObject);
        }
    }

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
        _IntermediateCalc();
        _InitializeObjects(underValueObjectList);
    }

    // 入力：PointerInput.csから毎f呼び出される
    public void Input()
    {
        _QContent();
        _InputValue();
        _IntermediateCalc();
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
        _AnswerExample();
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
