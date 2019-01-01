using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Winin : MonoBehaviour
{
   public static void win()
    {
        Global.GetInstance().cavas.SetActive(false);
        Animation a = Global.GetInstance().Camera.GetComponent<Animation>();
        if(a.isPlaying == false)
        {
            a.Play("Win");
        }
        Global.GetInstance().cavas2.SetActive(true);
    }

    public static void loss()
    {
        Global.GetInstance().cavas.SetActive(false);
        Animation a = Global.GetInstance().Camera.GetComponent<Animation>();
        if (a.isPlaying == false)
        {
            a.Play("Win");
        }
        Global.GetInstance().cavas3.SetActive(true);
    }
}
