using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameController : MonoBehaviour
{

    // ボタンを押したときの関数。プレイヤーが答えを確定する。
    // フィールド変数としてpublicでふたつ宣言。
    public TMP_InputField input;
    public TMP_Text message;

    // 正解の数字をランダムで設定する。
    // フィールド変数としてint型のcorrectNumberを宣言。
    private int correctNumber = 0;

    // Start is called before the first frame update
    void Start()
    {

        // Input FieldとTextへの参照を取得
        input = GameObject.Find("InputField").GetComponent<TMP_InputField>();
        message = GameObject.Find("messageText").GetComponent<TMP_Text>();

        // 参照が正しく取得できたか確認
        if (input != null && message != null)
        {
            // ここにスクリプトのロジックを記述します
        }
        else
        {
            Debug.LogError("Input FieldまたはTextが見つかりません。GameObjectの名前を確認してください。");
        }

        int r = Random.Range(0, 100);
        correctNumber = r;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PressButton()
    {
        // InputFieldに入力された文字列（数字だけど）を、int型に変換して、
        // int型のplayerAnswerに割り当てる。
        int playerAnswer = int.Parse(input.text);

        // 正解とプレイヤーの答えを比べて、messageにメッセージを表示。

        if (playerAnswer == correctNumber)
        {
            message.text = "Accepted.";
        }
        else if (playerAnswer > correctNumber)
        {
            message.text = "The answer is less.";
        }
        else if (playerAnswer < correctNumber)
        {
            message.text = "The answer is greater.";
        }
    }

    // ニューゲームボタンを押したときの処理。
    public void PressNewGameButton()
    {
        SceneManager.LoadScene(0);
    }

}
