using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadLeftCard : MonoBehaviour {
    public Text myLeftCard;
    public Text yourLeftCard;
	// Use this for initialization
	public void Refresh()
    {
        myLeftCard.text = Global.GetInstance().My_CardLeft.ToString();
        yourLeftCard.text = Global.GetInstance().Your_CardLeft.ToString();

    }
}
