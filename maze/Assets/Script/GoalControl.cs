using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class GoalControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player")
        {
            Player objplay=other.GetComponent<Player>();
            objplay.SetIsWin(true);
            GameObject.FindGameObjectWithTag("canvas").GetComponent<Text>().text = "��ϲ��ʤ����\r\n�ף������������һ��";
        }
    }
}
