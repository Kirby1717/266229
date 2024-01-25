using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstLogIn : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public FirstLogIn()
    {
        StartCoroutine(FirstLogInCoroutine());
    }

    private IEnumerator FirstLogInCoroutine()
    {
        /*
        タイトル、サブタイトル、バージョン表示、ウェルカムメッセージ
        */

        yield break;

    }
}
