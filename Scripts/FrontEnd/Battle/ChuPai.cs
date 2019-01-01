using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;


public class ChuPai : MonoBehaviour {
    public Image image;
    public Button b1;
    public string loadpath = "D:/timg.jpg"; //IO方式加载的路径
    public string picpathWWW = "test.jpg"; //WWW的加载方式路径
    public GameObject g1;
    public Image[] images;
    public int[] flags;



    public void getimage()
    {
        //Image image;
        int i = 0;
        for (i = 0; i < images.Length; i++)
        {
            if (flags[i] == 0)
            {
                image = images[i];
                flags[i] = 1;
                image = GetComponent<Image>();
                b1 = GetComponent<Button>();
                //IO方法加载速度快
                LoadByIO();
                break;
            }

        }
    }


   public void LoadByIO()
    {
        double startTime = (double)Time.time;
        //创建文件流
        FileStream fileStream = new FileStream(loadpath, FileMode.Open, FileAccess.Read);
        fileStream.Seek(0, SeekOrigin.Begin);
        //创建文件长度的缓冲区
        byte[] bytes = new byte[fileStream.Length];
        //读取文件
        fileStream.Read(bytes, 0, (int)fileStream.Length);
        //释放文件读取liu
        fileStream.Close();
        fileStream.Dispose();
        fileStream = null;

        //创建Texture
        int width = 15;
        int height = 23;
        Texture2D texture2D = new Texture2D(width, height);
        texture2D.LoadImage(bytes);

        Sprite sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height),
            new Vector2(0.5f, 0.5f));
        image.sprite = sprite;
        double time = (double)Time.time - startTime;
        Debug.Log("IO加载用时：" + time);
    }


  


    public void chupai()
    {
        g1.SetActive(false);
    }

}
