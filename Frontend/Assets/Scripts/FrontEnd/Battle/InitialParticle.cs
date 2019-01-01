using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialParticle : MonoBehaviour {
    public GameObject[] Myp;
    public GameObject[] Yourp;
    // Use this for initialization

    void Start () {
        int i = 0;
        Global.GetInstance().Myparticles = Myp;
        Global.GetInstance().Yourparticles = Yourp;
        /*  for(;i< Global.GetInstance().particles.Length; i++)
          {
              Global.GetInstance().particles[i].SetActive(false);
          }*/

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
