using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tuichu : MonoBehaviour
{

    // Start is called before the first frame update
    public void click1()
    {
        Global.GetInstance().cavas2.SetActive(false);
        SceneManager.LoadScene(4);

    }
    public void click2()
    {
        Global.GetInstance().cavas3.SetActive(false);
        SceneManager.LoadScene(4);

    }

}
