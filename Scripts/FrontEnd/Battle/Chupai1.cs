using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Chupai1 : MonoBehaviour {
    public int Idx;
    string temp = "";



    // Use this for initialization

    public static Chupai1 cp1;

    private void Awake()
    {
        if (cp1 != null)
        {
            return;
        }
        cp1 = this;
    }
    public static void Initial()
    {
        if (cp1 == null)
        {
            cp1 = new Chupai1();
        }
            
    }


    public void click()
    {
        //出卡
        //reqUseCard(int position)
        if (Global.GetInstance().My_Round == false)
        {
            //UnityEditor.EditorUtility.DisplayDialog("无效", "不是你的回合", "确认");
        }
        else
        {
            if (Global.GetInstance().My_Card_In.Count >= 6)
            {
                //UnityEditor.EditorUtility.DisplayDialog("无效", "场上已无空位", "确认");
            }
            else if (Global.GetInstance().canOperate == false)
            {
                //UnityEditor.EditorUtility.DisplayDialog("无效", "已出牌", "确认");
            }
            else
            {

                Debug.Log("准备出牌");
                BattleField.bf.reqUseCard(Idx);
                //send message to server   按钮：Idx  场上位置：Global.GetInstance().myInNum 
                //   Global.GetInstance().canOperate = false;
                //successful();   //
            }
        }
        
        
    }

    public void RefreshShoupai()
    {
        int i = 0;
        for (; i < Global.GetInstance().HandCard.Count; i++)     //刷新手牌
        {
            Global.GetInstance().HandCardModel[i].SetActive(false);
            Global.GetInstance().HandCardModel[i].SetActive(true);
            temp = "Texture/" + Global.GetInstance().HandCard[i].ToString();
            Global.GetInstance().HandCardsImages[i].overrideSprite = Resources.Load(temp, typeof(Sprite)) as Sprite;
            Global.GetInstance().buttons[i].SetActive(false);
            Global.GetInstance().buttons[i].SetActive(true);

        }
        for (; i < 5; i++)
        {
            Global.GetInstance().HandCardModel[i].SetActive(false); //后面的卡牌和按钮全部disactive
            Global.GetInstance().buttons[i].SetActive(false);
        }

        Debug.Log("剩余手牌为" + Global.GetInstance().My_CardLeft);
        Debug.Log("对方活力值为" + Global.GetInstance().Enemyenergy);
        Debug.Log("我方活力值为" + Global.GetInstance().Userenergy);
    }



    public void RefreshIn()
    {
        int i = 0;
        int tempID = 0; //记录出的卡牌
                        //    Debug.Log(Global.GetInstance().HandCard.Count);
        //tempID = Global.GetInstance().HandCard[Idx];    //获取出的牌
        
        Global.GetInstance().canOperate = true;



        for (; i<Global.GetInstance().My_Card_In.Count; i++)
        {
            
            temp = "";
            tempID = Global.GetInstance().My_Card_In[i];
            temp = "Texture/" + tempID.ToString();
            Global.GetInstance().MyCardPositions[i].overrideSprite = Resources.Load(temp, typeof(Sprite)) as Sprite;
            Global.GetInstance().mybuttons[i].SetActive(true);
            Global.GetInstance().myattk[i].text = Global.GetInstance().MyFollowerAttack[i].ToString();
            Global.GetInstance().myblood[i].text = Global.GetInstance().MyFollowerBlood[i].ToString();

        }

        for (; i < 6; i++)
        {
            temp = "";
            tempID = 100;
            temp = "Texture/" + tempID.ToString();
            Global.GetInstance().MyCardPositions[i].overrideSprite = Resources.Load(temp, typeof(Sprite)) as Sprite;
            Global.GetInstance().mybuttons[i].SetActive(false);
            Global.GetInstance().myattk[i].text = " ";
            Global.GetInstance().myblood[i].text = " ";

        }

        i = 0;

        for (; i < Global.GetInstance().Your_Card_In.Count; i++)
        {
            temp = "";
            tempID = Global.GetInstance().Your_Card_In[i];
            temp = "Texture/" + tempID.ToString();
            Global.GetInstance().YourCardPositions[i].overrideSprite = Resources.Load(temp, typeof(Sprite)) as Sprite;
            Global.GetInstance().yourbuttons[i].SetActive(true);
            Global.GetInstance().yourattk[i].text = Global.GetInstance().YourFollowerAttack[i].ToString();
            Global.GetInstance().yourblood[i].text = Global.GetInstance().YourFollowerBlood[i].ToString();
        }

        for (; i < 6; i++)
        {
            temp = "";
            tempID = 100;
            temp = "Texture/" + tempID.ToString();
            Global.GetInstance().YourCardPositions[i].overrideSprite = Resources.Load(temp, typeof(Sprite)) as Sprite;
            Global.GetInstance().yourbuttons[i].SetActive(false);
            Global.GetInstance().yourattk[i].text = " ";
            Global.GetInstance().yourblood[i].text = " ";
        }


    }
}
