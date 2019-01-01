using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Testround : MonoBehaviour {
    public Text round;
    
	// Update is called once per frame
	void LateUpdate () {
        round.text = Global.GetInstance().RoundNum.ToString();

	}
}
