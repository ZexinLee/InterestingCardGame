using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialIfAttack : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        int i = 0;
        Global.GetInstance().HasAttak = new bool[6];
        for (; i < 6; i++)
        {
            Global.GetInstance().HasAttak[i] = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
