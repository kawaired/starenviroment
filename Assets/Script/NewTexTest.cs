using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewTexTest : MonoBehaviour
{
    public Texture2D inputtex;
    //private Texture2D tex;
    // Start is called before the first frame update
    void Start()
    {
        Vector2 texsize = inputtex.texelSize;
        Debug.Log(inputtex.GetPixel(100, 150));
        //tex = new Texture2D(512, 512);
        //tex.set
    }
}
