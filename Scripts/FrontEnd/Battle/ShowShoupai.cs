using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowShoupai : MonoBehaviour
{
    // Start is called before the first frame update
    public static void showshoupai(int id)
    {
        string temp = "";
        temp = "Texture/" + id.ToString();
        Global.GetInstance().Detail.overrideSprite = Resources.Load(temp, typeof(Sprite)) as Sprite;
    }
}
