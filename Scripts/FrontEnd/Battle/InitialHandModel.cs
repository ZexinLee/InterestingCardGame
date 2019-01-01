using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialHandModel : MonoBehaviour {
    public GameObject[] handmodels;
    public GameObject[] buttons;



	// Use this for initialization
	void Start () {
        Global.GetInstance().HandCardModel = handmodels;
        Global.GetInstance().buttons = buttons;
    }
	
	
}
