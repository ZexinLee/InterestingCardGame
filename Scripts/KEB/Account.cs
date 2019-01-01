namespace KBEngine
{
	using UnityEngine;
	using System.Collections;
	using System.Collections.Generic;
	using System;
    using UnityEngine.SceneManagement;

    public class Account : Entity
    {

        public override void __init__()
        {
            base.__init__();
            Debug.Log("登陆成功 已经创建Account实体");

            KBEngine.Event.fireOut("onLoginSuccessfully");
        }

        public void recv_BattleData(object data)
        {

            KBEngine.Event.fireOut("recv_BattleData", data);
        }



        public void reqName()
        {
            //更新用户名
            baseCall("reqName");
        }

        

        public void onName(object data)
        {
            KBEngine.Event.fireOut("onName", data);
        }

        public void onBuyKaBaoResult(object data)
        {
            KBEngine.Event.fireOut("onBuyKaBaoResult", data);
        }

        public void onNextRound_Your(object data)
        {

            KBEngine.Event.fireOut("onNextRound_Your", data);
            //开始下一个你的回合
        }

        public void onNextRound_Opps(object data)
        {

            KBEngine.Event.fireOut("onNextRound_Opps", data);
            //开始下一个对方的回合
        }

        public void onUseCard(object data)
        {
            KBEngine.Event.fireOut("onUseCard", data);
            //使用卡之后调用
        }

        public void onRetinueAction()
        {
            KBEngine.Event.fireOut("onRetinueAction");
            //使用随从之后调用
        }


        public void onBattleEndResult(object r, object money, object exp, object rank)
        {
            KBEngine.Event.fireOut("onBattleEndResult", r, money, exp, rank);
            //对战结果
        }


        public void onOpenKaBaoResult(object data)
        {
            KBEngine.Event.fireOut("onOpenKaBaoResult", data);
        }

        public void set_Data(object data)
        {
            Event.fireOut("set_Data", getDefinedProperty("Data"));
        }

        public void set_CardList(object data)
        {
            KBEngine.Event.fireOut("set_CardList", getDefinedProperty("CardList"));
        }

        public void onInitBattleField(object data, object name)
        {
            KBEngine.Event.fireOut("onInitBattleField", data, name);
        }

        public void onServerMsg(object data)
        {
            KBEngine.Event.fireOut("onServerMsg", data);
        }

        public void set_BattleHandCardList(object data)
        {
            Event.fireOut("set_BattleHandCardList", data);
        }

        public void set_BattleData(object data)
        {
            Event.fireOut("set_BattleData", data);
        }
        public void onRemoveHandCard()
        {
            KBEngine.Event.fireOut("onRemoveHandCard");
            //使用卡之后调用
        }
        public void recChatMsg(object data)
        {
            KBEngine.Event.fireOut("recChatMsg",data);
            //使用卡之后调用
        }



        public void set_BattleInfoList(object data)
        {
            Event.fireOut("set_BattleInfoList", data);
        }

        public void set_Field(object data)
        {
            Event.fireOut("set_Field", data);
        }
    }    
}