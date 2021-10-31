using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    public float activeRadius;
    public Vector3 normalDir;
    public Vector3 position;
    // Start is called before the first frame update
    void Start()
    {
        activeRadius = 6f;
        position = transform.position;
        normalDir =  Vector3.Cross(transform.up, transform.right);
        GameObject normLine = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        normLine.name = "NormalLine";
        normLine.GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        normLine.GetComponent<Renderer>().material.color = Color.blue;
        normLine.transform.up = normalDir;
        normLine.transform.localScale = new Vector3(0.1f, 5f, 0.1f);
        normLine.transform.position = transform.position + 5f * normalDir; 
        normLine.transform.parent = gameObject.transform;

    }

    // Update is called once per frame
    void Update()
    {
        normalDir =  Vector3.Cross(transform.up, transform.right);
        // GameObject[] aimLines = GameObject.FindGameObjectsWithTag("LineSegment");
        // foreach(GameObject line in aimLines)
        // {
        //     Debug.Log("find line: " + line.name);
        //     if (Vector3.Dot(line.transform.up, normalDir) != 0)
        //     {   
        //         Debug.Log("create reflection line");
        //         CreateReflectionLine(line);
        //     }
        // }
    }

    // void CreateReflectionLine(GameObject line)
    // {
    //     Vector3 pl = line.GetComponent<AimLine>().Pl.position;
    //     Vector3 pr = line.GetComponent<AimLine>().Pr.position;
    //     Vector3 v1 = pr - pl;
    //     float D = Vector3.Dot(transform.position, normalDir);
    //     float d = (D - (Vector3.Dot(pl, normalDir))) / (Vector3.Dot(v1, normalDir));
    //     Vector3 pon = pl + v1.normalized * d;
    //     // check whether the intersection is on the active radius
    //     if ((pon - transform.position).magnitude < activeRadius)
    //     {
    //         Vector3 von = pl - pon;
    //         Vector3 reflect_dir = (2 * Vector3.Dot(von, normalDir) * normalDir - von).normalized;
    //         float length = von.magnitude / 2f;
    //         Vector3 pos = pon + length * reflect_dir;
    //         GameObject reflect_line = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
    //         reflect_line.name = "ReflectionLine";
    //         reflect_line.transform.localScale = new Vector3(0.2f, length, 0.2f);
    //         reflect_line.transform.position = pos;
    //         reflect_line.GetComponent<Renderer>().material.color = Color.green;
    //     }        
    // }
}
