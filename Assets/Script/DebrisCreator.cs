using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebrisFieldsData
{
    public int itemcount;
    public float movespeed;
    public float rotatespeed;
    public Vector2 movevector;
    public GameObject fieldobj;
    public float rangescale;
    public List<MeshRenderer> debrisrenderers;
}

public class DebrisCreator : MonoBehaviour
{
    public Shader debrisshader;
    public List<Texture2D> debristexlist;
    public int debrisfieldnum;

    private GameObject debrisfieldobj;
    private List<DebrisFieldsData> fieldsdatas = new List<DebrisFieldsData>();
    private Vector2 fieldsize = new Vector2(84.5f, 67);
    private Vector2 skysize = new Vector2(60, 45);

    void Start()
    {
        debrisfieldobj = new GameObject();
        debrisfieldobj.name = "debrisfield";
        CreateDebrisField(debrisfieldobj);
        for(int i=0;i< fieldsdatas.Count;i++)
        {
            MoveDebrisField(fieldsdatas[i], Random.Range(0, 4000));
        }
    }

    void Update()
    {
        for(int i=0;i< fieldsdatas.Count;i++)
        {
            MoveDebrisField(fieldsdatas[i]);
            RotateDebrisField(fieldsdatas[i]);
        }
    }

    private void MoveDebrisField(DebrisFieldsData data,float step=1)
    {
        data.fieldobj.transform.localPosition = data.fieldobj.transform.localPosition + new Vector3(data.movevector.x, data.movevector.y, 0) * data.movespeed * Time.deltaTime * step;
        if(Mathf.Abs(data.fieldobj.transform.localPosition.x)>(skysize.x+data.rangescale* fieldsize.x) ||Mathf.Abs(data.fieldobj.transform.localPosition.y)> (skysize.y + data.rangescale * fieldsize.y))
        {
            data.fieldobj.transform.rotation = Quaternion.identity;
            RandomDebrisFields(data);
            DebrisRandomMat(data);
        }
    }

    private void DebrisRandomMat(DebrisFieldsData data)
    {
        for(int i=0;i<data.debrisrenderers.Count;i++)
        {
            Material debrismat = new Material(debrisshader);
            debrismat.SetTexture("_DebrisTex", debristexlist[Random.Range(0, debristexlist.Count)]);
            data.debrisrenderers[i].material = debrismat;
        }
    }

    private void RotateDebrisField(DebrisFieldsData data)
    {
        data.fieldobj.transform.rotation = data.fieldobj.transform.rotation * Quaternion.AngleAxis(data.rotatespeed * Time.deltaTime, Vector3.back);
    }

    private void CreateDebrisField(GameObject centerobj)
    {
        centerobj.transform.SetParent(transform);
        centerobj.transform.localPosition = new Vector3(0, 0, -0.02f);

        //Texture2D debristex = null;
        //List<Texture2D> temptexlist = new List<Texture2D>();

        //for (int i = 0; i < debrisfieldnum; i++)
        //{
        //    GameObject singlefieldobj = new GameObject();
        //    singlefieldobj.name = "debris_" + i.ToString();
        //    singlefieldobj.transform.SetParent(centerobj.transform);
        //    singlefieldobj.transform.localPosition = Vector3.zero;
        //    DebrisFieldsData data = new DebrisFieldsData
        //    {
        //        fieldobj = singlefieldobj,
        //        debrisrenderers = new List<MeshRenderer>()
        //    };
        //    RandomDebrisFields(data);
        //    fieldsdatas.Add(data);
        //    for (int j = 0; j < data.itemcount; j++)
        //    {
        //        temptexlist.Add(debristexlist[Random.Range(0, debristexlist.Count)]);
        //        //DebrisItemCreate(data, singlefieldobj.transform);
        //    }
        //    debristex = TextureMerge(temptexlist);
        //    MeshRenderer renderer = singlefieldobj.AddComponent<MeshRenderer>();
        //    MeshFilter filter = singlefieldobj.AddComponent<MeshFilter>();
        //    Mesh mesh = new Mesh();
        //    filter.mesh = mesh;
        //    mesh.vertices = new Vector3[]
        //    {
        //        new Vector3(-fieldsize.x * data.rangescale, -fieldsize.y * data.rangescale, singlefieldobj.transform.position.z),
        //        new Vector3(-fieldsize.x * data.rangescale, fieldsize.y * data.rangescale, singlefieldobj.transform.position.z),
        //        new Vector3(fieldsize.x * data.rangescale, -fieldsize.y * data.rangescale, singlefieldobj.transform.position.z),
        //        new Vector3(fieldsize.x * data.rangescale, fieldsize.y * data.rangescale, singlefieldobj.transform.position.z),
        //    };
        //    mesh.uv = new Vector2[]
        //    {
        //        Vector2.zero,
        //        Vector2.up,
        //        Vector2.right,
        //        Vector2.one
        //    };
        //    mesh.triangles = new int[] { 0, 1, 3, 0, 3, 2 };
        //    Material debrismat = new Material(debrisshader);
        //    debrismat.SetTexture("_DebrisTex", debristex);
        //    renderer.material = debrismat;
        //    //DebrisRandomMat(data);
        //}

        //for (int i = 0; i < debrisfieldnum; i++)
        //{
        //    temptexlist.Add(debristexlist[Random.Range(0, debristexlist.Count)]);
        //}

        //debristex = TextureMerge(temptexlist);
        for (int i = 0; i < debrisfieldnum; i++)
        {
            GameObject singlefieldobj = new GameObject();
            singlefieldobj.name = "debris_" + i.ToString();
            singlefieldobj.transform.SetParent(centerobj.transform);
            singlefieldobj.transform.localPosition = Vector3.zero;
            DebrisFieldsData data = new DebrisFieldsData
            {
                fieldobj = singlefieldobj,
                debrisrenderers = new List<MeshRenderer>()
            };
            RandomDebrisFields(data);
            fieldsdatas.Add(data);
            for (int j = 0; j < data.itemcount; j++)
            {
                DebrisItemCreate(data, singlefieldobj.transform);
            }
            DebrisRandomMat(data);
        }
    }

    private void RandomDebrisFields(DebrisFieldsData data)
    {
        data.itemcount = 2;
        data.movespeed = Random.Range(2f, 4f);
        data.rotatespeed = Random.Range(-4f, 4f);
        data.rangescale = Random.Range(1.5f, 3f);
        float facx = Random.Range(-1f, 1f);
        float facy = Mathf.Sqrt(1 - Mathf.Pow(facx, 2)) * (Random.Range(-1f, 1f) >= 0 ? 1 : -1);
        Vector2 dir = new Vector2(facx, facy);
        data.fieldobj.transform.localPosition = ComputeStartPos(skysize, fieldsize * data.rangescale, dir);
        data.movevector = RandomMoveVector(-dir);
    }

    private Vector2 ComputeStartPos(Vector2 centersize,Vector2 selfsize,Vector2 dir)
    {
        Vector2 startpos = Vector2.zero;
        Vector2 normalcenter = centersize.normalized;
        if(Mathf.Abs(dir.y)>Mathf.Abs(normalcenter.y))
        {
            startpos = new Vector2(dir.x * (centersize.y + selfsize.y-10) / Mathf.Abs(dir.y), (centersize.y + selfsize.y-10) * (dir.y >= 0 ? 1 : (-1)));
        }
        else
        {
            startpos = new Vector2((centersize.x + selfsize.x-10) * (dir.x >= 0 ? 1 : (-1)), dir.y * (centersize.x + selfsize.y-10) / Mathf.Abs(dir.x));
        }
        return startpos;
    }

    private Vector2 RandomMoveVector(Vector2 dir)
    {
        Vector2 normaldir = dir.normalized;
        float angle = Random.Range(-15 * Mathf.PI / 180, 15 * Mathf.PI / 180);
        return new Vector2(normaldir.x * Mathf.Cos(angle) - normaldir.y * Mathf.Sin(angle), normaldir.x * Mathf.Sin(angle) + normaldir.y * Mathf.Cos(angle));
    }

    private Texture2D TextureMerge(List<Texture2D> debristexlist)
    {
        Texture2D mergedtex = debristexlist[0];
        int texwidth = debristexlist[0].width;
        int texheight = debristexlist[0].height;
        Texture2D tex = new Texture2D(texwidth, texheight);
        for (int i = 0; i < texwidth; i++)
        {
            for (int j = 0; j < texheight; j++)
            {
                Vector4 colorvec = Vector4.zero;
                for(int k=0;k<debristexlist.Count;k++)
                {
                    Vector4 pixelcolor = debristexlist[k].GetPixel(i, j);
                    colorvec += new Vector4(pixelcolor.x * pixelcolor.w, pixelcolor.y * pixelcolor.w, pixelcolor.z * pixelcolor.w, pixelcolor.w) / debristexlist.Count;
                }
                tex.SetPixel(i, j, colorvec * (debristexlist.Count));
                //tex.SetPixel(i, j, texone.GetPixel(i, j) + textwo.GetPixel(i, j));
            }
        }
        tex.Apply();
        return tex;


        //for(int i=0;i< debristexlist.Count;i++)
        //{
        //    if(mergedtex==null)
        //    {
        //        mergedtex = debristexlist[i];
        //    }
        //    else
        //    {
        //        mergedtex = AddTex(mergedtex, debristexlist[i]);
        //    }
        //}
        //return mergedtex;
    }

    //private Texture2D AddTex(Texture2D texone,Texture2D textwo)
    //{
    //    int texwidth = texone.width;
    //    int texheight = texone.height;
    //    Texture2D tex = new Texture2D(texwidth, texheight);
    //    for (int i = 0; i < texwidth; i++)
    //    {
    //        for (int j = 0; j < texheight; j++)
    //        {
    //            tex.SetPixel(i, j, texone.GetPixel(i,j)+textwo.GetPixel(i,j));
    //        }
    //    }
    //    tex.Apply();
    //    return tex;
    //}

    private void DebrisItemCreate(DebrisFieldsData data, Transform parent)
    {
        GameObject debrisitem = new GameObject();
        debrisitem.transform.SetParent(parent);
        debrisitem.transform.localPosition = Vector3.zero;
        //Debug.Log("add meshrenderer");
        MeshRenderer renderer = debrisitem.AddComponent<MeshRenderer>();
        data.debrisrenderers.Add(renderer);
        MeshFilter filter = debrisitem.AddComponent<MeshFilter>();
        Mesh mesh = new Mesh();
        filter.mesh = mesh;
        List<Vector3> meshgroup = new List<Vector3>();
        List<Vector2> uvgroup = new List<Vector2>();
        List<int> triangles = new List<int>();
        meshgroup.Add(new Vector3(-fieldsize.x * data.rangescale, -fieldsize.y * data.rangescale, debrisitem.transform.position.z));
        meshgroup.Add(new Vector3(-fieldsize.x * data.rangescale, fieldsize.y * data.rangescale, debrisitem.transform.position.z));
        meshgroup.Add(new Vector3(fieldsize.x * data.rangescale, fieldsize.y * data.rangescale, debrisitem.transform.position.z));
        meshgroup.Add(new Vector3(fieldsize.x * data.rangescale, -fieldsize.y * data.rangescale, debrisitem.transform.position.z));
        uvgroup.Add(new Vector2(0, 0));
        uvgroup.Add(new Vector2(0, 1));
        uvgroup.Add(new Vector2(1, 1));
        uvgroup.Add(new Vector2(1, 0));
        triangles.AddRange(new List<int> { 0, 1, 2 });
        triangles.AddRange(new List<int> { 0, 2, 3 });
        mesh.vertices = meshgroup.ToArray();
        mesh.uv = uvgroup.ToArray();
        mesh.triangles = triangles.ToArray();
    }
}
