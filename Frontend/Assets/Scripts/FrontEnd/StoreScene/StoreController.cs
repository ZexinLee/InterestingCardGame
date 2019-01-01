using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using KBEngine;

public class StoreController  : MonoBehaviour, ChangeSceneInterface {
    private int MainSceneNum = SceneNum.SceneDict["Main"];
    private GameObject Canvas0;
    private GameObject Canvas1;
	private GameObject RestMoneyText;
	private GameObject RestPackageText;
	private GameObject Button0; 
	private GameObject Button1;
	private GameObject Button2;
	private GameObject Button3;
	private GameObject Button4;
	private GameObject Image0;
	private GameObject Image1;
	private GameObject Image2;
	private GameObject Image3;
	private GameObject Image4;

	private bool OpenKaBaoStatus = true;
	private bool BuyCardStatus = true;

	private string PicturePath = "Texture/";
	private bool isShopOn;
	private bool OpenPackageStatus = false;
	private int PackageNum = Data.data.KaBao; // Debug
	private int RestMoney = Data.data.Money; // Debug

	private List<object> PackageResult = new List<object>();
    private int KabaoSum = 1; // Debug

    //PackageNum = PlayerData.data.KaBao; // Debug
	//RestMoney = PlayerData.data.Money; 

	public void ChangeScene(int num) {
		Application.LoadLevel(num);
	}

	public void LoadResource() {
		
	}

	public void JumpToMain() {
		Debug.Log(MainSceneNum);
		ChangeScene(MainSceneNum);
	}

	//Show package result
	//deal with null
	public void OpenOnePackage() {
		openKaBao();
		
	}

	private void RenderOpenPackageResult() {
		//Debug.Log("PackageRest is");
		//Debug.Log(PackageNum);
		OpenPackageStatus = true;
		Reload();
	}

	//Buy package result
	//deal with null
	public void BuyOnePackage() {
		BuyKaBao();
			//Debug.Log("Buy Successfully");
			//Debug.Log("RestMoney"+RestMoney.ToString());
		RenderBuyPackageResult();
	}

	private void RenderBuyPackageResult() {
		Reload();
	}

	//Show shop canvas
	public void EnterShop() {
		isShopOn = true;
		Reload();
	}

	//Unshow shop canvas
	public void QuitShop() {
		isShopOn = false;
		Reload();
	}

	private void GetObjectInThisScene() {
		Canvas0 = GameObject.Find("Canvas0");
		Canvas1 = GameObject.Find("Canvas1");
		RestPackageText = GameObject.Find("Canvas0/RestPackageText");
		RestMoneyText = GameObject.Find("Canvas1/RestMoneyText");
		Button0 = GameObject.Find("Canvas0/Button0"); 
		Button1 = GameObject.Find("Canvas0/Button1");
		Button2 = GameObject.Find("Canvas0/Button2");
		Button3 = GameObject.Find("Canvas0/Button3");
		Button4 = GameObject.Find("Canvas0/Button4");
		Image0 = GameObject.Find("Canvas0/Image0");
		Image1 = GameObject.Find("Canvas0/Image1");
		Image2 = GameObject.Find("Canvas0/Image2");
		Image3 = GameObject.Find("Canvas0/Image3");
		Image4 = GameObject.Find("Canvas0/Image4");
	}

	private void Reload() {
		if(isShopOn) {
			Canvas0.SetActive(false);
			RestMoneyText.GetComponent<Text>().text = RestMoney.ToString();
			//shop
		}
		else {
			Canvas0.SetActive(true);
			RestPackageText.GetComponent<Text>().text = PackageNum.ToString();
			//open package
		}

		if(OpenPackageStatus) {
			Image0.SetActive(true);
			Image1.SetActive(true);
			Image2.SetActive(true);
			Image3.SetActive(true);
			Image4.SetActive(true);
			string Path;
			Path = PicturePath + (int.Parse(PackageResult[0].ToString()) + 1).ToString();
			Image0.GetComponent<Image>().overrideSprite = Resources.Load(Path, typeof(Sprite)) as Sprite;
			Path = PicturePath + (int.Parse(PackageResult[1].ToString()) + 1).ToString();
			Image1.GetComponent<Image>().overrideSprite = Resources.Load(Path, typeof(Sprite)) as Sprite;
			Path = PicturePath + (int.Parse(PackageResult[2].ToString()) + 1).ToString();
			Image2.GetComponent<Image>().overrideSprite = Resources.Load(Path, typeof(Sprite)) as Sprite;
			Path = PicturePath + (int.Parse(PackageResult[3].ToString()) + 1).ToString();
			Image3.GetComponent<Image>().overrideSprite = Resources.Load(Path, typeof(Sprite)) as Sprite;
			Path = PicturePath + (int.Parse(PackageResult[4].ToString()) + 1).ToString();
			Image4.GetComponent<Image>().overrideSprite = Resources.Load(Path, typeof(Sprite)) as Sprite;
		}
		
		else {
			Image0.SetActive(false);
			Image1.SetActive(false);
			Image2.SetActive(false);
			Image3.SetActive(false);
			Image4.SetActive(false);
		}

		OpenPackageStatus = false;
	}


    //------------------------------------------------------------------------------------------
	/*
		API offered by Server
	*/
	public void openKaBao()
    {
        Account Me = (Account)KBEngine.KBEngineApp.app.player();
        if (Data.data.KaBao > 0 && Me != null && PackageNum > 0)
        {
        	PackageNum -= 1;
            Debug.Log("本地检查通过，提交开卡请求");
            Me.baseCall("reqOpenKaBao");
        }
        else
        {
            Debug.Log("开卡失败");
            OpenKaBaoStatus = false;
            StartCoroutine(ShowErrorWindows());
        }
    }

    public void onOpenKaBaoResult(object data)
    {
        PackageResult = (List<object>)data;
        foreach (object i in PackageResult)
        {
            Debug.Log("获取到卡包结果:" + i.ToString());
        }
        RenderOpenPackageResult();
    }

    //public void BuyKaBao(int KabaoSum)
    public void BuyKaBao()
    {
        //int KabaoSum = 1;
        Account Me = (Account)KBEngine.KBEngineApp.app.player();
        if (KabaoSum > 0 && Me != null && RestMoney >= 100)
        {
        	PackageNum += 1;
        	RestMoney -= 100;
            Debug.Log("本地检查通过，提交购买数据");
            Me.baseCall("reqBuyKaBao", KabaoSum);
        }
        else
        {
            Debug.Log("购买失败");
            BuyCardStatus = false;
            StartCoroutine(ShowErrorWindows());
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


    private IEnumerator ShowErrorWindows()
    {
        yield return new WaitForSeconds(3.0f);
        BuyCardStatus = true;
        OpenKaBaoStatus = true;
        Debug.Log("After 3s");
    }

    private void OnGUI() {
        if(!OpenKaBaoStatus) {
            GUI.color = Color.green;
            GUI.Window(0, new Rect(Screen.width/12*5, Screen.height/12*5, Screen.width/12*2, Screen.height/12*2), DoWindow0, "Store Failed Message");
        }
        if(!BuyCardStatus) {
            GUI.color = Color.green;
            GUI.Window(0, new Rect(Screen.width/12*5, Screen.height/12*5, Screen.width/12*2, Screen.height/12*2), DoWindow1, "Store Failed Message");
        }
    }

    private void DoWindow0(int windowID) {
        GUI.color = Color.red;
        GUI.Label(new Rect(Screen.width/24, Screen.height/24*3/2, Screen.width/12*2, Screen.height/12*2), "Failed to buy cards");
    }

    private void DoWindow1(int windowID) {
        GUI.color = Color.red;
        GUI.Label(new Rect(Screen.width/24, Screen.height/24*3/2, Screen.width/12*2, Screen.height/12*2), "Failed to openKaBao");
    }

    //------------------------------------------------------------------------------------------
	// Use this for initialization
	void Start () {
		KBEngine.Event.registerOut("onOpenKaBaoResult", this, "onOpenKaBaoResult");
		KBEngine.Event.registerOut("onBuyKaBaoResult", this, "onBuyKaBaoResult");
		GetObjectInThisScene();
		Reload();
		Debug.Log("reach");
	}
	
	// Update is called once per frame
	void Update () {
		//ChangeCameraPos();
	}
}
