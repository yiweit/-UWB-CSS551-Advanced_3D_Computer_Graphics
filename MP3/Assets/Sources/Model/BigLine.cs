using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigLine : MonoBehaviour
{
    public Transform Po, Pe;
    public float radius;
    // Start is called before the first frame update
    void Start()
    {
        radius = Po.localScale.x;
        SetLinePos(Po, Pe);
    }


    public void SetLinePos(Transform o, Transform e)
    {
    //   Pb.position = b.position;
    //   Pf.position = 
    //   Vector3 v = Pb.position - Pf.position;
      
    //   transform.up = v.normalized;
    //   float halfLen = v.magnitude / 2.0f;
    //   transform.localScale = new Vector3(radius, halfLen, radius);
    //   transform.localPosition = Pf.localPosition + halfLen * v.normalized;
        Po.position = o.position;
        Pe.position = e.position;
        Vector3 v = Pe.position - Po.position;

        transform.up = v.normalized;
        float halfLen = v.magnitude / 2.0f;
        transform.localScale = new Vector3(radius, halfLen, radius);
        transform.position = Po.position + halfLen * v.normalized;

    }

    // public void BuildProjectBall()
    // {
    //     Vector3 v = transform.up;
    //     Vector3 v1, Pon;
    //     float d, h;
    //     foreach(GameObject ball in GameObject.FindGameObjectsWithTag("Ball"))
    //     {
    //         v1 = ball.transform.position - Po.position;
    //         d = Vector3.Dot(v1, v);
    //         Pon = Po.position + d * v;
    //         h = (Pon - ball.transform.position).magnitude;

    //         if (HashSet < 10f)
    //         {
    //             Transform projBall = null;
    //             Transform projLine = null;
    //             foreach(GameObject pb in gameObject.transform.Find("ProjBall"))
    //             {

    //             }
    //         }
    //     }
    // }
}
