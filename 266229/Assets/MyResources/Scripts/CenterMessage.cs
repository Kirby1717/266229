using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterMessage : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Show()
    {
        StartCoroutine(ShowCoroutine());
    }

    private IEnumerator ShowCoroutine()
    {
        /*
        引数：表示時間（1s）、文字色、フォントサイズ（180？）、最大文字幅補正（？）
        不透明度50%
        */
    }
}
