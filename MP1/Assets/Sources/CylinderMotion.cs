using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CylinderMotion : MonoBehaviour
{
    private float speed = 1f;
    private float mDir = 1f;
    public float zRange = 5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // boundary issue
        if (transform.position.z > zRange)
        {
            mDir = -1f;
        }
        if (mDir < 0f & transform.position.z < 0f)
        {
            mDir *= -1;
        }
        transform.position += mDir * new Vector3(0, 0, speed * Time.deltaTime);

        // color changes when direction changes
        if (mDir > 0)
        {
            GetComponent<Renderer>().material.color = new Color(1, 1, 1);
        }
        else
        {
            GetComponent<Renderer>().material.color = new Color(1, 1, 0);
        }
    }
}
