using System;  // Convertを使用するため
using System.Collections;
using System.Collections.Generic;
using System.Data;  // DataTableを使用するため
using UnityEngine;

public class Calculator : GameController
{
    private DataTable dataTable = new DataTable();
    private string _term;
    private int _numerator;
    private int _denominator;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // 最大公約数を求める
    private int _Gcd(int a, int b)
    {
        if (a < b)
        {
            int tmp = a;
            a = b;
            b = tmp;
        }
        while (b != 0)
        {
            int remainder = a % b;
            a = b;
            b = remainder;
        }
        return a;
    }

    // 除算を行う、割り切れなければ最も簡単な分数にする
    private string _ComputeDivision(string formula)  // e.g. "6/4" -> "3/2"
    {
        string[] fraction = formula.Split('/');
        int numerator = int.Parse(fraction[0]);
        int denominator = int.Parse(fraction[1]);
        int gcd = _Gcd(numerator, denominator);
        numerator /= gcd;
        denominator /= gcd;
        if (denominator == 1)
        {
            return numerator.ToString();
        }
        else
        {
            return $"{numerator}/{denominator}";
        }
    }

    // 途中計算実行
    public void IntermediateCalc(string formula)  // e.g. "_+6_6/2__" -> {"_", "6", "?", "3", "_"}
    {
        // 途中計算初期化
        GameController.intermediateList.Clear();
        _numerator = 1;
        _denominator = 1;

        // 演算子を基準に式を分割  e.g. "_+6_6/2__" -> {"_", "6/1", "?", "6/2", "_"}
        for (int i = 1; i <= formula.Length - 2; i += 2)
        {
            if (i == 1)
            {
                if (formula[0] == '_')
                {
                    GameController.intermediateList.Add("_");
                }
                else
                {
                    _numerator *= int.Parse(formula[0].ToString());
                }
            }
            switch (formula[i])
            {
                case '+' or '-' or '_':
                    GameController.intermediateList.Add($"{_numerator}/{_denominator}");
                    _numerator = 1;
                    _denominator = 1;
                    break;
                case '*':
                    GameController.intermediateList.Add("?");
                    _numerator *= int.Parse(formula[i + 1].ToString());
                    break;
                case '/':
                    GameController.intermediateList.Add("?");
                    _denominator *= int.Parse(formula[i + 1].ToString());
                    break;
            }
            if (i == formula.Length - 2)
            {
                GameController.intermediateList.Add($"{_numerator}/{_denominator}");
            }
        }

        // 除算実行し途中計算を完成  e.g. {"_", "6/1", "?", "6/2", "_"} -> {"_", "6", "?", "3", "_"}
        for (int i = 0; i <= GameController.intermediateList.Count - 1; i++)
        {
            if (GameController.intermediateList[i].Contains("/"))
            {
                // 割り切れなければ最も簡単な分数にする
                GameController.intermediateList[i] = _ComputeDivision(GameController.intermediateList[i]);
            }
        }
    }

    // 演算実行
    public float Compute(string formula)  // e.g. "2+6+6/2-2" -> 9f
    {
        return Convert.ToSingle(dataTable.Compute(formula, ""));
    }
}
