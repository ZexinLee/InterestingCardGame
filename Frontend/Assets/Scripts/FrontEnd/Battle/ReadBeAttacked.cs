using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadBeAttacked : MonoBehaviour {
    Animation attacker1;

	// Use this for initialization
	public void Read () {

        Debug.Log("进入函数");
        if (Global.GetInstance().EnemyAttacker!=-1 && Global.GetInstance().EnemyAim != -1 && Global.GetInstance().IfBeAttacked == true)
        {
            Debug.Log("开始渲染对方攻击");
            Global.GetInstance().Myparticles[Global.GetInstance().EnemyAim-1].SetActive(false); //场上是从1开始编号
            Global.GetInstance().Myparticles[Global.GetInstance().EnemyAim-1].SetActive(true);
            attacker1 = Global.GetInstance().YourCardPositions[Global.GetInstance().EnemyAttacker - 1].GetComponent<Animation>();
            attacker1.Play("Attacker1");

        }
		
	}

}
