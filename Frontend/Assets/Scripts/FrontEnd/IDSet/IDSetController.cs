using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using KBEngine;

public class IDSetController : MonoBehaviour, ChangeSceneInterface {
    private int SignInSceneNum = SceneNum.SceneDict["SignIn"];
    private int MainSceneNum = SceneNum.SceneDict["Main"];
    private bool IDSetStatus = true;

	public InputField Name;
	public void ChangeScene(int num) {
		Application.LoadLevel(num);
	}

	public void LoadResource() {
		
	}

	public void JumpToSignIn() {
		ChangeScene(SignInSceneNum);
	}

	public void JumpToMain() {
		ChangeScene(MainSceneNum);
	}

	public void reqCreateName()
    {
    	//注册用户名
        if (Name.text == "")
        {
        	IDSetStatus = false;
            Debug.Log("空名字不允许");
            StartCoroutine(ShowErrorWindows());
            return;
        }

        Account Me = (Account)KBEngine.KBEngineApp.app.player();
        Me.baseCall("reqCreateName", Name.text);
        JumpToMain();
    }


    private IEnumerator ShowErrorWindows()
    {
        yield return new WaitForSeconds(3.0f);
        IDSetStatus = true;
        Debug.Log("After 3s");
    }

    private void OnGUI() {
        if(!IDSetStatus) {
            GUI.color = Color.green;
            GUI.Window(0, new Rect(Screen.width/12*5, Screen.height/12*5, Screen.width/12*2, Screen.height/12*2), DoWindow0, "IDSet Failed Message");
        }
    }

    private void DoWindow0(int windowID) {
        GUI.color = Color.red;
        GUI.Label(new Rect(Screen.width/24, Screen.height/24*3/2, Screen.width/12*2, Screen.height/12*2), "Empty name error");
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
