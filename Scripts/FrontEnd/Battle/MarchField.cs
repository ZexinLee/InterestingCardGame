using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KBEngine;

public class March : MonoBehaviour {

    public void stopMarch()
    {
        print("reqDelMarcher");
        Account Me = (Account)KBEngine.KBEngineApp.app.player();

        if (Me != null)
        {
            Me.baseCall("reqStopMarch");
        }

    }

    public void startMarch()
    {

        Account Me = (Account)KBEngine.KBEngineApp.app.player();

        if (Me != null)
        {
            Me.baseCall("reqStartMarch");
        }

    }

    public void onMarchResult(object result)
    {
        if(int.Parse(result.ToString()) == 1)
        {
            Debug.Log("匹配成功");
        }
        else
        {
            Debug.Log("匹配失败");
        }
    }
}
