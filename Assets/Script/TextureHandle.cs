using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureHandle : MonoBehaviour
{
    List<Vector4> stardrawdatalist = new List<Vector4>();
    List<float> uvidxlist = new List<float>();
    

    private int pixelsizeH = 1280;
    private int pixelsizeV = 720;
    Vector4[] randomstardatas =new Vector4[2000];
    private int startnum = 200;
    void Start()
    {
        for(int i=0;i<startnum;i++)
        {
            Vector4 tempdata = CalculateRandomData();
            randomstardatas[i] = tempdata;
        }

        Shader.SetGlobalFloat("_PixelSizeH", pixelsizeH);
        Shader.SetGlobalFloat("_PixelSizeV", pixelsizeV);
        Shader.SetGlobalVectorArray("_RandomStarDatas", randomstardatas);
        Shader.SetGlobalFloat("_StarNum", startnum);
    }

    Vector4 CalculateRandomData()
    {
        Vector4 randomdata = new Vector4(0, 0, 0, 0);
        randomdata.x = Random.Range(0, pixelsizeH);
        randomdata.y = Random.Range(0, pixelsizeV);
        randomdata.z = Random.Range(0, 4);
        randomdata.w = Random.Range(0, 3);
        return randomdata;
    }
}
