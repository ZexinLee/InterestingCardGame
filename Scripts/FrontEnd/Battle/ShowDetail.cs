using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowDetail : MonoBehaviour {
    public Image detail;
    public int idx;
    string temp = "";
    Animation attacker1;
   // public GameObject[] Myparticles;
  //  public GameObject[] Yourparticles;
  

    void Start()
    {
        
      
    }


    public void Click()
    {
        int i = 0;
        //Debug.Log(Global.GetInstance().My_Card_In[1]);
        if(idx >= 0)
        {
            if (Global.GetInstance().My_Card_In.Count >= idx)       //在左边展示卡片细节
            {
                i = Global.GetInstance().My_Card_In[idx - 1];
            }
            temp = "Texture/" + i.ToString();
            Global.GetInstance().Detail.overrideSprite = Resources.Load(temp, typeof(Sprite)) as Sprite;


            
            Global.GetInstance().Attacker = idx-1;   //确认攻击发起者
            Debug.Log("攻击者是" + Global.GetInstance().Attacker);
            if (Global.GetInstance().IfAttack == true)      //如果攻击对象选的是自己，则变成重新选择攻击对象
            {
                Global.GetInstance().IfAttack = false;
                Global.GetInstance().Attacker = -1;
                //UnityEditor.EditorUtility.DisplayDialog("无效", "无法攻击自己", "确认");
            }
        }
        if (idx < 0)
        {
            if (Global.GetInstance().Your_Card_In.Count >= -idx) //在左边显示卡片细节
            {
                i = Global.GetInstance().Your_Card_In[-idx - 1];
            }
            temp = "Texture/" + i.ToString();
            Global.GetInstance().Detail.overrideSprite = Resources.Load(temp, typeof(Sprite)) as Sprite;


            if (Global.GetInstance().Attacker != -1 && Global.GetInstance().HasAttak[Global.GetInstance().Attacker] == true)
            {
                //UnityEditor.EditorUtility.DisplayDialog("无效", "该卡片本回合已攻击过", "确认");
            }



            else if (Global.GetInstance().Attacker!= -1 && Global.GetInstance().IfAttack == true && Global.GetInstance().HasAttak[Global.GetInstance().Attacker] == false)
            {
                Global.GetInstance().Aim = -idx-1;   //确认目标
                Debug.Log("攻击目标是" + Global.GetInstance().Aim);
                Debug.Log("你猜此时Global的ifattack为" + Global.GetInstance().IfAttack);
                //发送攻击指令
                //reqUseFollower(int attacker, int aim)
                BattleField.bf.reqUseFollower(Global.GetInstance().Attacker, Global.GetInstance().Aim);
                Debug.Log("准备攻击");

                //sendmessage    attacker position: Global.GetInstance().Attacker         aim position: Global.GetInstance().Aim


             //   successful();
            }
            
        }

    }




}
