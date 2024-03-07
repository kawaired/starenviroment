using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct StarData
{
    public Texture startex;
    public float range;
}

public class StartMeshCreater : MonoBehaviour
{
    public Shader starshader;

    [SerializeField]
    public List<StarData> stardatas = new List<StarData>();

    private GameObject starsobj;

    private Quaternion rotatespeed = new Quaternion();

    private float timecountor = 0;

    void Start()
    {
        starsobj = new GameObject();
        starsobj.name = "starobj";
        CreateStars(starsobj);

        timecountor = 0;
        rotatespeed = Quaternion.AngleAxis(0.5f*0.25f, Vector3.forward);    
    }

    private void Update()
    {
        timecountor = timecountor + Time.deltaTime;
        if (timecountor > 0.25f)
        {
            timecountor = 0;
            starsobj.transform.rotation = starsobj.transform.rotation * rotatespeed;
        }
    }


    private void CreateStars(GameObject centerobj)
    {
        centerobj.transform.SetParent(transform);
        centerobj.transform.localPosition = Vector3.zero;

        for (int i = 0; i < stardatas.Count; i++)
        {
            Material tempmat = new Material(starshader);
            tempmat.SetTexture("_MainTex", stardatas[i].startex);
            tempmat.SetInt("_StarIndex", (int)UnityEngine.Random.Range(0, 4));
            tempmat.SetFloat("_StarSwitchTime", UnityEngine.Random.Range(0, 3));
            tempmat.SetFloat("_LightSwitchFac", UnityEngine.Random.Range(0.2f, 0.5f));
            CreateTypeStarMesh(stardatas[i].range, UnityEngine.Random.Range(160, 240), new Vector2(UnityEngine.Random.Range(72, 108), UnityEngine.Random.Range(54, 90)), centerobj.transform, tempmat);
        }
    }
    private void CreateTypeStarMesh(float range, int starcount, Vector2 totalrange,Transform parent, Material material)
    {
        GameObject starobj = new GameObject();
        starobj.transform.SetParent(parent);
        starobj.transform.localPosition = new Vector3(0,0,0.02f);
        MeshRenderer starrender = starobj.AddComponent<MeshRenderer>();
        starrender.material = material;
        MeshFilter filter = starobj.AddComponent<MeshFilter>();
        Mesh starmesh = new Mesh();
        filter.mesh = starmesh;
        List<Vector3> meshptgroup = new List<Vector3>();
        List<Vector2> UVgroup = new List<Vector2>();
        List<int> Triangles = new List<int>();

        Vector2 tempstarpos = new Vector2();
        int curidx = 0;
        for (int i = 0; i < starcount; i++)
        {
            tempstarpos = new Vector2(starobj.transform.position.x + UnityEngine.Random.Range(-totalrange.x, totalrange.x), starobj.transform.position.y + UnityEngine.Random.Range(-totalrange.y, totalrange.y));
            meshptgroup.Add(new Vector3(tempstarpos.x - range, tempstarpos.y - range, starobj.transform.position.z));
            meshptgroup.Add(new Vector3(tempstarpos.x - range, tempstarpos.y + range, starobj.transform.position.z));
            meshptgroup.Add(new Vector3(tempstarpos.x + range, tempstarpos.y - range, starobj.transform.position.z));
            meshptgroup.Add(new Vector3(tempstarpos.x + range, tempstarpos.y + range, starobj.transform.position.z));
            UVgroup.Add(new Vector2(0, 0));
            UVgroup.Add(new Vector2(0, 1));
            UVgroup.Add(new Vector2(1, 0));
            UVgroup.Add(new Vector2(1, 1));
            Triangles.AddRange(new List<int> { curidx, curidx + 1, curidx + 3 });
            Triangles.AddRange(new List<int> { curidx, curidx + 3, curidx + 2 });
            curidx += 4;
        }
        starmesh.vertices = meshptgroup.ToArray();
        starmesh.triangles = Triangles.ToArray();
        starmesh.uv = UVgroup.ToArray();
    }
}
