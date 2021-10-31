using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimLine : MonoBehaviour
{
    public Transform Pl, Pr;
    GameObject reflect_line;

    
    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
      SetLinePos(Pl, Pr);

      // reflection
      Reflection();

    }

    public void SetPl(Vector3 p)
    {
      // Debug.Log("reset Pl to " + p.ToString());
      Pl.localPosition = p;
    }

    public void SetPr(Vector3 p)
    {
      // Debug.Log("reset Pr to " + p.ToString());
      Pr.localPosition = p;
    }

 

    public void SetLinePos(Transform l, Transform r)
    {
      SetPl(l.position);
      SetPr(r.position);
      Vector3 v = Pr.position - Pl.position;
      transform.up = v.normalized;
      float halfLen = v.magnitude / 2.0f;
      transform.localScale = new Vector3(0.2f, halfLen, 0.2f);
      transform.localPosition = Pl.localPosition + halfLen * v.normalized;
    }

    public void Reflection(){
      GameObject barrier = GameObject.Find("TheBarrier");
      Vector3 normalDir = barrier.GetComponent<Barrier>().normalDir;
      float activeRadius = barrier.GetComponent<Barrier>().activeRadius;
      if (Vector3.Dot(gameObject.transform.up, normalDir) != 0) // not parallel
      {   
          // Debug.Log("have reflection");
          // find the intersected point
          Vector3 v1 = transform.up; // vector from left endpt to right endpt
          float D = Vector3.Dot(barrier.transform.position, normalDir); // distance from any point on the barrier to the origin
          float d = (D - (Vector3.Dot(Pl.position, normalDir))) / (Vector3.Dot(v1, normalDir)); // distance from left end to intersection point 
          Vector3 pon = Pl.position + v1.normalized * d; // intersection point

          // have reflection line when intersected point is on the active radius
          // Debug.Log("distance from pon to barrier center: " + (pon - barrier.transform.position).magnitude.ToString());
          if ((pon - barrier.transform.position).magnitude < activeRadius)
          {
              // Debug.Log("having reflection line");

              if (reflect_line == null)
              {
                reflect_line = CreateReflectionLine();
              }
              Vector3 von = Pl.position - pon;
              Vector3 reflect_dir = (2 * Vector3.Dot(von, normalDir) * normalDir - von).normalized;
              float length = von.magnitude / 2.0f;
              Vector3 pos = pon + length/2f * reflect_dir;
              Vector3 parent_size = transform.localScale;
              reflect_line.transform.localScale = new Vector3(0.2f, length/2f, 0.2f);
              reflect_line.transform.position = pos;
              reflect_line.transform.up = reflect_dir;

              Debug.DrawLine(pon, pon + 4f * reflect_dir, Color.blue, 0.5f);
          } else {
            Destroy(reflect_line);
            reflect_line = null;
          }
  
      }
    }

    GameObject CreateReflectionLine()
    {
        GameObject reflect_line = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        reflect_line.name = "ReflectionLine";
        reflect_line.GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        reflect_line.GetComponent<Renderer>().material.color = Color.green;   
        // reflect_line.transform.parent = gameObject.transform;
        return reflect_line;
    }
}
