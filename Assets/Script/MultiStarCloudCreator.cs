using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public struct StarCloudData
{
    public Vector2 sizefac;
    public GameObject cloudobj;
    public Vector2 centerpos;
    public int startypenum;
}

public class MultiStarCloudCreator : MonoBehaviour
{
    public Material singlestarmat;
    public Shader starshader;
    public int starcloudcount;

    private List<StarCloudData> starcloudlist = new List<StarCloudData>();

    [SerializeField]
    public List<StarData> stardatas = new List<StarData>();
    void Start()
    {
        CreateMultiStarCloud();
    }

    private void CreateMultiStarCloud()
    {
        for (int i = 0; i < starcloudcount; i++)
        {
            starcloudlist.Add(CreateStarCloud());
        }

        for (int i = 0; i < starcloudlist.Count; i++)
        {
            StarCloudPrepar(starcloudlist[i]);
        }
    }

    private StarCloudData CreateStarCloud()
    {
        GameObject starcloudobj = new GameObject();
        starcloudobj.transform.position = new Vector3(UnityEngine.Random.Range(-200, 200), UnityEngine.Random.Range(-150, 150), 0);

        starcloudobj.transform.SetParent(transform);
        StarCloudData starcloud;
        starcloud.cloudobj = starcloudobj;
        starcloud.sizefac = new Vector2(UnityEngine.Random.Range(60, 160), UnityEngine.Random.Range(40, 120));
        starcloud.startypenum = (int)UnityEngine.Random.Range(1, 11);
        starcloud.centerpos = new Vector2(starcloudobj.transform.position.x, starcloudobj.transform.position.y);
        return starcloud;
    }

    private void StarCloudPrepar(StarCloudData data)
    {
        for (int i = 0; i < data.startypenum; i++)
        {
            int staridx = (int)UnityEngine.Random.Range(0, stardatas.Count);
            Material tempmat = new Material(starshader);
            tempmat.SetTexture("_MainTex", stardatas[staridx].startex);
            tempmat.SetInt("_StarIndex", (int)UnityEngine.Random.Range(0, 4));
            tempmat.SetFloat("_StarSwitchTime", UnityEngine.Random.Range(0, 3));
            tempmat.SetFloat("_LightSwitchFac", UnityEngine.Random.Range(0.5f, 1));
            CreateTypeStarCloudMesh(stardatas[staridx].range, UnityEngine.Random.Range(20, 50), data, tempmat);
        }
    }

    private void CreateTypeStarCloudMesh(float range, int starcount, StarCloudData data, Material material)
    {
        GameObject starobj = new GameObject();
        starobj.transform.SetParent(data.cloudobj.transform);
        starobj.transform.localPosition = Vector3.zero;
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
            tempstarpos = new Vector2(UnityEngine.Random.Range(-data.sizefac.x, data.sizefac.x), UnityEngine.Random.Range(-data.sizefac.y, data.sizefac.y));
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
