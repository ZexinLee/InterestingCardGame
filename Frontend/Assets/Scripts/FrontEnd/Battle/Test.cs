using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class Test : MonoBehaviour
{
    ///Image image2;

    public Image image;
    public string loadpath = "D:/timg.jpg"; //IO方式加载的路径
    public string picpathWWW = "test.jpg"; //WWW的加载方式路径


    // Use this for initialization
    public void Start()
    {
        image = GetComponent<Image>();
        //IO方法加载速度快
        LoadByIO();
        //WWW 加载速度慢


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
}

