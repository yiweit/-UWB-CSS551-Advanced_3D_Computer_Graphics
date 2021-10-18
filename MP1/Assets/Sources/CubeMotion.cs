using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMotion : MonoBehaviour
{
    private float speed = 1f;
    private float mDir = 1f;
    public float yRange = 5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // boundary issue
        if (transform.position.y > yRange)
        {
            mDir = -1f;
        }
        if (mDir < 0f & transform.position.y < 0.5f)
        {
            mDir *= -1;
        }
        transform.position += mDir * new Vector3(0, speed * Time.deltaTime, 0);
        transform.Rotate(0, 90 * Time.deltaTime, 0);

        // color changes when direction changes
        if (mDir > 0)
        {
            GetComponent<Renderer>().material.color = new Color(1, 1, 1);
        }
        else
        {
            GetComponent<Renderer>().material.color = new Color(1, 0, 1);
        }
    }
}
