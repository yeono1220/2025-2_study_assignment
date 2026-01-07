using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    Text MyText;

    Coroutine NowCoroutine;

    void Awake() {
        // MyText를 얻어오고, 내용을 지운다.dd
        // ---------- TODO ---------- 
        // Hierarchy 구조상 UIManager가 붙은 Canvas 하위에 "MyText"라는 이름의 오브젝트가 있어야 함
        Transform textObj = transform.Find("MyText");
        if (textObj != null)
        {
            MyText = textObj.GetComponent<Text>();
        }
        
        if (MyText != null)
        {
            MyText.text = "";
        }
        // -------------------- 
    }

    public void DisplayText(string text, float duration)
    {
        // NowCoroutine이 있다면 멈추고 새로운 DisplayTextCoroutine을 시작한다.
        // ---------- TODO ---------- 
        if (NowCoroutine != null)
        {
            StopCoroutine(NowCoroutine);
        }
        NowCoroutine = StartCoroutine(DisplayTextCoroutine(text, duration));
        // -------------------- 
    }

    IEnumerator DisplayTextCoroutine(string text, float duration)
    {
        // MyText에 text를 duration초 동안 띄운다.
        // ---------- TODO ---------- 
        if (MyText != null)
        {
            MyText.text = text;
            yield return new WaitForSeconds(duration);
            MyText.text = "";
        }
        else
        {
            yield return null;
        }
        // -------------------- 
    }
}