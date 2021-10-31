using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    public Transform po, pe;
    public float halfLen;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        gameObject.GetComponent<Renderer>().material.color = Color.black;
    }

    // Update is called once per frame
    void Update()
    {
        
        SetLinePos();
        if (po == null | pe == null)
        {
            Destroy(gameObject);
        }
        if (transform.localScale == Vector3.one)
        {
            Debug.Log("scale of 1 line: " + transform.position.ToString());
            gameObject.GetComponent<Renderer>().material.color = Color.green;
        }
    }

    public void SetParam(Transform o, Transform e)
    {
        po = o;
        pe = e;
    }
    void SetLinePos()
    {
        if (pe != null & po != null)
        {
            Vector3 v = pe.position - po.position;
            transform.up = v.normalized;
            halfLen = v.magnitude / 2.0f;
            // Debug.Log("length: " + v.magnitude.ToString() + ", halfLen: " + halfLen.ToString());
            transform.localScale = new Vector3(0.02f, halfLen, 0.02f);
            transform.position = po.position + halfLen * v.normalized;
        }
        
    }
}
