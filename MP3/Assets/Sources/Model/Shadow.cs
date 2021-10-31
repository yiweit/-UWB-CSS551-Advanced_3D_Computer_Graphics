using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow : MonoBehaviour
{
    // Start is called before the first frame update
    Transform ball, barrier;
    void Start()
    {
        gameObject.GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        gameObject.GetComponent<Renderer>().material.color = Color.black;
    }

    // Update is called once per frame
    void Update()
    {
        ProjectOnBarrier();
    }

    void ProjectOnBarrier()
    {
        Vector3 barrier_vn = barrier.gameObject.GetComponent<Barrier>().normalDir;
        Vector3 barrier_pos = barrier.position;
        Vector3 vt = ball.position - barrier_pos;
        if (Vector3.Dot(vt.normalized, barrier_vn) > 0)
        {   
            // come from the front
            float D = Vector3.Dot(barrier_pos, barrier_vn);
            float d = Vector3.Dot(ball.position, barrier_vn);
            Vector3 Pon = ball.position - (d-D-0.1f/2f) * barrier_vn;
            GameObject on = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            on.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            on.transform.position = Pon;
            transform.position = Pon;
            if ((Pon - barrier_pos).magnitude < 6f)
            {
                transform.up = barrier_vn;
                // transform.position = Pon;
                float multipler = 2f / (d-D);
                if (multipler > 1f)
                {
                    transform.localScale = new Vector3(1.1f, 0.1f, 1.1f);
                } else if (multipler < 0.1f) 
                {
                    transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                } else 
                {
                    transform.localScale = new Vector3(multipler, 0.1f, multipler);
                }
            }
        } else {
            Destroy(gameObject);
        }
    }

    public void SetParam(Transform bl, Transform br)
    {
        ball = bl;
        barrier = br;
    }
}
