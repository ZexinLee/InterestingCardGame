using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using UnityEngine.UI;

public class CollectionController  : MonoBehaviour, ChangeSceneInterface {
    private int MainSceneNum = SceneNum.SceneDict["Main"];
	private bool isDetailAble = false;
	private int AbleButtonNum = 5;
	private int ShowPageNum = 0; 
	private int ShowDetailNum = 0;
	//private int ShowGroupNum = 0;
	//private string PicturePath = "C:\\Users\\Administrator\\Desktop\\project\\Card\\Assets\\Plugins\\Archive";
	private string PicturePath = "Texture/";
	private string Path;

	private List<List<int>> ActiveList = new List<List<int>>(); // length:6
	private List<string> StoryList = new List<string>();

	//private List<Texture2D> NoUseTexture = new List<Texture2D>(); // To release memory
	//private List<Texture2D> BadTexture = new List<Texture2D>(); // To release memory

	private GameObject Canvas;
	private GameObject Image0; 
	private GameObject Image1;
	private GameObject Image2;
	private GameObject Image3;
	private GameObject DetailImage; 
	private GameObject Button0; 
	private GameObject Button1; 
	private GameObject Button2; 
	private GameObject Button3;
	private GameObject Button4;
	private GameObject Button5;
	private GameObject Button6;
	private GameObject Button7;
	private GameObject Button8;
	private GameObject Button9;
	private GameObject Button10;
	private GameObject Button11;
	private GameObject Story;

	public void ChangeScene(int num) {
		Application.LoadLevel(num);
	}

	public void LoadResource() {
		
	}

	public void JumpToMain() {
		ChangeScene(MainSceneNum);
	}

	private void GetObjectInThisScene() {
		Canvas = GameObject.Find("Canvas");
		Image0 = GameObject.Find("Canvas/Image0");
		Image1 = GameObject.Find("Canvas/Image1");
		Image2 = GameObject.Find("Canvas/Image2");
		Image3 = GameObject.Find("Canvas/Image3");
		DetailImage = GameObject.Find("Canvas/Image4");
		Button0 = GameObject.Find("Canvas/Button0");
		Button1 = GameObject.Find("Canvas/Button1");
		Button2 = GameObject.Find("Canvas/Button2");
		Button3 = GameObject.Find("Canvas/Button3");
		Button4 = GameObject.Find("Canvas/Button4");
		Button5 = GameObject.Find("Canvas/Button5");
		Button6 = GameObject.Find("Canvas/Button6");
		Button7 = GameObject.Find("Canvas/Button7");
		Button8 = GameObject.Find("Canvas/Button8");
		Button9 = GameObject.Find("Canvas/Button9");
		Button10 = GameObject.Find("Canvas/Button10");
		Button11 = GameObject.Find("Canvas/Button11");
		Story = GameObject.Find("Canvas/Text");
	}
	
	private void Reload() {
		int maxNum = ShowPageNum * 4 + 3;
		int maxIndex = ActiveList[AbleButtonNum].Count - 1;
		Debug.Log(maxNum);
		int renderNum = 0;

		if(maxNum == 3)
			Button4.SetActive(false);
		else
			Button4.SetActive(true);
		if(maxNum >= maxIndex)
			Button5.SetActive(false);
		else
			Button5.SetActive(true);
		//Debug
		//isDetailAble = true;
		//isDetailAble = false;
		if(isDetailAble == true) {
			Story.GetComponent<Text>().text = StoryList[ActiveList[AbleButtonNum][ShowDetailNum]];
			Path = PicturePath+(ActiveList[AbleButtonNum][ShowDetailNum]+1).ToString();
			DetailImage.GetComponent<Image>().overrideSprite = Resources.Load(Path, typeof(Sprite)) as Sprite;
			//LoadByIO(DetailImage.GetComponent<RawImage>(),PicturePath+"\\img ("+(ShowDetailNum+1).ToString()+").png",isDetailAble);
			DetailImage.SetActive(true);
			Story.SetActive(true);
			//foreach(Texture2D t in NoUseTexture) {
			//	Destroy(t);
			//}
			//Set(Detail_image).path = ShowDetailNum.ToString() +".jpg";
			//Set(Detail_image).isDetailAble;
		}
		
		else {
			Story.SetActive(false);
			DetailImage.SetActive(false);
			Image0.SetActive(false);
			Image1.SetActive(false);
			Image2.SetActive(false);
			Image3.SetActive(false);
			Button6.SetActive(false);
			Button7.SetActive(false);
			Button8.SetActive(false);
			Button9.SetActive(false);

			//To release the memory
			//foreach(Texture2D t in BadTexture) {
			//	Destroy(t);
			//}
			// Set(Detail_image).isDetailAble;
			// Set(image.maxNum).isDetailAble;
			// Set(image.maxNum-1).isDetailAble;
			// Set(image.maxNum-2).isDetailAble;
			// Set(image.maxNum-3).isDetailAble;
			if(maxNum <= maxIndex) {
				renderNum = 4;
				goto index4;
			}
			else if(maxNum - 1 <= maxIndex) {
				renderNum = 3;
				goto index3;
			}
			else if(maxNum - 2 <= maxIndex) {
				renderNum = 2;
				goto index2;
			}
			else if(maxNum - 3 <= maxIndex) {
				renderNum = 1;
				goto index1;
			}
			else {
				goto index0;
			}
			index4:
				//LoadByIO(Image3.GetComponent<RawImage>(),PicturePath+"\\img ("+(maxNum+1).ToString()+").png",isDetailAble);
				//img (1)
				//temp = "Texture/" + i.ToString();
            	//detail.overrideSprite = Resources.Load(temp, typeof(Sprite)) as Sprite;

				Path = PicturePath+(ActiveList[AbleButtonNum][maxNum]+1).ToString();
				Debug.Log(Path);
				Image3.GetComponent<Image>().overrideSprite = Resources.Load(Path, typeof(Sprite)) as Sprite;
			
				Image3.SetActive(true);
				Button9.SetActive(true);
				// Set(image.maxNum).path = maxNum +".jpg";
				//Set(image.maxNum).able;
			index3:
				//LoadByIO(Image2.GetComponent<RawImage>(),PicturePath+"\\img ("+(maxNum+0).ToString()+").png",isDetailAble);
				Path = PicturePath+(ActiveList[AbleButtonNum][maxNum-1]+1).ToString();
				Image2.GetComponent<Image>().overrideSprite = Resources.Load(Path, typeof(Sprite)) as Sprite;
			
				Image2.SetActive(true);
				Button8.SetActive(true);
				// Set(image.maxNum-1).path = maxNum +".jpg";
				//Set(image.maxNum-1).able;
			index2:
				//LoadByIO(Image1.GetComponent<RawImage>(),PicturePath+"\\img ("+(maxNum-1).ToString()+").png",isDetailAble);
				Path = PicturePath+(ActiveList[AbleButtonNum][maxNum-2]+1).ToString();
				Image1.GetComponent<Image>().overrideSprite = Resources.Load(Path, typeof(Sprite)) as Sprite;
			
				Image1.SetActive(true);
				Button7.SetActive(true);
				// Set(image.maxNum-2).path = maxNum +".jpg";
				//Set(image.maxNum-2).able;
			index1:
				//LoadByIO(Image0.GetComponent<RawImage>(),PicturePath+"\\img ("+(maxNum-2).ToString()+").png",isDetailAble);
				Path = PicturePath+(ActiveList[AbleButtonNum][maxNum-3]+1).ToString();
				Image0.GetComponent<Image>().overrideSprite = Resources.Load(Path, typeof(Sprite)) as Sprite;
			

				Image0.SetActive(true);
				Button6.SetActive(true);
				// Set(image.maxNum-3).path = maxNum +".jpg";
				//Set(image.maxNum-3).able;
			index0:
				Debug.Log("no card");
			}
		}

	public void ReduceShowPage() {
		if(ShowPageNum == 0) {
			//Debug.Log("Head");
			return;
		}
		else {
			ShowPageNum -= 1;
			isDetailAble = false;
			Reload();
		}
	}

	public void AddShowPage() {
		//Debug.Log("maxNum:"+(ShowPageNum*4+4).ToString());
		//Debug.Log("Need:"+(ActiveList[AbleButtonNum].Count).ToString());
		if(ShowPageNum*4+4 >= ActiveList[AbleButtonNum].Count) {
			return;
		}
		else {
			ShowPageNum += 1;
			isDetailAble = false;
			Reload();
		}
	}


	public void ChangeShowGroup(int num) {
		if(num == AbleButtonNum) {
			AbleButtonNum = 5;
		}
		else {
			AbleButtonNum = num;
		}
		ShowPageNum = 0;
		isDetailAble = false;
		Reload();
	}

	public void ShowDetail(int num) {
		isDetailAble = true;
		ShowDetailNum = ShowPageNum*4+num;
		Reload();
	}

	public void FetchData() {
		//For debug: i
		//i: cardNum
		FileLoader.GetInstance();
		int cardNum = FileLoader.card.cardnum;
		//Debug.Log(cardNum);
		List<int> group0 = new List<int>();
		List<int> group1 = new List<int>();
		List<int> group2 = new List<int>();
		List<int> group3 = new List<int>();
		List<int> group4 = new List<int>();
		List<int> group5 = new List<int>();

		for(int i = 0;i < cardNum;i++) {
			int groupNum = FileLoader.card.config[i].effect.group;
			StoryList.Add(FileLoader.card.config[i].story);
			group5.Add(i);
			switch(groupNum) {
				case 0: group0.Add(i); break;
				case 1: group1.Add(i); break;
				case 2: group2.Add(i); break;
				case 3: group3.Add(i); break;
				case 4: group4.Add(i); break;
			}
		}

		//Debug.Log(//StoryList.Count);
		ActiveList.Add(group0);
		ActiveList.Add(group1);
		ActiveList.Add(group2);
		ActiveList.Add(group3);
		ActiveList.Add(group4);
		ActiveList.Add(group5);

		//Debug.Log(ActiveList[4].Count);
		// string s = "";
		// for(int i = 0;i < ActiveList[4].Count;i++) {
		// 	s += ActiveList[4][i].ToString();
		// 	s += " ";
		// }
		// Debug.Log(s);
	}

	//Test
	public void OnClickChangeButtonStyle() {
		/*if(Button11.active == false) {
			Button11.SetActive(true);
		}
		else {
			Button11.SetActive(false);
		}*/
	}

	void Awake() {
		FetchData();
		GetObjectInThisScene();
		Reload();
		//fetch data from PlayerData
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
