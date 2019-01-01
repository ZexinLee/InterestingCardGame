using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KBEngine;

public class OpenKaBao : MonoBehaviour {

    private void Start()
    {
        KBEngine.Event.registerOut("onOpenKaBaoResult", this, "onOpenKaBaoResult");
    }

    public void openKaBao()
    {
        Account Me = (Account)KBEngine.KBEngineApp.app.player();
        if (Data.data.KaBao > 0 && Me != null)
        {
            Debug.Log("本地检查通过，提交开卡请求");
            Me.baseCall("reqOpenKaBao");
        }
        else
        {
            Debug.Log("开卡失败");
        }
    }

    public void onOpenKaBaoResult(object data)
    {
        List<uint> result = (List<uint>)data;
        foreach (uint i in result)
        {
            Debug.Log("获取到卡包结果:" + i.ToString());
        }
    }

}
