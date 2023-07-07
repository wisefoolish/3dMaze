using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class referenceSlide : MonoBehaviour
{
    private float preferredHeight;
    private float actualHeight;
    Text textComponent;
    // Start is called before the first frame update
    void Start()
    {
        RectTransform textArea = GameObject.FindWithTag("textArea").GetComponent<RectTransform>();
        textComponent = GetComponentInChildren<Text>();
        preferredHeight = textArea.rect.yMax - textArea.rect.yMin;
        actualHeight = textComponent.preferredHeight;
        Vector2 v = textComponent.rectTransform.localPosition;
        v.y = -(actualHeight - preferredHeight) / 2;
        textComponent.rectTransform.localPosition = v;
        Scrollbar scrollbar = transform.GetComponentInChildren<Scrollbar>();
        scrollbar.size = Math.Min(preferredHeight / actualHeight, 1);
        scrollbar.value = 0;
        scrollbar.onValueChanged.AddListener(OnScrollbarValueChanged);
    }

    private void OnScrollbarValueChanged(float value)
    {
        // 可以根据具体的需求执行其他操作
        Vector2 v = textComponent.rectTransform.localPosition;
        v.y = (actualHeight - preferredHeight) * (value - 0.5f);
        textComponent.rectTransform.localPosition = v;
    }

    public void Back()
    {
        SceneManager.LoadScene("InitialInterface");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
