using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitialChat : MonoBehaviour
{
    public Text Chat;
    public Text Input;
    // Start is called before the first frame update
    void Start()
    {
        Global.GetInstance().Chat = Chat;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void click()
    {
         Global.GetInstance().Chat.text = Global.GetInstance().Chat.text + "\n" + Global.GetInstance().Username + "：" + Input.text;
        // Global.GetInstance().Chat.text = Global.GetInstance().Chat.text + "\n" + "大帅哥" + "：" + Input.text;
        BattleField.bf.sendChatMsg(Input.text);
    }
}
