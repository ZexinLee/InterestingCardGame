using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitialRoundInfo : MonoBehaviour
{
    public GameObject t1;
    public GameObject t2;
    


    // Start is called before the first frame update
    void Start()
    {
        Global.GetInstance().MyRoundInfo = t1;
        Global.GetInstance().YourRoundInfo = t2;
    }

    // Update is called once per frame
    void Update()

    {
        
    }
}
