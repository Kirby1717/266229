using System.Collections;
using System.Collections.Generic;
using System.Linq;  // UniqueList作成用
using static System.Math;
using System.Text;  // StringBuilder用
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PointerInput : GameController
{
    private Calculator Calculator;
    private UI UI;
    private Canvas canvas;
    private List<GameObject> inputCandidateObjectList = new List<GameObject>();
    private GameObject _inputCandidateObject;

    private string _objectName;
    private int inputPanelNo;  // 左から 1, 2, ..., (GameController.qLv * 2 + 1)
    private List<int> qContentUniqueList = new List<int>();  // e.g. {2, 6}
    private float posX;
    private float posY;
    private Vector3 pointerPos;  // 原点は画面左下端
    private int index;
    private StringBuilder _inputFormula;

    void Awake()
    {
        Calculator = this.gameObject.AddComponent<Calculator>();
        UI = this.gameObject.AddComponent<UI>();
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // 入力待ち時、GameControllerから毎フレーム呼ばれる
    public void MainProcess()
    {
        // ポインタ座標取得
        pointerPos = Input.mousePosition;

        // 判定処理用の子プロセス
        _InputValue();
        _Reset();
        _Finish();
        _Pass();
    }

    // 入力（共通）
    private void _InputValue()
    {
        // 初期化：入力パネルフレームと入力候補を一旦全て消す
        for (int i = 1; i <= GameController.qLv * 2 + 1; i++)
        {
            _objectName = $"InputPanelFrame{i.ToString()}";
            if (i % 2 == 1)
            {
                GameObject.Find(_objectName).GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            }
            else
            {
                GameObject.Find(_objectName).GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1f);
            }
        }
        for (int i = 0; i <= inputCandidateObjectList.Count - 1; i++)
        {
            Destroy(inputCandidateObjectList[i]);
        }
        inputCandidateObjectList.Clear();

        // 実処理
        _Number();
        _Operator();
    }

    // 数字入力
    private void _Number()
    {
        if (GameController.pointerObjectName.Contains("InputPanelFrame") == true)
        {
            inputPanelNo = int.Parse(GameController.pointerObjectName.Substring(15));

            if (inputPanelNo % 2 == 1)
            {
                // 選択状態
                _objectName = GameController.pointerObjectName;  // e.g. "InputPanelFrame1"
                GameObject.Find(_objectName).GetComponent<Image>().color = new Color(1f, 1f, 0f, 1f);

                // 入力候補表示
                qContentUniqueList = GameController.qContentList.Distinct().ToList();
                for (int i = 1; i <= qContentUniqueList.Count; i++)
                {
                    switch ((qContentUniqueList.Count, i))
                    {
                        case (1, 1):
                            posX = 0f;
                            posY = 0f;
                            break;
                        case (2, 1):
                            posX = 0f;
                            posY = 33f;
                            break;
                        case (2, 2):
                            posX = 0f;
                            posY = -33f;
                            break;
                        case (3, 1):
                            posX = -23f;
                            posY = 23f;
                            break;
                        case (3, 2):
                            posX = 23f;
                            posY = 23f;
                            break;
                        case (3, 3):
                            posX = 0f;
                            posY = -23f;
                            break;
                        case (4, 1):
                            posX = -23f;
                            posY = 23f;
                            break;
                        case (4, 2):
                            posX = 23f;
                            posY = 23f;
                            break;
                        case (4, 3):
                            posX = -23f;
                            posY = -23f;
                            break;
                        case (4, 4):
                            posX = 23f;
                            posY = -23f;
                            break;
                        case (5, 1):
                            posX = -23f;
                            posY = 48f;
                            break;
                        case (5, 2):
                            posX = 23f;
                            posY = 48f;
                            break;
                        case (5, 3):
                            posX = -23f;
                            posY = 0f;
                            break;
                        case (5, 4):
                            posX = 23f;
                            posY = 0f;
                            break;
                        case (5, 5):
                            posX = 0f;
                            posY = -48f;
                            break;
                        case (6, 1):
                            posX = -23f;
                            posY = 48f;
                            break;
                        case (6, 2):
                            posX = 23f;
                            posY = 48f;
                            break;
                        case (6, 3):
                            posX = -23f;
                            posY = 0f;
                            break;
                        case (6, 4):
                            posX = 23f;
                            posY = 0f;
                            break;
                        case (6, 5):
                            posX = -23f;
                            posY = -48f;
                            break;
                        case (6, 6):
                            posX = 23f;
                            posY = -48f;
                            break;
                    }
                    _inputCandidateObject = Instantiate(Resources.Load("Prefabs/Text"), canvas.transform) as GameObject;
                    _inputCandidateObject.transform.position = GameObject.Find(_objectName).transform.position;
                    _inputCandidateObject.transform.position += new Vector3(posX, posY, 0f);
                    _inputCandidateObject.GetComponent<TMP_Text>().text = qContentUniqueList[i - 1].ToString();
                    _inputCandidateObject.GetComponent<TMP_Text>().color = new Color(1f, 1f, 0f, 1f);
                    _inputCandidateObject.GetComponent<TMP_Text>().fontSize = 54;
                    inputCandidateObjectList.Add(_inputCandidateObject);
                }

                // クリック時でなければここで処理終了
                if (GameController.pointerStatus != "DOWN")
                {
                    return;
                }

                // 対象入力パネルフレームの位置取得
                posX = GameObject.Find(_objectName).transform.position.x;
                posY = GameObject.Find(_objectName).transform.position.y;

                // クリック判定
                switch (qContentUniqueList.Count)
                {
                    case 1 or 2:
                        index = (int)(pointerPos.y / -360f) + 2;
                        break;
                    case 3 or 4:
                        index = ((int)(pointerPos.y / -360f) + 2) * 2;
                        if (pointerPos.x <= posX)
                        {
                            index -= 1;
                        }
                        break;
                    case 5 or 6:
                        if (pointerPos.y > 384f)
                        {
                            index = 2;
                        }
                        else if (pointerPos.y > 336f)
                        {
                            index = 4;
                        }
                        else
                        {
                            index = 6;
                        }
                        if (pointerPos.x <= posX)
                        {
                            index -= 1;
                        }
                        break;
                }
                index = Min(index, qContentUniqueList.Count);

                // 入力値を入力式にセット
                _inputFormula = new StringBuilder(GameController.inputFormula);
                _inputFormula[inputPanelNo - 1] = qContentUniqueList[index - 1].ToString()[0];
                GameController.inputFormula = _inputFormula.ToString();

                // UI更新
                UI.Input();

                // 入力式が完成していれば（空欄がなければ）演算実行、正解ならresultに"ACCEPTED"を返す
                if (GameController.inputFormula.Contains("_") == false)
                {
                    if (Calculator.Calculate(GameController.inputFormula) == float.Parse(GameController.equation.Substring(GameController.qLv * 2 + 2)))
                    {
                        GameController.result = "ACCEPTED";
                    }
                }

                // クリック状態を解除
                GameController.pointerStatus = "ENTER";
            }
        }
    }

    // 演算子入力
    private void _Operator()
    {

    }

    // リセット
    private void _Reset()
    {
        _objectName = "ResetButton";
        if (GameController.pointerObjectName == _objectName)
        {
            // 選択状態
            GameObject.Find(_objectName).GetComponent<Image>().color = new Color(1f, 0.5f, 0f, 1f);
            GameObject.Find(_objectName.Replace("Button", "")).GetComponent<TMP_Text>().color = new Color(1f, 0.5f, 0f, 1f);

            // クリック時
            if (GameController.pointerStatus == "DOWN")
            {
                GameController.inputFormula = "";
                for (int i = 1; i <= GameController.qLv * 2 + 1; i++)
                {
                    GameController.inputFormula += "_";
                }
                UI.Input();
                GameController.pointerStatus = "ENTER";
            }
        }
        else
        {
            // 選択状態解除
            GameObject.Find(_objectName).GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            GameObject.Find(_objectName.Replace("Button", "")).GetComponent<TMP_Text>().color = new Color(1f, 1f, 1f, 1f);
        }
    }

    // フィニッシュ
    private void _Finish()
    {
        if (GameController.pointerObjectName == "FinishButton")
        {

        }
        else
        {

        }
    }

    // パス
    private void _Pass()
    {
        _objectName = "PassButton";

        // TRIALモードならパスが残っている場合のみ反応
        if (GameController.gameMode != "TRIAL" || GameController.restPass >= 1)
        {
            if (GameController.pointerObjectName == _objectName)
            {
                // 選択状態
                GameObject.Find(_objectName).GetComponent<Image>().color = new Color(1f, 1f, 0f, 1f);
                GameObject.Find(_objectName.Replace("Button", "")).GetComponent<TMP_Text>().color = new Color(1f, 1f, 0f, 1f);

                // クリック時
                if (GameController.pointerStatus == "DOWN")
                {
                    GameController.result = "PASS";
                    GameController.pointerStatus = "ENTER";
                }
            }
            else
            {
                // 選択状態解除
                GameObject.Find(_objectName).GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                GameObject.Find(_objectName.Replace("Button", "")).GetComponent<TMP_Text>().color = new Color(1f, 1f, 1f, 1f);
            }
        }
        else
        {
            // パス残り0
            GameObject.Find(_objectName).GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.5f);
            GameObject.Find(_objectName.Replace("Button", "")).GetComponent<TMP_Text>().color = new Color(1f, 1f, 1f, 0.5f);
        }
    }

    // 選択状態全解除
    public void ExitAll()
    {

    }
}
