using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // ランダムなH値（0-1199）
        StartCoroutine(Loop());
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator Loop()
    {
        /*
        背景：不透明度25%、10F毎にアニメ、+0.1deg/1f、Hを0-1200で毎F増やしてRGBに換算
        前景：不透明度50%、5F毎にアニメ、+0.2deg/1f、色補正なし
        */
    }
}
