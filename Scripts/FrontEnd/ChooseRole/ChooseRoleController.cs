using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseRoleController : MonoBehaviour, ChangeSceneInterface
{
    private int BattleNum = SceneNum.SceneDict["Battle"];
    private int MainSceneNum = SceneNum.SceneDict["Main"];
    private int CurrentRole = -1;
    private bool StopMatch = false;
    private string Path0 = "Texture/Role0";
    private string Path1 = "Texture/Role1";
    private string Path2 = "Texture/Role2";
    private string Path3 = "Texture/Role3";
    private GameObject Button1;
    private GameObject Button2;
    private GameObject Button3;
    private GameObject Button4;
    private GameObject Button5;
    private GameObject Button6;
    private GameObject DetailImage;
    private GameObject Canvas1;
    private GameObject TimeText;

    private bool ChooseRoleStatus = true;

    public void ChangeScene(int num)
    {
        Application.LoadLevel(num);
    }

    public void LoadResource()
    {

    }

    public void JumpToBattle()
    {
        if (CurrentRole != -1)
        {
            BattleField.bf.startMarch(CurrentRole);
        }
        Debug.Log("Current: " + CurrentRole.ToString());
        /*
        if (CurrentRole == -1)
        {
            ChooseRoleStatus = false;
            StartCoroutine(ShowErrorWindows());
        }
        else
        {
            Canvas1.SetActive(true);
            //StartCoroutine(ShowMatchingWindows(CurrentRole));
            //ChangeScene(BattleNum);
            GoToBattle(CurrentRole);
        }*/
    }

    private void GoToBattle(int role) {
        BattleField.bf.startMarch(role);
        while (true)
        {
            if (Data.data.marchsuccess)
            {
                break;
            }
            if (StopMatch)
            {
                StopMatch = false;
                BattleField.bf.stopMarch();
                return;
            }
        }
        ChangeScene(BattleNum);
    }

    private IEnumerator ShowMatchingWindows(int role)
    {
        int timecount = 0;
        Button1.SetActive(false);
        Button2.SetActive(false);
        BattleField.bf.startMarch(role);
        while (true)
        {
            TimeText.GetComponent<Text>().text = timecount.ToString();
            if (Data.data.marchsuccess)
            {

                break;
            }
            if (StopMatch)
            {
                Canvas1.SetActive(false);
                Button1.SetActive(true);
                Button2.SetActive(true);
                StopMatch = false;
                BattleField.bf.stopMarch();
                yield break;
            }
            yield return new WaitForSeconds(1.0f);
            timecount += 1;
        }
        ChangeScene(BattleNum);
    }

    public void ToStopMatch()
    {
        StopMatch = true;
    }

    public void JumpToMain()
    {
        ChangeScene(MainSceneNum);
    }

    private void Reload()
    {
        DetailImage.SetActive(true);
        Canvas1.SetActive(false);
        switch (CurrentRole)
        {
            case 0: DetailImage.GetComponent<Image>().overrideSprite = Resources.Load(Path0, typeof(Sprite)) as Sprite; break;
            case 1: DetailImage.GetComponent<Image>().overrideSprite = Resources.Load(Path1, typeof(Sprite)) as Sprite; break;
            case 2: DetailImage.GetComponent<Image>().overrideSprite = Resources.Load(Path2, typeof(Sprite)) as Sprite; break;
            case 3: DetailImage.GetComponent<Image>().overrideSprite = Resources.Load(Path3, typeof(Sprite)) as Sprite; break;
            default: DetailImage.SetActive(false); break;
        }
    }

    public void ChangeCurrentRole(int RoleNum)
    {
        CurrentRole = RoleNum;
        Reload();
    }

    private void GetObjectInThisScene()
    {
        Button1 = GameObject.Find("Canvas/Button1");
        Button2 = GameObject.Find("Canvas/Button2");
        Button3 = GameObject.Find("Canvas/Button3");
        Button4 = GameObject.Find("Canvas/Button4");
        Button5 = GameObject.Find("Canvas/Button5");
        Button6 = GameObject.Find("Canvas/Button6");
        DetailImage = GameObject.Find("Canvas/Image2");
        Canvas1 = GameObject.Find("Canvas1");
        TimeText = GameObject.Find("Canvas1/Text1");
    }

    private IEnumerator ShowErrorWindows()
    {
        yield return new WaitForSeconds(3.0f);
        ChooseRoleStatus = true;
        Debug.Log("After 3s");
    }

    private void OnGUI()
    {
        if (!ChooseRoleStatus)
        {
            GUI.color = Color.green;
            GUI.Window(0, new Rect(Screen.width / 12 * 5, Screen.height / 12 * 5, Screen.width / 12 * 2, Screen.height / 12 * 2), DoWindow0, "Choose Role Failed Message");
        }
    }

    private void DoWindow0(int windowID)
    {
        GUI.color = Color.red;
        GUI.Label(new Rect(Screen.width / 24, Screen.height / 24 * 3 / 2, Screen.width / 12 * 2, Screen.height / 12 * 2), "Unselect Role");
    }

    // Use this for initialization
    void Start()
    {
        GetObjectInThisScene();
        Reload();
    }

    // Update is called once per frame
    void Update()
    {

    }
}