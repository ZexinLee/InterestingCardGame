using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowChat : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Chat;
    public void show()
    {
        Animation a = Chat.GetComponent<Animation>();
        if (a.isPlaying == false)
        {
            a.Play("ShowChat");
        }
    }
}
