using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpandDown : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public static void UpDown()
    {
        foreach (Action act in Data.InfoList)
        {
            if (act.action == "att")
            {
                if (act.end == 8)
                {
                    Global.GetInstance().MyPressureUp.GetComponent < Text >().text = "+"+act.value.ToString();
                    Global.GetInstance().MyPressureUp.SetActive(false);
                    Global.GetInstance().MyPressureUp.SetActive(true);
                }
                else if (act.end == -8)
                {
                    Global.GetInstance().YourPressureUp.GetComponent<Text>().text = "+" + act.value.ToString();
                    Global.GetInstance().YourPressureUp.SetActive(false);
                    Global.GetInstance().YourPressureUp.SetActive(true);
                }

            }
            if (act.action == "cure")
            {
                if (act.end == 8)
                {
                    Global.GetInstance().MyPressureUp.GetComponent<Text>().text = act.value.ToString();
                    Global.GetInstance().MyPressureUp.SetActive(false);
                    Global.GetInstance().MyPressureUp.SetActive(true);
                }
                else if (act.end == -8)
                {
                    Global.GetInstance().YourPressureUp.GetComponent<Text>().text = act.value.ToString();
                    Global.GetInstance().YourPressureUp.SetActive(false);
                    Global.GetInstance().YourPressureUp.SetActive(true);
                }

            }
            if (act.action == "add")
            {
                if (act.end == 8)
                {
                    Global.GetInstance().MyPressureUp.GetComponent<Text>().text = act.value.ToString();
                    Global.GetInstance().MyPressureUp.SetActive(false);
                    Global.GetInstance().MyPressureUp.SetActive(true);
                }
                else if (act.end == -8)
                {
                    Global.GetInstance().YourPressureUp.GetComponent<Text>().text = act.value.ToString();
                    Global.GetInstance().YourPressureUp.SetActive(false);
                    Global.GetInstance().YourPressureUp.SetActive(true);
                }

            }

        }

    }

   
}
