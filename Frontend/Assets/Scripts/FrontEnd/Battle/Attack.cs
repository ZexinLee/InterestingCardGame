using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {

	public void click()
    {
        if(Global.GetInstance().Attacker == -1)
        {
            //UnityEditor.EditorUtility.DisplayDialog("无效", "请选择攻击发起者后再点攻击", "确认");
        }
        else
        {
            Global.GetInstance().IfAttack = true;
        }
    }
}
