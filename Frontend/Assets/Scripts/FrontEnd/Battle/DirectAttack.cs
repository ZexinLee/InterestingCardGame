using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectAttack : MonoBehaviour {

	public void click()
    {
        if (Global.GetInstance().IfAttack == true)
        {
            Global.GetInstance().Aim = 7;
            BattleField.bf.reqUseFollower(Global.GetInstance().Attacker, Global.GetInstance().Aim);
            Global.GetInstance().IfAttack = false;
            Global.GetInstance().Attacker = -1;
            Global.GetInstance().Aim = -1;
            //发送直接攻击信号
        }
    }
}
