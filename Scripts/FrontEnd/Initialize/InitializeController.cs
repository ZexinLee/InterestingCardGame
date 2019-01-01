using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using KBEngine;

public class InitializeController : MonoBehaviour, ChangeSceneInterface, SignInInterface
{
    private int SignUpSceneNum = SceneNum.SceneDict["SignUp"];
    private int SetIDSceneNum = SceneNum.SceneDict["SetID"];
    private int MainSceneNum = SceneNum.SceneDict["Main"];
    public InputField stringAccount;
    public InputField stringPasswd;
    private bool LoginStatus = true;

    private string Name;

    public void ChangeScene(int num)
    {
        Application.LoadLevel(num);
    }

    public void LoadResource()
    {

    }

    public void Login()
    {

        Debug.Log("正在登陆，账号：" + stringAccount.text);

        KBEngine.Event.fireIn("login", stringAccount.text, stringPasswd.text, System.Text.Encoding.UTF8.GetBytes("PC"));
    }

    public void onLoginFailed(UInt16 failedcode)
    {
        LoginStatus = false;
        StartCoroutine(ShowErrorWindows());
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

    public void onName(object getData)
    {
        Name = (string)getData;
        Debug.Log("用户名称为：" + Name);
        if (Name == "")
        {
            JumpToSetID();
        }
        else
        {
            JumpToMain();
        }
    }

    public void JumpToSignUp()
    {
        ChangeScene(SignUpSceneNum);
    }

    private void JumpToSetID()
    {
        ChangeScene(SetIDSceneNum);
    }

    private void JumpToMain()
    {
        ChangeScene(MainSceneNum);
    }

    public void JumpToNext()
    {
        Login();
    }

    private IEnumerator ShowErrorWindows()
    {
        yield return new WaitForSeconds(3.0f);
        LoginStatus = true;
        Debug.Log("After 3s");
    }

    private void OnGUI()
    {
        if (!LoginStatus)
        {
            GUI.color = Color.green;
            GUI.Window(0, new Rect(Screen.width / 12 * 5, Screen.height / 12 * 5, Screen.width / 12 * 2, Screen.height / 12 * 2), DoWindow0, "Login Failed Message");
        }
    }

    private void DoWindow0(int windowID)
    {
        GUI.color = Color.red;
        GUI.Label(new Rect(Screen.width / 24, Screen.height / 24 * 3 / 2, Screen.width / 12 * 2, Screen.height / 12 * 2), "Failed to login in ");
    }

    public void Start()
    {
        KBEngine.Event.registerOut("onName", this, "onName");
        KBEngine.Event.registerOut("onLoginFailed", this, "onLoginFailed");
        KBEngine.Event.registerOut("onLoginSuccessfully", this, "onLoginSuccessfully");
    }

    // Update is called once per frame
    void Update()
    {

    }
}