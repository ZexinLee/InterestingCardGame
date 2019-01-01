using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Howtowin : MonoBehaviour
{
    public GameObject cavas;
    public GameObject camera;
    public GameObject cavas2;
    public GameObject cavas3;
    // Start is called before the first frame update
    void Start()
    {
        Global.GetInstance().cavas = cavas;
        Global.GetInstance().Camera = camera;
        Global.GetInstance().cavas2 = cavas2;
        Global.GetInstance().cavas3 = cavas3;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
