using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectBall : MonoBehaviour
{

    public Transform ball, bigline, line;
    
    // Start is called before the first frame update
    void Start()
    {
        // PositionUpdate();
    }

    // Update is called once per frame
    void Update()
    {
        PositionUpdate();
        if (line.gameObject.GetComponent<Line>().halfLen > 5f) {
            Destroy(gameObject);
            Destroy(line.gameObject);
        }
        if (bigline == null | ball == null)
        {
            Destroy(gameObject);
            Destroy(line.gameObject);
        }
        
        // if line is less than 0.6f, reflect
        if (ball != null & bigline != null & line != null)
        {
            float D = Vector3.Dot(bigline.position, bigline.right);
            TravellingBall script = ball.GetComponent<TravellingBall>();
            Vector3 mDir = script.mDir;
            Vector3 vn = bigline.right;
            if ((ball.position - transform.position).magnitude < 0.6f & Vector3.Dot(vn, mDir) < 0)
            {
                Vector3 newDir = ReflectDir(D, vn, mDir);
                script.SetDirection(newDir);
            }
        }
        
    }

    Vector3 ReflectDir(float D, Vector3 vn, Vector3 curDir)
    {
        float d = 1f;
        Vector3 p1 = transform.position - d * curDir;
        float h = Vector3.Dot(p1, vn) - D;
        Vector3 pm = transform.position + h * vn;
        Vector3 p2 = pm + (pm - p1);
        Vector3 newDir = (p2 - transform.position).normalized;
        return newDir;

    }

    public void SetParam(Transform b, Transform bl, Transform l) 
    {
        ball = b;
        bigline = bl;
        line = l;
    }
    void PositionUpdate()
    {
        if (bigline != null & ball != null)
        {
            Vector3 po = bigline.gameObject.GetComponent<BigLine>().Po.position;
            float radius = bigline.gameObject.GetComponent<BigLine>().radius;
            Vector3 bigline_vn = bigline.up;
            Vector3 v1 = ball.position - po;
            float d = Vector3.Dot(v1, bigline_vn);
            Vector3 pon = po + d * bigline_vn;
            transform.position = pon + (ball.position - pon).normalized * radius / 2f;
        }
        
    }
}
