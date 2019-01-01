using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideChat : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Chat;
    public void hide()
    {
        Animation a = Chat.GetComponent<Animation>();
        if (a.isPlaying == false)
        {
            a.Play("HideChat", PlayMode.StopAll);
        }
    }
}
