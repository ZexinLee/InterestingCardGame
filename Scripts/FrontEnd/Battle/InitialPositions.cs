using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitialPositions : MonoBehaviour {     //初始化场上卡位
    public Image[] mypositions;
    public Image[] yourpositions;
    public GameObject[] mybuttons;
    public GameObject[] yourbuttons;
    public Image Detail;

    string temp = "";


    List<int> list1 = new List<int>();
    List<int> list2 = new List<int>();

    // Use this for initialization
    void Start () {
        int i = 0;
        Global.GetInstance().MyCardPositions = mypositions;
        Global.GetInstance().YourCardPositions = yourpositions;
        Global.GetInstance().mybuttons = mybuttons;
        Global.GetInstance().yourbuttons = yourbuttons;
        Global.GetInstance().Detail = Detail;

        //test
        //list1.Add(1);
        //list1.Add(4);
        //list1.Add(5);


        //list2.Add(2);


        for (i = 0; i < list1.Count; i++)           //读我方场上的卡片
        {
            temp = "Texture/" + list1[i].ToString();
           // Debug.Log(Global.GetInstance().MyCardPositions.Length);
           // Debug.Log(list1.Count);
           // Debug.Log(list2.Count + "  2222");
            Global.GetInstance().MyCardPositions[i].overrideSprite = Resources.Load(temp, typeof(Sprite)) as Sprite;
            Global.GetInstance().myInNum += 1;      //记录总牌数
        }
        for (; i < 6; i++)
        {
            Global.GetInstance().mybuttons[i].SetActive(false);
        }
        for (i = 0; i < list2.Count; i++)       //读对方场上的卡牌
        {
            temp = "Texture/" + list2[i].ToString();
            Global.GetInstance().YourCardPositions[i].overrideSprite = Resources.Load(temp, typeof(Sprite)) as Sprite;
            Global.GetInstance().yourInNum += 1;
        }
        for (; i < 6; i++)
        {
            Global.GetInstance().yourbuttons[i].SetActive(false);
        }
        Global.GetInstance().My_Card_In = list1;
        Global.GetInstance().Your_Card_In = list2;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
