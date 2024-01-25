using System;  // Convertを使用するため
using System.Collections;
using System.Collections.Generic;
using System.Data;  // DataTableを使用するため
using UnityEngine;

public class Calculator : MonoBehaviour
{
    DataTable dataTable = new DataTable();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public float Calculate(string formula)  // e.g. "2+6+6/2-2" -> 9f
    {
        return Convert.ToSingle(dataTable.Compute(formula, ""));
    }
}
