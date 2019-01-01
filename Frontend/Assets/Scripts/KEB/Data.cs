using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Data : MonoBehaviour {

    public static Data data;

    public string Name;
    public int Money;
    public int Role;
    public int Grade;
    public int EXP;
    public int Rank;
    public int KaBao;

    public int Round;

    public static List<Dictionary<int, int>> cardList = new List<Dictionary<int, int>>(); //拥有的卡牌列表，前一个为卡牌ID，后一个收集度
    public static List<int> HandCardList = new List<int>(); //手牌列表，都是卡牌ID，10000开始
    public static Dictionary<string, int> BattleData = new Dictionary<string, int>(); //
    //key : selfenergy selfstress oppenergy oppstress selfhandcardnum selfcardnum opphandcardnum oppcardnum
    public static List<Action> InfoList = new List<Action>(); //动作列表，Action类
    public static FieldData Field = new FieldData();

    public static string ChatMsg;

    public bool marchsuccess = false;

    private void Awake()
    {
        if (data != null)
        {
            Destroy(gameObject);
            return;
        }
        data = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        KBEngine.Event.registerOut("set_Data", this, "set_Data");
        KBEngine.Event.registerOut("onName", this, "onName");
        KBEngine.Event.registerOut("set_CardList", this, "set_CardList");
        KBEngine.Event.registerOut("onServerMsg", this, "onServerMsg");
        KBEngine.Event.registerOut("set_BattleHandCardList", this, "set_BattleHandCardList");
        KBEngine.Event.registerOut("set_BattleData", this, "set_BattleData");
        KBEngine.Event.registerOut("set_BattleInfoList", this, "set_BattleInfoList");
        KBEngine.Event.registerOut("set_Field", this, "set_Field");
        KBEngine.Event.registerOut("set_ChatMsg", this, "set_ChatMsg");
        KBEngine.Event.registerOut("recv_BattleData", this, "recv_BattleData");
    }

    public void onName(object getData)
    {
        Name = (string)getData;
        Debug.Log("用户名称为：" + Name);
        
    }

    public void onChooseRole(object data)
    {
        Role = int.Parse(data.ToString());
        Debug.Log("用户角色为：" + Role);
    }

    public void set_Data(object getdata)
    {
        Dictionary<string, object> info = (Dictionary<string, object>)getdata;
        Name = (string)info["name"];
        Debug.Log("以获取到" + Name + "初始数据");
        Money = Convert.ToInt32(info["money"]);
        Role = Convert.ToInt32(info["role"]);
        Grade = Convert.ToInt32(info["grade"]);
        EXP = Convert.ToInt32(info["exp"]);
        Rank = Convert.ToInt32(info["rank"].ToString());
        KaBao = Convert.ToInt32(info["kabao"]);
    }

    public void set_CardList(object data)
    {
        
        List<object> cl = (List<object>)data;
        cardList.Clear();
        foreach (object p in cl)
        {
            Dictionary<string, object> dic = (Dictionary<string, object>)p;
            Dictionary<int, int> dic2 = new Dictionary<int, int>();
            dic2.Add(Convert.ToInt32(dic["cardID"])+1, Convert.ToInt32(dic["value"]));
            cardList.Add(dic2);
        }
        Debug.Log("成功获取用户卡牌,卡牌数：" + cardList.Count);
    }

    public void onServerMsg(object data)
    {
        Debug.Log("服务器向你发送信息：" + (string)data);
    }

    public void set_BattleHandCardList(object data)
    {
        List<object> cl = (List<object>)data;
        HandCardList.Clear();
        foreach (object p in cl)
        {
            HandCardList.Add(int.Parse(p.ToString())+1);
        }
        Debug.Log("手牌信息更新：" + HandCardList.ToString());
        Global.GetInstance().HandCard = HandCardList;
    }

    public void recv_BattleData(object data)
    {
        Debug.Log("BattleData信息更新：");
        BattleData.Clear();
        List<object> info = (List<object>)data;
        BattleData.Add("selfenergy", int.Parse(info[0].ToString()));
        BattleData.Add("selfenergylimit", int.Parse(info[1].ToString()));
        BattleData.Add("selfstress", int.Parse(info[2].ToString()));
        BattleData.Add("selfstresslimit", int.Parse(info[3].ToString()));
        BattleData.Add("oppenergy", int.Parse(info[4].ToString()));
        BattleData.Add("oppenergylimit", int.Parse(info[5].ToString()));
        BattleData.Add("oppstress", int.Parse(info[6].ToString()));
        BattleData.Add("oppstresslimit", int.Parse(info[7].ToString()));
        BattleData.Add("selfhandcardnum", int.Parse(info[8].ToString()));
        BattleData.Add("selfcardnum", int.Parse(info[9].ToString()));
        BattleData.Add("opphandcardnum", int.Parse(info[10].ToString()));
        BattleData.Add("oppcardnum", int.Parse(info[11].ToString()));

        Debug.Log("RECV   selfenergy" + info[0].ToString() + " oppenergy" + info[4].ToString());


        Global.GetInstance().Userpressure = BattleData["selfstress"].ToString();
        Global.GetInstance().Enemypressure = BattleData["oppstress"].ToString();

        Global.GetInstance().Userenergy = BattleData["selfenergy"].ToString();
        Global.GetInstance().Enemyenergy = BattleData["oppenergy"].ToString();

        Global.GetInstance().UserenergyLimit = BattleData["selfenergylimit"].ToString();
        Global.GetInstance().EnemyenergyLimit = BattleData["oppenergylimit"].ToString();
        Global.GetInstance().UserpressureLimit = BattleData["selfstresslimit"].ToString();
        Global.GetInstance().EnemypressureLimit = BattleData["oppstresslimit"].ToString();

        Global.GetInstance().My_NumofHandcard = BattleData["selfhandcardnum"];
        Global.GetInstance().Enemy_NumofHandcard = BattleData["opphandcardnum"];

        Global.GetInstance().My_CardLeft = BattleData["selfcardnum"];
        Global.GetInstance().Your_CardLeft = BattleData["oppcardnum"];
    }

    public void set_BattleData(object data)
    {
       /* Debug.Log("BattleData信息更新：");
        BattleData.Clear();
        Dictionary<string, object> info = (Dictionary<string, object>)data;
        BattleData.Add("selfenergy", Convert.ToInt32(info["selfenergy"]));
        BattleData.Add("selfenergylimit", Convert.ToInt32(info["selfenergylimit"]));
        BattleData.Add("selfstress", Convert.ToInt32(info["selfstress"]));
        BattleData.Add("selfstresslimit", Convert.ToInt32(info["selfstresslimit"]));
        BattleData.Add("oppenergy", Convert.ToInt32(info["oppenergy"]));
        BattleData.Add("oppenergylimit", Convert.ToInt32(info["oppenergylimit"]));
        BattleData.Add("oppstress", Convert.ToInt32(info["oppstress"]));
        BattleData.Add("oppstresslimit", Convert.ToInt32(info["oppstresslimit"]));
        BattleData.Add("selfhandcardnum", Convert.ToInt32(info["selfhandcardnum"]));
        BattleData.Add("selfcardnum", Convert.ToInt32(info["selfcardnum"]));
        BattleData.Add("opphandcardnum", Convert.ToInt32(info["opphandcardnum"]));
        BattleData.Add("oppcardnum", Convert.ToInt32(info["oppcardnum"]));

        Global.GetInstance().Userenergy = BattleData["selfenergy"].ToString();
        Global.GetInstance().Enemyenergy = BattleData["oppenergy"].ToString();
        Global.GetInstance().Userpressure = BattleData["selfstress"].ToString();
        Global.GetInstance().Enemypressure = BattleData["oppstress"].ToString();

        Global.GetInstance().UserenergyLimit = BattleData["selfenergylimit"].ToString();
        Global.GetInstance().EnemyenergyLimit = BattleData["oppenergylimit"].ToString();
        Global.GetInstance().UserpressureLimit = BattleData["selfstresslimit"].ToString();
        Global.GetInstance().EnemypressureLimit = BattleData["oppstresslimit"].ToString();

        Global.GetInstance().My_NumofHandcard = BattleData["selfhandcardnum"];
        Global.GetInstance().Enemy_NumofHandcard = BattleData["opphandcardnum"];

        Global.GetInstance().My_CardLeft = BattleData["selfcardnum"];
        Global.GetInstance().Your_CardLeft = BattleData["oppcardnum"];
        */
        

    }


    public void set_Field(object data)
    {
        Debug.Log("对战Field更新：");
        Field.Clear();
        Dictionary<string, object> dic = (Dictionary<string, object>)data;
        List<object> s = (List<object>)dic["selffollower"];

        Global.GetInstance().My_Card_In.Clear();
        Global.GetInstance().MyFollowerBlood.Clear();
        Global.GetInstance().MyFollowerAttack.Clear();
        foreach (object p in s)
        {
            Follower s2 = new Follower();
            Dictionary<string, object> di2 = (Dictionary<string, object>)p;
            s2.id = Convert.ToInt32(di2["id"])+1;
            s2.att = Convert.ToInt32(di2["att"]);
            s2.hp = Convert.ToInt32(di2["hp"]);
            s2.cost = Convert.ToInt32(di2["cost"]);
            Field.selfAdd(s2);
            Debug.Log("我方场上加卡" + s2.id);
            Global.GetInstance().My_Card_In.Add(s2.id);
            Global.GetInstance().MyFollowerBlood.Add(s2.hp);
            Global.GetInstance().MyFollowerAttack.Add(s2.att);
        }
        List<object> o = (List<object>)dic["oppfollower"];

        Global.GetInstance().Your_Card_In.Clear();
        Global.GetInstance().YourFollowerBlood.Clear();
        Global.GetInstance().YourFollowerAttack.Clear();
        foreach (object p in o)
        {
            Follower o2 = new Follower();
            Dictionary<string, object> di2 = (Dictionary<string, object>)p;
            o2.id = Convert.ToInt32(di2["id"])+1;
            o2.att = Convert.ToInt32(di2["att"]);
            o2.hp = Convert.ToInt32(di2["hp"]);
            o2.cost = Convert.ToInt32(di2["cost"]);
            Field.oppAdd(o2);
            Debug.Log("对方场上加卡" + o2.id);
            Global.GetInstance().Your_Card_In.Add(o2.id);
            Global.GetInstance().YourFollowerBlood.Add(o2.hp);
            Global.GetInstance().YourFollowerAttack.Add(o2.att);
        }

    }


    public void set_BattleInfoList(object data)
    {
        Debug.Log("对战动作更新：");
        List<object> cl = (List<object>)data;
        InfoList.Clear();
        Debug.Log(cl.Count);
        foreach (object p in cl)
        {
            Dictionary<string, object> dic = (Dictionary<string, object>)p;
            Action a = new Action();
            a.action = Convert.ToString(dic["action"]);
            a.start = Convert.ToInt32(dic["start"]);
            a.end = Convert.ToInt32(dic["end"]);
            a.value = Convert.ToInt32(dic["value"]);
            if (a.action == "power" || a.action == "bepower")
            {
                a.valuehp = Convert.ToInt32(dic["valuehp"]);
            }
            InfoList.Add(a);
        }
        ShowAttack.Attack();
        UpandDown.UpDown();
    }


    public void set_ChatMsg(object data)
    {
        Debug.Log("聊天信息更新");
        ChatMsg = data.ToString();

    }
    
    

    public class FieldData
    {
        public List<Follower> selffollower
        {
            get { return _selffollower; }
            set { _selffollower = value; }
        }

        public void selfAdd(Follower f)
        {
            _selffollower.Add(f);
        }

        private List<Follower> _selffollower = new List<Follower>();
        public List<Follower> oppfollower
        {
            get { return _oppfollower; }
            set { _oppfollower = value; }
        }

        public void oppAdd(Follower f)
        {
            _oppfollower.Add(f);
        }

        private List<Follower> _oppfollower = new List<Follower>();

        public void Clear()
        {
            _selffollower.Clear();
            _oppfollower.Clear();
        }
    }

    public class Follower
    {
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }
        private int _id;

        public int hp
        {
            get { return _hp; }
            set { _hp = value; }
        }
        private int _hp;

        public int att
        {
            get { return _att; }
            set { _att = value; }
        }
        private int _att;

        public int cost
        {
            get { return _cost; }
            set { _cost = value; }
        }
        private int _cost;
    }
}
