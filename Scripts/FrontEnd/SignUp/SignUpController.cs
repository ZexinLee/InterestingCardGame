using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using KBEngine;

public class SignUpController : MonoBehaviour, ChangeSceneInterface, SignUpInterface
{
    private int SignInSceneNum = SceneNum.SceneDict["SignIn"];
    private bool RegStatus = true;
    public InputField regID;
    public InputField regPassword;
    public InputField regPassword2;

    public void ChangeScene(int num)
    {
        Application.LoadLevel(num);
    }

    public void LoadResource()
    {

    }

    public void JumpToSignIn()
    {
        ChangeScene(SignInSceneNum);
    }

    public void CreateAccont()
    {
        if (regPassword2.text != regPassword.text)
        {
            //UnityEditor.EditorUtility.DisplayDialog("无效", "两次密码不一样", "确认");
        }
        else
        {
            KBEngine.Event.fireIn("createAccount", regID.text, regPassword.text, System.Text.Encoding.UTF8.GetBytes("PC"));
        }
        //用户名，密码


    }

    public void onCreateAccountResult(UInt16 retcode, object datas)
    {
        Debug.LogFormat("注册结果，返回码：{0},返回结果：{1}", retcode, KBEngineApp.app.serverErr(retcode));
        if (retcode != 0)
        {
            RegStatus = false;
            StartCoroutine(ShowErrorWindows());
        }
        else
        {
            JumpToSignIn();
        }
    }

    private IEnumerator ShowErrorWindows()
    {
        yield return new WaitForSeconds(3.0f);
        RegStatus = true;
        Debug.Log("After 3s");
    }

    private void OnGUI()
    {
        if (!RegStatus)
        {
            GUI.color = Color.green;
            GUI.Window(0, new Rect(Screen.width / 12 * 5, Screen.height / 12 * 5, Screen.width / 12 * 2, Screen.height / 12 * 2), DoWindow0, "Register Failed Message");
        }
    }

    private void DoWindow0(int windowID)
    {
        GUI.color = Color.red;
        GUI.Label(new Rect(Screen.width / 24, Screen.height / 24 * 3 / 2, Screen.width / 12 * 2, Screen.height / 12 * 2), "Failed to create account");
    }
    // Use this for initialization
    void Start()
    {
        KBEngine.Event.registerOut("onCreateAccountResult", this, "onCreateAccountResult");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
