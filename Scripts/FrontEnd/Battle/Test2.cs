using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test2: MonoBehaviour {
    public GameObject[] cameras;
    public bool changeAudioListener = true;
    public int flag = 0;

    /*public void ChangeScene(int num)
    {
        Application.LoadLevel(num);
    }
    // Use this for initialization*/
    void SwitchCamera(int index)
    {
        int i = 0;
        for (i = 0; i < cameras.Length; i++)
        {
            if (i != index)
            {
                if (changeAudioListener)
                {
                    cameras[i].GetComponent<AudioListener>().enabled = false;
                }
                cameras[i].GetComponent<Camera>().enabled = false;
            }
            else
            {
                if (changeAudioListener)
                {
                    cameras[i].GetComponent<AudioListener>().enabled = true;
                }
                cameras[i].GetComponent<Camera>().enabled = true;
            }
        }
    }

    public void click()
    {
        if(flag == 0)
        {
            SwitchCamera(0);
            flag = 1;
        }
        else
        {
            SwitchCamera(1);
            flag = 0;
        }
    }
}
