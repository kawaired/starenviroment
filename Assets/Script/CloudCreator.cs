using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudData
{
    public float angle;
    public float moveradius;
    public GameObject cloud;
}

public class CloudCreator : MonoBehaviour
{
    public Shader cloudshader;
    public List<Texture2D> cloudtexlist;
    public int cloudcount;
    public float anglespeed;
    private readonly float radius = 183.6f;
    private List<CloudData> cloudlist;
    private float timecount=0;
    void Start()
    {
        cloudlist = new List<CloudData>();
        GameObject cloudfield = new GameObject();
        cloudfield.name = "cloudfield";
        cloudfield.transform.SetParent(transform);
        cloudfield.transform.localPosition = new Vector3(0, -radius + 20.2f - 48.5f, -1.5f);
        for(int i=0;i<cloudcount;i++)
        {
            cloudlist.Add(CreateCloud(cloudfield.transform, Random.Range(0f, 360f), Random.Range(0.95f, 1f)));
        }
    }

    void Update()
    {
        timecount += Time.deltaTime;
        if (timecount > 0.5f)
        {
            for (int i = 0; i < cloudlist.Count; i++)
            {
                CloudData tempdata = cloudlist[i];
                tempdata.angle = tempdata.angle + Time.deltaTime * anglespeed;
                tempdata.angle = (tempdata.angle >= 360) ? (tempdata.angle - 360) : tempdata.angle;
                tempdata.cloud.transform.localPosition = new Vector3(tempdata.moveradius * Mathf.Cos(tempdata.angle * Mathf.PI / 180), tempdata.moveradius * Mathf.Sin(tempdata.angle * Mathf.PI / 180), -1.5f);
            }
            timecount = 0;
        }
    }

    private CloudData CreateCloud(Transform parent, float angle,float rscale)
    {
        CloudData data = new CloudData();
        data.angle = angle;
        data.moveradius = rscale * radius;
        GameObject cloudobj = new GameObject();
        cloudobj.transform.SetParent(parent);
        cloudobj.transform.localPosition = new Vector3(data.moveradius * Mathf.Cos(angle*Mathf.PI/180), data.moveradius * Mathf.Sin(angle * Mathf.PI / 180), -1.5f);
        Texture2D cloudtex = cloudtexlist[Random.Range(0, cloudtexlist.Count)];
        float sizescale = Random.Range(0.25f, 0.3f);
        Vector2 pixelsize = new Vector2(sizescale*cloudtex.width, sizescale*cloudtex.height);
        
        MeshRenderer renderer = cloudobj.AddComponent<MeshRenderer>();
        MeshFilter filter = cloudobj.AddComponent<MeshFilter>();
        Mesh mesh = new Mesh();
        filter.mesh = mesh;
        mesh.vertices = new Vector3[] { new Vector3(-pixelsize.x, 0, 0) * sizescale, new Vector3(-pixelsize.x, 2 * pixelsize.y, 0) * sizescale, new Vector3(pixelsize.x, 0, 0) * sizescale, new Vector3(pixelsize.x, 2 * pixelsize.y, 0) * sizescale };
        mesh.uv = new Vector2[] { Vector2.zero, Vector2.up, Vector2.right, Vector2.one };
        mesh.triangles = new int[] { 0, 1, 3, 0, 3, 2 };
        Material cloudmat = new Material(cloudshader);
        
        cloudmat.SetTexture("_MainTex", cloudtex);
        renderer.material = cloudmat;
        data.cloud = cloudobj;
        return data;
    }
}
