using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debugger : MonoBehaviour
{
    private static int da = 0;
    private static int db = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Da(int a)
    {
        da = a;
        Debug.Log($"D: {da}, {db}");
    }

    public void Db(int b)
    {
        db = b;
        Debug.Log($"D: {da}, {db}");
    }
}
