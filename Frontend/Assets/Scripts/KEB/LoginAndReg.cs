using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using KBEngine;

public class LoginAndReg : MonoBehaviour {

    string stringPasswd;
    string stringAccount;

    string regID;
    string regPassword;
    string regPassword2;

    string nameSet;

    bool c = true;

    public void Start()
    {
        KBEngine.Event.registerOut("onLoginFailed", this, "onLoginFailed");
        KBEngine.Event.registerOut("onLoginSuccessfully", this, "onLoginSuccessfully");
        KBEngine.Event.registerOut("onCreateAccountResult", this, "onCreateAccountResult");
    }

    public void login()
    {
        stringAccount = "123";
        stringPasswd = "123";

        Debug.Log("正在登陆，账号：" + stringAccount);

        KBEngine.Event.fireIn("login", stringAccount, stringPasswd, System.Text.Encoding.UTF8.GetBytes("PC"));
    }

    public void CreateAccont()
    {
        stringAccount = "123";
        stringPasswd = "123";

        KBEngine.Event.fireIn("createAccount", stringAccount, stringPasswd, System.Text.Encoding.UTF8.GetBytes("PC"));
    }

    public void onCreateAccountResult(UInt16 retcode, object datas)
    {
        Debug.LogFormat("注册结果，返回码：{0},返回结果：{1}", retcode, KBEngineApp.app.serverErr(retcode));
    }

    public void onLoginFailed(UInt16 failedcode)
    {
        if (failedcode == 20)
        {
            Debug.Log("login is failed(登陆失败), err=" + KBEngineApp.app.serverErr(failedcode) + ", " + System.Text.Encoding.ASCII.GetString(KBEngineApp.app.serverdatas()));
        }
        else
        {
            Debug.Log("login is failed(登陆失败), err=" + KBEngineApp.app.serverErr(failedcode));
        }
    }

    public void onLoginSuccessfully()
    {
        Debug.Log("登录成功！");
        Account Me = (Account)KBEngine.KBEngineApp.app.player();
        print("调用获取姓名方法");
        Me.reqName();
        //登录成功后会执行此方法
        //进入游戏主界面
    }

    public void reqChooseRole()
    {
        int role = 1;

        if (role == null)
        {
            Debug.Log("空ROLE不允许");
            return;
        }

        Account Me = (Account)KBEngine.KBEngineApp.app.player();
        Me.baseCall("reqChooseRole", role);

    }

    public void reqCreateName()
    {
        string Name = "zzr";

        if (Name == "")
        {
            Debug.Log("空名字不允许");
            return;
        }

        Account Me = (Account)KBEngine.KBEngineApp.app.player();
        Me.baseCall("reqCreateName", Name);

    }

    
}
