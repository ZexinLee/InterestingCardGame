using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour, ChangeSceneInterface {
    private int MainSceneNum = SceneNum.SceneDict["Main"];
	public void ChangeScene(int num) {
		Application.LoadLevel(num);
	}

	public void LoadResource() {
		
	}

	public void JumpToMain() {
		ChangeScene(MainSceneNum);
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
