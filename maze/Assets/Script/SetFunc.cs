using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SetFunc : MonoBehaviour
{
    private InputField Long;
    private InputField Width;
    private InputField Height;
    private GenerateMaze maze;
    // Start is called before the first frame update
    void Start()
    {
        maze = FindObjectOfType<GenerateMaze>();
        Long = GameObject.FindWithTag("Long").GetComponent<InputField>();
        Width = GameObject.FindWithTag("Width").GetComponent<InputField>();
        Height = GameObject.FindWithTag("Height").GetComponent<InputField>();
        Long.text = maze.GetTag("Long").ToString();
        Width.text = maze.GetTag("Width").ToString();
        Height.text = maze.GetTag("Height").ToString();
        Long.onValidateInput += ValidateInput;
        Width.onValidateInput += ValidateInput;
        Height.onValidateInput += ValidateInput;
    }

    private char ValidateInput(string text, int charIndex, char addedChar)
    {
        // 检查输入字符是否是数字
        if (!char.IsDigit(addedChar))
        {
            // 返回空字符，表示不接受输入字符
            return '\0';
        }

        // 允许输入字符
        return addedChar;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ConfirmFunc()
    {
        maze.SetTag("Long", int.Parse(Long.text));
        maze.SetTag("Width", int.Parse(Width.text));
        maze.SetTag("Height", int.Parse(Height.text));
        SceneManager.LoadScene("InitialInterface");
    }
}
