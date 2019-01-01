using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitialHpandATK : MonoBehaviour
{
    public Text[] myblood;
    public Text[] yourblood;
    public Text[] myattk;
    public Text[] yourattk;
    // Start is called before the first frame update
    void Start()
    {
        Global.GetInstance().myblood = myblood;
        Global.GetInstance().yourblood = yourblood;
        Global.GetInstance().myattk = myattk;
        Global.GetInstance().yourattk = yourattk;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
