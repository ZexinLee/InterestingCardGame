using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	// Use this for initialization
	void Change (int SceneNumber) {

        Application.LoadLevel(SceneNumber);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
