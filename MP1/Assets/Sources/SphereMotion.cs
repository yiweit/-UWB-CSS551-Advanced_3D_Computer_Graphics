using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereMotion : MonoBehaviour
{
    
    
    private float speed = 1f;
    private float mDir = 1f;
    public float xRange = 5f;
    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
        // boundary issue
        if (transform.position.x + 0.5f > xRange)
        {
            Debug.Log("Sphere changes direction, " + transform.position);
            mDir = -1f;
        }
        if (mDir < 0f & transform.position.x < 0f)
        {
            mDir *= -1;
            Debug.Log("Sphere reaches 0-x, change direction, " + transform.position);

        }
        transform.position += mDir * new Vector3(speed * Time.deltaTime, 0, 0);

        // color changes when direction changes
        if (mDir > 0)
        {
            GetComponent<Renderer>().material.color = new Color(1, 1, 1);
        } else
        {
            GetComponent<Renderer>().material.color = new Color(0, 1, 1);
        }
    }


}
