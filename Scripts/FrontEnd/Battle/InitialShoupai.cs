using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialShoupai : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string temp = "";
        int i = 0;
        for (i = 0; i < 5; i++)
        {
            Global.GetInstance().HandCardModel[i].SetActive(true);
            temp = "Texture/" + Global.GetInstance().HandCard[i].ToString();
            Global.GetInstance().HandCardsImages[i].overrideSprite = Resources.Load(temp, typeof(Sprite)) as Sprite;
            Global.GetInstance().buttons[i].SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
