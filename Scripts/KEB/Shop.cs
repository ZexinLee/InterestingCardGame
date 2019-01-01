using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KBEngine;

public class Shop : MonoBehaviour {

    bool c = true;
    private void Start()
    {
        KBEngine.Event.registerOut("onBuyKaBaoResult", this, "onBuyKaBaoResult");
    }

    public void buyKaBao()
    {
        int KabaoSum = 1;
        Account Me = (Account)KBEngine.KBEngineApp.app.player();
        if (KabaoSum > 0 && Me != null)
        {
            Debug.Log("本地检查通过，提交购买数据");
            Me.baseCall("reqBuyKaBao", KabaoSum);
        }
        else
        {
            Debug.Log("购买失败");
        }
    }

    public void onBuyKaBaoResult(object data)
    {
        if (int.Parse(data.ToString()) == 1)
        {
            Debug.Log("购买成功");
        }
        else
        {
            Debug.Log("购买失败" + int.Parse(data.ToString()));
        }
    }

}
