using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Pointer : GameController, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // ポインタがUIオブジェクトに入った時
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Debug.Log("Enter: " + this.gameObject.name);
        // this.gameObject.GetComponent<Image>().color = new Color(1f, 1f, 0f, 1f);
        GameController.pointerObjectName = this.gameObject.name;
        GameController.pointerStatus = "ENTER";
    }

    // ポインタがUIオブジェクトから出た時
    public void OnPointerExit(PointerEventData eventData)
    {
        // Debug.Log("Exit: " + this.gameObject.name);
        // this.gameObject.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
        GameController.pointerObjectName = "";
        GameController.pointerStatus = "";
    }

    // ポインタがUIオブジェクトを押した時
    public void OnPointerDown(PointerEventData eventData)
    {
        // Debug.Log("Down: " + this.gameObject.name);
        GameController.pointerObjectName = this.gameObject.name;
        GameController.pointerStatus = "DOWN";
    }

    // ポインタがUIオブジェクトを離した時
    public void OnPointerUp(PointerEventData eventData)
    {
        // Debug.Log("Up: " + this.gameObject.name);
        GameController.pointerObjectName = this.gameObject.name;
        GameController.pointerStatus = "UP";
    }

}
