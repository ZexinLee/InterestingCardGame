using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitialHandImages : MonoBehaviour {
    public Image[] images;
	// Use this for initialization
	void Start () {
        Global.GetInstance().HandCardsImages = images;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
