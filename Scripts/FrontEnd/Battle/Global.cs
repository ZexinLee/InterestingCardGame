using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Global : Singleton<Global>
{

    public string Username;
    public string Enemyname;
    public string Userenergy = "10";
    public string Enemyenergy = "10";
    public string Userpressure = "0";
    public string Enemypressure = "0";
    public string UserenergyLimit = "10";
    public string EnemyenergyLimit = "10";
    public string UserpressureLimit = "30";
    public string EnemypressureLimit = "30";

    public string Username2;
    public string Userenergy2 = "10";
    public string Userpressure2 = "0";


    public int My_NumofHandcard;    //手牌数
    public int Enemy_NumofHandcard;

    public int RoundNum = 0;

    public int My_CardLeft;
    public int Your_CardLeft;

    public List<int> My_Card_In  = new List<int>();    //场上形势（有哪些卡）
    public List<int> Your_Card_In = new List<int>();

    public int myInNum = 0;     //我方场上牌的数量
    public int yourInNum = 0;   //对方场上牌的数量

    public List<int> HandCard;

    public int Attacker = -1;
    public bool IfAttack = false;
    public int Aim = -1;

    public int EnemyAttacker = 1;          //处理被攻击的指令
    public bool IfBeAttacked = true;
    public int EnemyAim = 1;


    public GameObject[] Myparticles; //爆炸特效
    public GameObject[] Yourparticles; //爆炸特效

    public Image[] MyCardPositions;     //我方场上的卡    //存场上位置上的图片
    public Image[] YourCardPositions;   //对方场上的卡
    public GameObject[] mybuttons;    //我方场上卡牌对应的按钮
    public GameObject[] yourbuttons;
    public Image Detail;


    public GameObject[] HandCardModel;  //手牌模型
    public Image[] HandCardsImages; //手牌模型贴图
    public GameObject[] buttons;    //手牌附带按钮

    public List<int> MyFollowerBlood = new List<int>(); //我方战场上怪兽的血量
    public List<int> YourFollowerBlood = new List<int>(); 
    public List<int> MyFollowerAttack = new List<int>();  //我方战场上怪兽的攻击力
    public List<int> YourFollowerAttack = new List<int>();

    public Text[] myblood;
    public Text[] yourblood;
    public Text[] myattk;
    public Text[] yourattk;


    public GameObject MyRoundInfo;
    public GameObject YourRoundInfo;


    public GameObject MyEnergyUp;
    public GameObject YourEnergyUp;
    public GameObject MyPressureUp;
    public GameObject YourPressureUp;
    public GameObject MyPressureDown;
    public GameObject YourPressureDown;

    public Text Chat;

    public GameObject cavas;
    public GameObject cavas2;
    public GameObject cavas3;
    public GameObject Camera;

    public bool My_Round = false;

    public bool[] HasAttak;

    public bool IfBuff = false;

    


    public bool canOperate = true;


    public static Global instance;







    void Start()
    {


        if (instance == null)
            instance = this;




}
    private void Awake()
    {
        instance = this;
    }



    

}
