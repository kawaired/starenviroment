using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlanetData
{
    public string masktype;
    public string planetname;
}

public enum Planet
{
    alien=0,
    arctic=1,
    barren=2,
    desert=3,
    forest=4,
    garden=5,
    jungle=6,
    magma=7,
    midnight=8,
    moon=9,
    ocean=10,
    savannah=11,
    snow=12,
    tentacles=13,
    toxic=14,
    tundra=15,
    volcanic=16,
}

public enum LandMask
{
    arid=0,
    ocean=1,
    temperate,
}
//[Serializable]
//public class planetD

[Serializable]
public class PlanetClass
{
    public List<Texture2D> planetlist;
}

[Serializable]
public class LandMaskClass
{
    public List<Texture2D> Landmasklist;
}

public class DrawPlanet : MonoBehaviour
{
    public int planetcount;
    public Shader planetshader;
    public Shader bgshader;
    public Texture2D liquidtex;
    public Texture2D shadowtex;
    public List<Texture2D> liquidtexlist = new List<Texture2D>();
    public List<Texture2D> aridmasklist = new List<Texture2D>();
    public List<Texture2D> oceanmasklist = new List<Texture2D>();
    public List<Texture2D> temperatmasklist = new List<Texture2D>();
    public Texture2D gasbase;
    public Texture2D gascloud1;
    public Texture2D gascloud2;
    public List<Texture2D> gasmasklist = new List<Texture2D>();
    public List<PlanetClass> planetclasslist = new List<PlanetClass>();
    public List<LandMaskClass> landmasklist = new List<LandMaskClass>();
    //public List<PlanetListData> planetdatalist = new List<PlanetListData>();
    public bool havegas;

    private List<GameObject> planetobjs = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < planetdatalist.Count; i++)
        {
            GameObject planetobj = CreatePlanet(i);

            planetobjs.Add(planetobj);
        }
    }

    public GameObject CreatePlanet(int idx)
    {
        GameObject planetobj = new GameObject();
        planetobj.name = "sphereplanet";
        planetobj.transform.SetParent(transform);
        planetobj.transform.localPosition = new Vector3(UnityEngine.Random.Range(-60f, 60f), UnityEngine.Random.Range(-25f, 40f), -0.1f * (idx + 1));
        float planetsize = UnityEngine.Random.Range(2f, 15f);

        if (havegas)
        {
            CreateLayerMesh(planetobj.transform, planetsize, 0).material = CreateBgMat(gasbase);
            CreateLayerMesh(planetobj.transform, planetsize, 1).material = CreateLayerMat(gascloud1, gasmasklist[UnityEngine.Random.Range(0, gasmasklist.Count)]);
            CreateLayerMesh(planetobj.transform, planetsize, 2).material = CreateLayerMat(gascloud2, gasmasklist[UnityEngine.Random.Range(0, gasmasklist.Count)]);
        }
        else
        {
            CreateLayerMesh(planetobj.transform, planetsize, 0).material = CreateBgMat(liquidtexlist[UnityEngine.Random.Range(0, liquidtexlist.Count)]);
            for (int i = 0; i < planetdatalist[idx].planetlist.Count; i++)
            {
                CreateLayerMesh(planetobj.transform, planetsize, i + 1).material = CreateLayerMat(planetdatalist[idx].planetlist[i]);
            }
        }
        return planetobj;
    }

    public MeshRenderer CreateLayerMesh(Transform parent,float size ,int layeridx)
    {
        GameObject meshobj = new GameObject();
        meshobj.transform.SetParent(parent);
        meshobj.transform.localPosition = new Vector3(0, 0, -layeridx * 0.01f);
        MeshRenderer renderer = meshobj.AddComponent<MeshRenderer>();
        MeshFilter filter = meshobj.AddComponent<MeshFilter>();
        Mesh mesh = new Mesh();
        filter.mesh = mesh;
        mesh.vertices = new Vector3[] { new Vector3(-size, -size, 0), new Vector3(-size, size, 0), new Vector3(size, size, 0), new Vector3(size, -size, 0) };
        mesh.uv = new Vector2[] { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0) };
        mesh.triangles = new int[] { 0, 1, 2, 0, 2, 3 };
        return renderer;
    }

    private Material CreateLayerMat(Texture2D maintex,Texture2D masktex)
    {
        Material layermat = new Material(planetshader);
        layermat.SetTexture("_MainTex", maintex);
        layermat.SetTexture("_MaskTex", masktex);
        layermat.SetTexture("_ShadowTex", shadowtex);
        return layermat;
    }

    private Material CreateBgMat(Texture maintex)
    {
        Material bgmat = new Material(bgshader);
        bgmat.SetTexture("_MainTex", maintex);
        bgmat.SetTexture("_ShadowTex", shadowtex);
        return bgmat;
    }

    //private 
}
