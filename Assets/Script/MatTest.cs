using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatTest : MonoBehaviour
{
    Material testmat;
    
    void Start()
    {
        testmat = GetComponent<MeshRenderer>().material;
        Texture2D testpic = new Texture2D(512, 512);
        for(int i=0;i<512;i++)
        {
            for(int j=0;j<512;j++)
            {
                testpic.SetPixel(i, j, new Color(1f, 0.5f, 0.5f, 1));
            }
        }
        testpic.Apply();
        testmat.SetTexture("_MainTex", testpic);
        
        //testmat.SetTextureArray("_TexArray",texs);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
