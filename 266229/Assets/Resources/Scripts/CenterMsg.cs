using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CenterMsg : MonoBehaviour
{
    private TMP_Text centerMsg;

    void Awake()
    {
        centerMsg = GameObject.Find("CenterMsg").GetComponent<TMP_Text>();
        // Debug.Log(centerMsg);
        // Debug.Log(centerMsg.text);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Show(string msg, float showTime = 0f, string color = "white", int size = 180, bool animation = true)
    {
        /*
        引数：表示時間（1s）、文字色、フォントサイズ（180？）、最大文字幅補正（？）
        不透明度50%
        */

        centerMsg.text = $"<color=\"{color}\"><size={size}>{msg}</size></color>";
        if (showTime > 0f)
        {
            StartCoroutine(ShowCoroutine(showTime, animation));
        }
    }

    private IEnumerator ShowCoroutine(float showTime, bool animation)
    {
        if (animation == true)
        {

            // TODO: Implement.
            yield return new WaitForSeconds(showTime);

        }
        else
        {

            yield return new WaitForSeconds(showTime);

        }

        centerMsg.text = "";
        yield break;
    }
}
