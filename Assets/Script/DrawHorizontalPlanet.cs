using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DrawHorizontalPlanet : MonoBehaviour
{
    public List<Texture2D> tempermask_lefts = new List<Texture2D>();
    public List<Texture2D> tempermask_rights = new List<Texture2D>();
    public List<Texture2D> oceanmask_lefts = new List<Texture2D>();
    public List<Texture2D> oceanmask_rights = new List<Texture2D>();

    public Shader hplanetshader;
    public Shader atmosphereshader;

    public bool isoceanmask;
    public int maskcount;
    public Texture2D maintexleft;
    public Texture2D maintexright;
    public Texture2D liquidtexleft;
    public Texture2D liquidtexright;
    public Texture2D atmosphereleft;
    public Texture2D atmosphereright;

    private List<int> maskidxlist = new List<int>();
    private Texture2D leftmask;
    private Texture2D rightmask;
    private Vector2 atmospheresize = new Vector2(88.2f, 20.2f);
    private Vector2 HPlanetSize = new Vector2(78.9f, 16.5f);
    // Start is called before the first frame update
    void Start()
    {
        GameObject horizonplanetobj = new GameObject();
        horizonplanetobj.name = "horizonplanet";
        horizonplanetobj.transform.SetParent(transform);
        horizonplanetobj.transform.localPosition = new Vector3(0, -48.5f, -1f);
        for(int i=0;i< maskcount;i++)
        {
            maskidxlist.Add(Random.Range(0, tempermask_lefts.Count));
        }
        if(isoceanmask)
        {
            leftmask = MixMask(maskcount, oceanmask_lefts,maskidxlist);
            rightmask = MixMask(maskcount, oceanmask_rights,maskidxlist);
            //Debug.Log(leftmask.GetPixel(400, 80));
            //Debug.Log(leftmask.GetPixel(350, 40));
        }
        else
        {
            leftmask = MixMask(maskcount, tempermask_lefts,maskidxlist);
            rightmask = MixMask(maskcount, tempermask_rights,maskidxlist);
        }
        DrawAtmosphere(horizonplanetobj);
        DrawHorizon(leftmask, rightmask, horizonplanetobj);
    }

    private Texture2D MixMask(int masknum,List<Texture2D> texarray,List<int> idxlist)
    {
        Texture2D mixedmask = null;
        for (int i=0;i<masknum;i++)
        {
            int maskidx = idxlist[i];
            //Debug.Log(maskidx);
            if(mixedmask == null)
            {
                //Debug.Log(texarray[maskidx].texelSize);
                //Debug.Log(maskidx);
                mixedmask = texarray[maskidx];
                //Debug.Log(mixedmask.GetPixel(100, 100));
                //Debug.Log();
            }
            else
            {
                mixedmask = MixTex(mixedmask, texarray[maskidx]);
            }
        }
        //Debug.Log(mixedmask == null);
        return mixedmask;
    }

    private Texture2D MixTex(Texture2D texone, Texture2D textwo)
    {
        int texwidth = texone.width;
        int texheight = texone.height;
        //Debug.Log(texsize);
        Texture2D tex = new Texture2D(texwidth, texheight);
        //Debug.Log(texone.GetPixel(2, 3).a);
        for (int i=0;i< texwidth; i++)
        {
            for(int j=0;j< texheight; j++)
            {

                //Debug.Log( Mathf.Max(texone.GetPixel(i, j).a, textwo.GetPixel(i, j).a));
                tex.SetPixel(i, j, new Color(0, 0, 0, Mathf.Max(texone.GetPixel(i, j).a, textwo.GetPixel(i, j).a)));
                //tex.SetPixel(i, j, new Color(1,1,1,1));
            }
        }
        tex.Apply();
        return tex;
    }

    private void DrawHorizon(Texture2D maskleft, Texture2D maskright, GameObject planetobj)
    {
        GameObject leftobj = new GameObject();
        GameObject rightobj = new GameObject();
        leftobj.transform.SetParent(planetobj.transform);
        rightobj.transform.SetParent(planetobj.transform);
        leftobj.transform.localPosition = Vector3.zero;
        rightobj.transform.localPosition = Vector3.zero;

        MeshRenderer leftrenderer = leftobj.AddComponent<MeshRenderer>();
        MeshFilter leftfilter = leftobj.AddComponent<MeshFilter>();
        Mesh leftmesh = new Mesh();
        leftfilter.mesh = leftmesh;
        leftmesh.vertices = new Vector3[] { new Vector3(-HPlanetSize.x, 0, 0), new Vector3(-HPlanetSize.x, HPlanetSize.y, 0), Vector3.zero, new Vector3(0, HPlanetSize.y, 0) };
        leftmesh.uv = new Vector2[] { Vector2.zero, Vector2.up, Vector2.right, Vector2.one };
        leftmesh.triangles = new int[] { 0, 1, 3, 0, 3, 2 };
        Material leftmat = new Material(hplanetshader);
        leftmat.SetTexture("_MainTex", maintexleft);
        leftmat.SetTexture("_LiquidTex", liquidtexleft);
        leftmat.SetTexture("_MaskTex", maskleft);
        leftrenderer.material = leftmat;

        MeshRenderer rightrenderer = rightobj.AddComponent<MeshRenderer>();
        MeshFilter rightfilter = rightobj.AddComponent<MeshFilter>();
        Mesh rightmesh = new Mesh();
        rightfilter.mesh = rightmesh;
        rightmesh.vertices = new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, HPlanetSize.y, 0), new Vector3(HPlanetSize.x, 0, 0), new Vector3(HPlanetSize.x, HPlanetSize.y, 0) };
        rightmesh.uv = new Vector2[] { Vector2.zero, Vector2.up, Vector2.right, Vector2.one };
        rightmesh.triangles = new int[] { 0, 1, 3, 0, 3, 2 };
        Material rightmat = new Material(hplanetshader);
        rightmat.SetTexture("_MainTex",maintexright);
        rightmat.SetTexture("_LiquidTex", liquidtexright);
        rightmat.SetTexture("_MaskTex", maskright);
        rightrenderer.material = rightmat;
    }

    private void DrawAtmosphere( GameObject planetobj)
    {
        GameObject leftobj = new GameObject();
        GameObject rightobj = new GameObject();
        leftobj.transform.SetParent(planetobj.transform);
        rightobj.transform.SetParent(planetobj.transform);
        leftobj.transform.localPosition = Vector3.zero;
        rightobj.transform.localPosition = Vector3.zero;

        MeshRenderer leftrenderer = leftobj.AddComponent<MeshRenderer>();
        MeshFilter leftfilter = leftobj.AddComponent<MeshFilter>();
        Mesh leftmesh = new Mesh();
        leftfilter.mesh = leftmesh;
        leftmesh.vertices = new Vector3[] { new Vector3(-atmospheresize.x, 0, 0.01f), new Vector3(-atmospheresize.x, atmospheresize.y, 0.01f), new Vector3(0, 0, 0.01f), new Vector3(0, atmospheresize.y, 0.01f) };
        leftmesh.uv = new Vector2[] { Vector2.zero, Vector2.up, Vector2.right, Vector2.one };
        leftmesh.triangles = new int[] { 0, 1, 3, 0, 3, 2 };
        Material leftmat = new Material(atmosphereshader);
        leftmat.SetTexture("_MainTex", atmosphereleft);
        leftrenderer.material = leftmat;

        MeshRenderer rightrenderer = rightobj.AddComponent<MeshRenderer>();
        MeshFilter rightfilter = rightobj.AddComponent<MeshFilter>();
        Mesh rightmesh = new Mesh();
        rightfilter.mesh = rightmesh;
        rightmesh.vertices = new Vector3[] { new Vector3(0, 0, 0.01f), new Vector3(0, atmospheresize.y, 0.01f), new Vector3(atmospheresize.x, 0, 0.01f), new Vector3(atmospheresize.x, atmospheresize.y, 0.01f) };
        rightmesh.uv = new Vector2[] { Vector2.zero, Vector2.up, Vector2.right, Vector2.one };
        rightmesh.triangles = new int[] { 0, 1, 3, 0, 3, 2 };
        Material rightmat = new Material(atmosphereshader);
        rightmat.SetTexture("_MainTex", atmosphereright);

        rightrenderer.material = rightmat;
    }
}
