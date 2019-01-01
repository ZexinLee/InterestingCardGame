using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ReadInfo : MonoBehaviour { //更新场上信息
    public Text myEnergy;
    public Text myPressure;
    public Text yourEnergy;
    public Text yourPressure;
    public Text round;
    public Text myleft;
    public Text yourleft;
    public Text username;
    public Text enemyname;

    public Text username2;
    public Text userenergy2;
    public Text userpressure2;


    public GameObject MyEnergyUp;
    public GameObject YourEnergyUp;
    public GameObject MyPressureUp;
    public GameObject YourPressureUp;
    public GameObject MyPressureDown;
    public GameObject YourPressureDown;

    public Text Chat;

    public void Start()
    {
        Global.GetInstance().MyEnergyUp = MyEnergyUp;
        Global.GetInstance().YourEnergyUp = YourEnergyUp;
        Global.GetInstance().MyPressureUp = MyPressureUp;
        Global.GetInstance().YourPressureUp = YourPressureUp;
        Global.GetInstance().MyPressureDown = MyPressureDown;
        Global.GetInstance().YourPressureDown = YourPressureDown;
        Global.GetInstance().Chat = Chat;
    }

    public void Update()
    {
        int i;
        //Debug.Log("我的名字" + Global.GetInstance().Username);
        myEnergy.text = Global.GetInstance().Userenergy + "/" + Global.GetInstance().UserenergyLimit;
        myPressure.text = Global.GetInstance().Userpressure + "/" + Global.GetInstance().UserpressureLimit;
        yourEnergy.text = Global.GetInstance().Enemyenergy + "/" + Global.GetInstance().EnemyenergyLimit;
        yourPressure.text = Global.GetInstance().Enemypressure + "/" + Global.GetInstance().EnemypressureLimit;
        round.text = Global.GetInstance().RoundNum.ToString();
        myleft.text = Global.GetInstance().My_CardLeft.ToString();
        yourleft.text = Global.GetInstance().Your_CardLeft.ToString();
        username.text = Global.GetInstance().Username;
        enemyname.text = Global.GetInstance().Enemyname;
        username2.text = Global.GetInstance().Username;
        userenergy2.text = Global.GetInstance().Userenergy + "/" + Global.GetInstance().UserenergyLimit;
        userpressure2.text = Global.GetInstance().Userpressure + "/" + Global.GetInstance().UserpressureLimit;
        //Debug.Log(Global.GetInstance().Enemyname);
        Chat.text = Global.GetInstance().Chat.text;


    }
}
