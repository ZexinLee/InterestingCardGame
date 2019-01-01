using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainController : MonoBehaviour, ChangeSceneInterface {
    //private int BattleSceneNum = SceneNum.SceneDict["Battle"];
    private int ChooseRoleNum = SceneNum.SceneDict["ChooseRole"];
    private int StoreSceneNum = SceneNum.SceneDict["Store"];
    private int CollectionSceneNum = SceneNum.SceneDict["Collection"];
    private int LoginSceneNum = SceneNum.SceneDict["SignIn"];
    private int CameraMovingCount = 0;
    private GameObject ID;
    private GameObject Level;
    private GameObject Money;
    private GameObject EXP;
    private int maxEXP = 100;


    //private Vector3 offset;
	private bool isMoving = false;
	//private float t = 0.5f;

	public Transform Camera;
	//public Vector3 fromPos;
    private Vector3 toPos = new Vector3(-129,3,-58);

	public void ChangeScene(int num) {
		Application.LoadLevel(num);
	}

	public void LoadResource() {

	}

	public void JumpToBattle() {
		//ChangeScene(BattleSceneNum);
		ChangeScene(ChooseRoleNum);
	}

	public void JumpToStore() {
		isMoving = true;
		//ChangeScene(StoreSceneNum);
	}

	public void JumpToColletion() {
		ChangeScene(CollectionSceneNum);
	}

	public void QuitGame() {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            Debug.Log ("编辑状态游戏退出");
        #else
            Application.Quit();
            Debug.Log ("游戏退出");
        #endif
	}

	public void ConfirmFirstLogin() {
		bool FirstLogin = true;
		if(FirstLogin) {
			// begin The tutorial
			// Send Message to confirm not first login
		}
	}

	public void ChangeCameraPos() {
		if (!isMoving)
           return;
 		CameraMovingCount += 1;
 		Debug.Log(CameraMovingCount);
 		if(CameraMovingCount > 120) {
 			ChangeScene(StoreSceneNum);
 		}
        Camera.position = Vector3.Lerp(Camera.position, toPos, Time.deltaTime * 2f);
	}

	private void GetObjectInThisScene() {
		ID = GameObject.Find("Canvas/ID");
    	Level = GameObject.Find("Canvas/Text7");
    	Money = GameObject.Find("Canvas/Text8");
    	EXP = GameObject.Find("Canvas/Slider");
    	EXP.GetComponent<Slider>().maxValue = maxEXP;
    	Debug.Log(Data.data.EXP);
	}

	private void Reload() {
		ID.GetComponent<Text>().text = Data.data.Name.ToString();
		Level.GetComponent<Text>().text = ((int)(Data.data.EXP/maxEXP)).ToString();
		Money.GetComponent<Text>().text = Data.data.Money.ToString();
		EXP.GetComponent<Slider>().value = (int)(Data.data.EXP % maxEXP);
	}

	// Use this for initialization
	void Start () {
		FileLoader.GetInstance();
		GetObjectInThisScene();
		Reload();
		Debug.Log("Money:"+Data.data.Money.ToString());
	}
	
	// Update is called once per frame
	void Update () {
		ChangeCameraPos();
	}
}
