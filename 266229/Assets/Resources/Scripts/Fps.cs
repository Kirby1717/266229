using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Fps : GameController
{
    private TMP_Text fpsText;

    private int fps;
    private float processingTime = 0f;
    private float nextShow = 1f;

    void Awake()
    {
        fpsText = GameObject.Find("Fps").GetComponent<TMP_Text>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        fps += 1;
        processingTime += Time.deltaTime;
        if (processingTime >= nextShow)
        {
            fpsText.text = $"{fps.ToString()} fps";
            fps = 0;
            nextShow = processingTime + 1f;
        }
    }
}
