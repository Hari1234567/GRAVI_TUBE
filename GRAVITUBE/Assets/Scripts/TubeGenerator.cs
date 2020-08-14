using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class TubeGenerator : MonoBehaviour
{

    public float radius = 10;


    public int faces = 100;


    public int length = 500;


    public float spacing = 4;


    static int tubeDifficulty = 0;
    



    public bool faceInside = true;




    public GameObject ghostLoop;
    public GameObject magnetLoop;
    public GameObject backWall;

    static int lengthFactor = -1;

    static int perlinOffset = 0;

    public static List<Vector3> axes;
    public static List<Vector3> centers;

    List<Vector3> firstVerts;
    List<Vector3> lastVerts;

    Vector3 lastCenter;
    static float xSeed = -1, ySeed = -1, zSeed = -1;
    Mesh tube;
    List<Vector3> verts;
    List<int> tris;

    MeshFilter tubeFilter;
    List<Vector2> uv;

    MeshFilter subMeshFilter;
    Mesh subMesh;
    List<Vector3> subVerts;
    List<int> subTris;
    List<Vector2> subUVs;


    public GameObject halfDoor;
    public GameObject slitDoor;
    bool mainMenu;

    public GameObject checkPoint;

    public GameObject collectible;

    public TextMeshProUGUI debug;
    static int tubeIndex = 0;

    static bool multiplayerFlag = false;
    bool multiplayer = false;



    public GameObject finishLine;


    static Vector3 axis, ref1, ref2;
    Vector3 cur;    // Start is called before the first frame update
    public static void Reset()
    {
        xSeed = ySeed = zSeed = -1;
        tubeIndex = 0;
        centers = null;
        axes = null;
        perlinOffset = 0;
        lengthFactor = -1;
        UIScript.pauseMenu = false;
        UIScript.gameOver = false;
        tubeDifficulty = 0;
        Time.timeScale = 1;

    }
    void Awake()
    {
        multiplayer = SceneManager.GetActiveScene().buildIndex == 3;
      
        if (!multiplayerFlag && multiplayer)
        {
            Reset();
            multiplayerFlag = true;
        }
        mainMenu = SceneManager.GetActiveScene().buildIndex == 0 || SceneManager.GetActiveScene().buildIndex==2;
        if (centers != null)
        {
            cur = centers[centers.Count - 1];

        }
        if (mainMenu && !multiplayer)
        {
            if (length > 800)
            {
                if (lengthFactor == -1)
                {
                    lengthFactor = -1 + length / 800;
                }
                length = 800;

            }
        }



        centers = new List<Vector3>();



        tube = new Mesh();
        verts = new List<Vector3>();
        tris = new List<int>();
        tubeFilter = GetComponent<MeshFilter>();
        firstVerts = new List<Vector3>();
        lastVerts = new List<Vector3>();

        uv = new List<Vector2>();


        verts.Clear();
        tris.Clear();
        uv.Clear();
        if (faceInside && (xSeed == -1f || SceneManager.GetActiveScene().buildIndex == 3))
        {
            if (SceneManager.GetActiveScene().buildIndex == 3)
            {
                xSeed = PlayerListingMenu.offsets[0];
                ySeed = PlayerListingMenu.offsets[1];
                zSeed = PlayerListingMenu.offsets[2];

            }
            else
            {
                xSeed = Random.Range(0f, 100f);
                ySeed = Random.Range(0f, 100f);
                zSeed = Random.Range(0f, 100f);
                if (debug != null)
                {
                    debug.text = "WELP";
                }
            }
        }
        if (debug != null)
        {
            debug.text = xSeed + " " + ySeed + " " + zSeed;
        }

        for (int i = 0; i < length; i++)
        {
            float offsetX = 10 * (Mathf.PerlinNoise((float)(perlinOffset + i) / 29, xSeed));

            float offsetY = 5 - 10 * (Mathf.PerlinNoise((float)(perlinOffset + i) / 29, ySeed));
            float offsetZ = 5 - 10 * (Mathf.PerlinNoise((float)(perlinOffset + i) / 29, zSeed));
         

            if (i == 0 && axes != null)
            {


                axis = axes[axes.Count - 1];
                axes.Clear();

            }
            else
            {
                axis = (Vector3.right + new Vector3(offsetX, offsetY, offsetZ)).normalized;
                if (axes == null)
                {
                    axes = new List<Vector3>();
                }
            }

            axes.Add(axis);
            if (i > 0)
                cur += axis * spacing;
            centers.Add(cur);
            if (i % 3 == 0)
            {
                float radius = this.radius * Mathf.PerlinNoise((float)i / 29, xSeed);
                float angle = 2 * Mathf.PI * Mathf.PerlinNoise((float)i / 27, ySeed);
                float x = cur.x + radius * Mathf.Cos(angle) * ref1.x + radius * Mathf.Sin(angle) * ref2.x;
                float y = cur.y + radius * Mathf.Cos(angle) * ref1.y + radius * Mathf.Sin(angle) * ref2.y;
                float z = cur.z + radius * Mathf.Cos(angle) * ref1.z + radius * Mathf.Sin(angle) * ref2.z;
                if (SceneManager.GetActiveScene().buildIndex==1)
                    Instantiate(collectible, new Vector3(x, y, z), Quaternion.LookRotation(Random.insideUnitSphere));
            }
            if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                if (i % 25 == 0 && Random.Range(0, 100) > squishFactor(tubeDifficulty))
                {
                    if (!mainMenu && tubeDifficulty != 0)
                    {
                        if (Random.Range(0, 100) > 30)
                        {
                            GameObject door = Instantiate(halfDoor, cur, Quaternion.LookRotation(axis));
                            door.transform.Rotate(0, 0, Random.Range(0, 360));
                        }
                        else
                        {
                            GameObject door = Instantiate(slitDoor, cur - transform.position, Quaternion.LookRotation(axis));
                            door.transform.Rotate(0, 0, Random.Range(0, 360));

                        }
                    }
                }
            }
            else if (SceneManager.GetActiveScene().buildIndex == 3)
            {


                int index = 799 * tubeIndex + i;
                try
                {
                    if (PlayerListingMenu.hasWall && PlayerListingMenu.wallIndices[index] != -1 && (tubeIndex != 0 ||i>30))
                    {
                        if (PlayerListingMenu.wallorslit[index])
                        {
                            GameObject door = Instantiate(halfDoor, centers[i], Quaternion.LookRotation(centers[i] - centers[i - 1]));
                            door.transform.Rotate(0, 0, PlayerListingMenu.wallRot[index]);
                        }
                        else
                        {
                            GameObject door = Instantiate(slitDoor, centers[i], Quaternion.LookRotation(centers[i] - centers[i - 1]));
                            door.transform.Rotate(0, 0, PlayerListingMenu.wallRot[index]);
                        }
                    }
                }catch(System.IndexOutOfRangeException a)
                {
                    Debug.Log("Index out of range at " + index + " with i = " + i + " at tube index " + tubeIndex);
                }catch(System.ArgumentOutOfRangeException n)
                {
                    Debug.Log("Argument out of range at " + index + " with i = " + i + " at tube index " + tubeIndex);
                }
            }
            if (i % 200 == 0)
            {

                float ang;
                if (!multiplayer)
                    ang = Random.Range(0, 360) * Mathf.Deg2Rad;
                else
                    ang = i % 360;
                if (!mainMenu && tubeIndex!=0)
                    Instantiate(ghostLoop, cur + (radius * 0.5f * new Vector3(Mathf.Cos(ang), Mathf.Sin(ang))), Quaternion.LookRotation(axis));


            }
            else if (i % 371 == 0)
            {
                float ang = Random.Range(0, 360) * Mathf.Deg2Rad;
                if (!mainMenu && !multiplayer)
                    Instantiate(magnetLoop, cur + (radius * 0.5f * new Vector3(Mathf.Cos(ang), Mathf.Sin(ang))), Quaternion.LookRotation(axis));
            }
            if (i  == length-1)
            {
                if (!mainMenu && !multiplayer)
                    Instantiate(checkPoint, cur, Quaternion.LookRotation(axis));
            }

            ref1 = Vector3.Cross(axis, Vector3.up).normalized;
            ref2 = Vector3.Cross(axis, ref1).normalized;
            if (ref1 == Vector3.zero || ref2 == Vector3.zero)
            {
                break;
            }
            for (int j = 0; j < faces; j++)
            {
                float fract = (float)j / faces;
                float angle = Mathf.Deg2Rad * fract * 360;
                float x = cur.x + radius * Mathf.Cos(angle) * ref1.x + radius * Mathf.Sin(angle) * ref2.x;
                float y = cur.y + radius * Mathf.Cos(angle) * ref1.y + radius * Mathf.Sin(angle) * ref2.y;
                float z = cur.z + radius * Mathf.Cos(angle) * ref1.z + radius * Mathf.Sin(angle) * ref2.z;

                verts.Add(new Vector3(x, y, z));
                if (j == 0)
                {
                    firstVerts.Add(verts[verts.Count - 1]);

                }
                if (j == faces - 1)
                {
                    lastVerts.Add(verts[verts.Count - 1]);
                }




            }



        }
       
        if ((tubeIndex == 0 && backWall != null))
        {
            Instantiate(backWall, centers[0], Quaternion.LookRotation(centers[1] - centers[0]));
        }
        tubeIndex++;
        tubeDifficulty++;

        lastCenter = centers[centers.Count - 1];
        for (int i = 0; i < verts.Count; i++)
        {


            float xFract = (i % (faces) / (float)(faces - 1));

            float yFract = ((i / (float)(length)));

            uv.Add(new Vector2(xFract, yFract));


        }



        for (int i = 0; i < length - 1; i++)
        {
            for (int j = faces * i; j < faces + faces * i - 1; j++)
            {

                if (j == (faces * i + faces - 1))
                {

                    tris.Add(j - faces + 1);
                    tris.Add(j);
                    tris.Add(j + 1 + faces - 1);


                    tris.Add(j - faces + 1);
                    tris.Add(j + 1 + faces - 1);
                    tris.Add(j + 1);




                }
                else
                {


                    tris.Add(j + faces);
                    tris.Add(j + 1);
                    tris.Add(j);

                    tris.Add(j + faces);
                    tris.Add(j + faces + 1);
                    tris.Add(j + 1);


                }

            }
        }


        tube.vertices = verts.ToArray();
        tube.uv = uv.ToArray();

        tube.triangles = tris.ToArray();
        tube.RecalculateBounds();
        tube.RecalculateNormals();
        tubeFilter.mesh = tube;
        MeshCollider meshCollider = GetComponent<MeshCollider>();

        if (meshCollider != null)
        {
            meshCollider.sharedMesh = tube;
        }

        subMeshFilter = GetComponentsInChildren<MeshFilter>()[1];
        subMesh = new Mesh();
        subVerts = new List<Vector3>();
        subTris = new List<int>();
        subUVs = new List<Vector2>();
        for (int i = 0; i < length; i++)
        {
            subVerts.Add(firstVerts[i]);
            subVerts.Add(lastVerts[i]);
        }
        for (int i = 0; i < subVerts.Count - 3; i++)
        {
            subTris.Add(i);
            subTris.Add(i + 1);
            subTris.Add(i + 2);
            subTris.Add(i + 1);
            subTris.Add(i + 3);
            subTris.Add(i + 2);
        }
        for (int i = 0; i < subVerts.Count; i++)
        {

            subUVs.Add(new Vector2((i / 2) / ((float)subVerts.Count / 2), i % 2));

        }
        subMesh.vertices = subVerts.ToArray();
        subMesh.triangles = subTris.ToArray();
        subMesh.uv = subUVs.ToArray();
        GetComponentsInChildren<MeshCollider>()[1].sharedMesh = subMesh;
        subMeshFilter.mesh = subMesh;
        if (PlayerListingMenu.length==1)
        {
            if (SceneManager.GetActiveScene().buildIndex == 3)
            {
                if (finishLine != null)
                {
                  
                    Instantiate(finishLine, centers[(int)(centers.Count / 1.4f)], Quaternion.LookRotation(centers[(int)(centers.Count / 1.4f)] - centers[(int)(centers.Count / 1.4f )- 1]));
                }
               
            }
        }
        
        if (PlayerListingMenu.length > 1 && SceneManager.GetActiveScene().buildIndex==3)
        {
            PlayerListingMenu.length--;
            Instantiate(gameObject);

        }
        perlinOffset += length;

        
    }


    bool done = false;
    private void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex==1)
        {
            try
            {
                if (!done && (PlayerController.camRB.transform.position - Vector3.zero).magnitude > (centers[centers.Count / 2] - Vector3.zero).magnitude)
                {
                    
                
          
                    Instantiate(gameObject);
                    done = true;

                }
            }
            catch (System.NullReferenceException) { }

            if ((PlayerController.camRB.transform.position - Vector3.zero).magnitude - (lastCenter - Vector3.zero).magnitude > 600)
            {
                Destroy(gameObject);
            }
        }
    }

    float squishFactor(float diff)
    {
        return 1 / (1 + Mathf.Pow(3, -diff/2));
    }


}
