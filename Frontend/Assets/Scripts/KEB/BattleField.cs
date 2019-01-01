using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KBEngine;
using UnityEngine.SceneManagement;

public class BattleField : MonoBehaviour {



    public static BattleField bf;


    private void Awake()
    {
        if (bf != null)
        {
            return;
        }
        bf = this;
    }

    public void stopMarch()
    {
        print("reqDelMarcher");
        Account Me = (Account)KBEngine.KBEngineApp.app.player();

        if (Me != null)
        {
            Me.baseCall("reqStopMarch");
        }


    }

    public void startMarch(int role)
    {

        Account Me = (Account)KBEngine.KBEngineApp.app.player();

        if (Me != null)
        {
            Me.baseCall("reqStartMarch",role);
        }
    }

    public void onMarchResult(object result, object name)
    {
        if(int.Parse(result.ToString()) > -1)
        {
            Debug.Log("匹配成功");
            //跳转对战场景
            SceneManager.LoadScene(5);
            Data.data.marchsuccess = true;
            Global.GetInstance().Username = Data.data.Name;
            Debug.Log("匹配成功");
            Global.GetInstance().Enemyname = name.ToString();
        }
        else
        {
            Debug.Log("匹配失败");
        }
    }

    private void Start()
    {
        KBEngine.Event.registerOut("onInitBattleField", this, "onMarchResult");

        KBEngine.Event.registerOut("onNextRound_Your", this, "onNextRound_Your");
        KBEngine.Event.registerOut("onNextRound_Opps", this, "onNextRound_Opps");
        KBEngine.Event.registerOut("onUseCard", this, "onUseCard");
        KBEngine.Event.registerOut("onRetinueAction", this, "onRetinueAction");
        KBEngine.Event.registerOut("onRemoveHandCard", this, "onRemoveHandCard");

        KBEngine.Event.registerOut("onBattleEndResult", this, "onBattleEndResult");
        KBEngine.Event.registerOut("recChatMsg", this, "recChatMsg");

    }

    public void reqNextRound()
    {
        Account Me = (Account)KBEngine.KBEngineApp.app.player();

        if (Me != null)
        {
            Me.baseCall("reqNextRound");
        }
    }

    public void recChatMsg(object data)
    {
        string msg = data.ToString();
        Global.GetInstance().Chat.text = Global.GetInstance().Chat.text + "\n" + Global.GetInstance().Enemyname + "：" + msg;
    }

    public void reqGiveUp()
    {
        Account Me = (Account)KBEngine.KBEngineApp.app.player();

        if (Me != null)
        {
            Me.baseCall("reqGiveUp");
        }
    }

    public void reqUseCard(int position)
    {
        Debug.Log("出牌");
        Account Me = (Account)KBEngine.KBEngineApp.app.player();

        if (Me != null)
        {
            Me.baseCall("reqUseCard", position,0);
        }
    }

    public void reqUseFollower(int position, int target)
    {

        Account Me = (Account)KBEngine.KBEngineApp.app.player();

        if (Me != null)
        {
            Me.baseCall("reqRetinueAction", position, target);
        }
    }



    public void onNextRound_Your(object data)
    {
        int i = 0;
        Data.data.Round = int.Parse(data.ToString());
        Debug.Log("当前我方回合：" + Data.data.Round.ToString());
        //开始下一个你的回合
        Global.GetInstance().RoundNum = Data.data.Round;
        Global.GetInstance().My_Round = true;
        Global.GetInstance().MyRoundInfo.SetActive(false);
        Global.GetInstance().MyRoundInfo.SetActive(true);
        for(;i< Global.GetInstance().HasAttak.Length; i++)
        {
            Global.GetInstance().HasAttak[i] = false;
        }
        
    }

    public void onNextRound_Opps(object data)
    {
        Data.data.Round = int.Parse(data.ToString());
        Debug.Log("当前对方回合：" + Data.data.Round.ToString());
        //开始下一个对方的回合
        Global.GetInstance().RoundNum = Data.data.Round;
        Global.GetInstance().My_Round = false;
        Global.GetInstance().YourRoundInfo.SetActive(false);
        Global.GetInstance().YourRoundInfo.SetActive(true);
    }

    public void onUseCard(object data)
    {
        
        int cardID = int.Parse(data.ToString()) + 1;
        Debug.Log("开始渲染");
        Chupai1.Initial();
        Chupai1.cp1.RefreshIn();
        if(cardID > 0)
        {
            ShowShoupai.showshoupai(cardID);
        }
        
    }

    public void onRemoveHandCard()
    {
        Chupai1.Initial();
        Chupai1.cp1.RefreshShoupai();
    }



    public void onRetinueAction()
    {
        Chupai1.cp1.RefreshIn();
        //使用随从之后调用
    }

    public void onBattleEndResult(object r, object money, object exp, object rank)
    {
        int result = int.Parse(r.ToString());
        int rmpney = int.Parse(money.ToString());
        int rexp = int.Parse(exp.ToString());
        int rrank = int.Parse(rank.ToString());
        if (result == 1)
        {
            Debug.Log("你获得了胜利！");
            Winin.win();
        }
        else
        {
            Debug.Log("你也太垃圾了了吧。");
            Winin.loss();
        }

        Data.data.marchsuccess = false;
    }


    public void sendChatMsg(string msg)
    {
        Account Me = (Account)KBEngine.KBEngineApp.app.player();

        if (Me != null)
        {
            Me.baseCall("reqChat", msg);
            Debug.Log("发送消息");
        }
    }

}
