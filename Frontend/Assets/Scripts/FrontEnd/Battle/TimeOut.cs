using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeOut : MonoBehaviour {
    public Text roundNum;
    /*
    private void Start()
    {
        Global.GetInstance().RoundNum += 1;
    }*/
    public void click()
    {
        //send 
        Global.GetInstance().canOperate = false;
        //send timeout
        
        BattleField.bf.reqNextRound();




        //reqNextRound();
        //send time out
    }
}
