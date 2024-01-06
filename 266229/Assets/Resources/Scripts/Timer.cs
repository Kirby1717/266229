using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    private GameObject addTimeObject;
    private TMP_Text timer;
    private TMP_Text addTime;

    private bool display = false;
    private bool count = false;
    private string mode = "";  // "" -> "DOWN" or "UP"
    private float time;
    private int minute;
    private int second;
    private int csecond;
    private string show;
    private float wait;

    void Awake()
    {
        addTimeObject = GameObject.Find("AddTime");
        timer = GameObject.Find("Timer").GetComponent<TMP_Text>();
        addTime = GameObject.Find("AddTime").GetComponent<TMP_Text>();
        // Debug.Log(timer);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (count == true)
        {
            if (mode == "DOWN")
            {
                time -= Time.deltaTime;
                if (time <= 0f)
                {
                    StopCount();
                    time = 0f;
                }
            }
            else
            {
                time += Time.deltaTime;
            }
            if (display == true)
            {
                Show();
            }
        }
    }

    // 表示のON/OFFを切り替え
    public void Display(bool display)
    {
        this.display = display;
        if (display == true)
        {
            Show();
        }
        else
        {
            timer.text = "";
        }
    }

    // 時間のセット
    public void Set(float time)
    {
        this.time = time;
    }

    // 時間取得
    public float Get()
    {
        return time;
    }

    // 時間追加
    public void Add(float time)
    {
        this.time += time;
        StartCoroutine(ShowAdd(time));
    }

    // 表示文字列の作成、表示
    private void Show()
    {
        show = "";

        minute = (int)(time / 60f);
        second = (int)(time % 60f);
        csecond = (int)((time % 1f) / 0.01f);

        if (minute < 10)
        {
            show += "0";
        }
        show += $"{minute.ToString()}:";
        if (second < 10)
        {
            show += "0";
        }
        show += $"{second.ToString()}:";
        if (csecond < 10)
        {
            show += "0";
        }
        show += csecond.ToString();

        if (mode == "DOWN" && time <= 0f)
        {
            timer.text = $"<color=\"red\">{show}</color>";
        }
        else if (mode == "DOWN" && time < 10f)
        {
            timer.text = $"<color=\"orange\">{show}</color>";
        }
        else
        {
            timer.text = show;
        }
    }

    // 時間追加の表示・演出
    private IEnumerator ShowAdd(float time)
    {
        addTime.text = $"+{(int)time}sec.";
        for (int i = 1; i <= 5; i++)
        {
            this.gameObject.transform.position += new Vector3(80f, 0f, 0f);
            yield return new WaitForEndOfFrame();
            this.gameObject.transform.position += new Vector3(-80f, 0f, 0f);
            yield return new WaitForEndOfFrame();
            this.gameObject.transform.position += new Vector3(-80f, 0f, 0f);
            yield return new WaitForEndOfFrame();
            this.gameObject.transform.position += new Vector3(80f, 0f, 0f);
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(1f);
        addTime.text = "";
        yield break;
    }

    // カウントスタート
    public void StartCount(string mode)  // mode: "DOWN" or "UP"
    {
        this.mode = mode;
        count = true;
    }

    // カウントスタート
    public void StopCount()
    {
        count = false;
    }
}
