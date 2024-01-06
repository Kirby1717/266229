using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionMaker : GameController
{
    private Calculator Calculator;

    private string formula;
    private string equation;
    private List<string> operators = new List<string>() { "+", "-", "*", "/" };
    private float result;

    void Awake()
    {
        Calculator = this.gameObject.GetComponent<Calculator>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // 問題作成
    public string Make()
    {
        for (int i = 1; i <= 999999; i++)
        {
            // TODO: 試行が100回を超えた場合の措置を講じる
            formula = "";

            // 式をランダム生成
            for (int j = 1; j <= GameController.qLv; j++)
            {
                formula += Random.Range(2, 10).ToString();  // 数字
                formula += operators[Random.Range(0, 4)];  // 演算子
            }
            formula += Random.Range(2, 10).ToString();  // 数字

            // 生成された式を計算
            result = Calculator.Calculate(formula);

            // 結果が割り切れていて、適正な範囲内なら採用
            if (result % 1f == 0f && result > -10 && result < 100)
            {
                equation = formula + "=" + ((int)result).ToString();
                Debug.Log($"Equation ({equation}) is made after {i} trail(s).");
                break;
            }
        }
        return equation;
    }
}