using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ReadHandCard: MonoBehaviour {
  /*  public Image image1;
    public Image image2;
    public Image image3;
    public Image image4;
    public Image image5;*/
   /* public Image[] Hands;
    public GameObject[] models;
    public GameObject[] buttons;*/
    string temp = "";
    List<int> list = new List<int>();
    private void Start()
    {
        
    }

    


    public void getHandCard()
    {
        if (Global.GetInstance().My_Round == false)
        {
            //UnityEditor.EditorUtility.DisplayDialog("无效", "不是你的回合", "确认");
        }
        else
        {
            int i = 0;


            for (; i < Global.GetInstance().HandCard.Count; i++)
            {
                Global.GetInstance().HandCardModel[i].SetActive(false);
                Global.GetInstance().buttons[i].SetActive(false);
                Global.GetInstance().HandCardModel[i].SetActive(true);
                Global.GetInstance().buttons[i].SetActive(true);
                temp = "Texture/" + Global.GetInstance().HandCard[i].ToString();
                Global.GetInstance().HandCardsImages[i].overrideSprite = Resources.Load(temp, typeof(Sprite)) as Sprite;
            }
            //Debug.Log(i);
            for (; i < 5; i++)
            {
                Global.GetInstance().HandCardModel[i].SetActive(false);
                Global.GetInstance().buttons[i].SetActive(false);
            }
        }
        
        //Global.GetInstance().HandCard = list;

        // Debug.Log(Global.GetInstance().My_Card_In[0]);

    }


    // Update is called once per frame
    void Update () {
		
	}
}
