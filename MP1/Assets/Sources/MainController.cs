using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class MainController : MonoBehaviour
{
    public GameObject CreationTarget;
    public Camera MainCamera = null;
    public GameObject mSelectedObject = null;
    public RaycastHit hitObj;
    public Dropdown Dropdown = null;
    

    private float targetHeight;
    private float range = 7f;
    

    // Start is called before the first frame update
    void Start()
    {
        // create CreationTarget
        CreationTarget = GameObject.Find("CreationTarget");
        targetHeight = CreationTarget.GetComponent<MeshRenderer>().bounds.size.y;
        CreationTarget.transform.position = new Vector3(0, targetHeight/2f, 0);
        CreationTarget.GetComponent<Renderer>().material.color = Color.black;

        // Dropdown
        Dropdown.onValueChanged.AddListener(CreateNewObject);
    }

    // Update is called once per frame
    void Update()
    {
        LMClick();

        
    }

    void LMClick()
    {
        //TODO: when the mouse click on the plane, the CreationTarget will move the hit location.
        if (Input.GetMouseButtonUp(0))
        {
            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(MainCamera.ScreenPointToRay(Input.mousePosition), out hitInfo, 100f);

            if (hit)
            {
                mSelectedObject = hitInfo.transform.gameObject;
                Debug.Log("Hit " + mSelectedObject.name);
                if (mSelectedObject != null & mSelectedObject.name != "CreationTarget" & mSelectedObject.name != "CreationPlane")
                {
                    Destroy(mSelectedObject);
                    mSelectedObject = null;
                }

                else if (Mathf.Abs(hitInfo.point.x) <= range & Mathf.Abs(hitInfo.point.z) <= range)
                {
                    
                    Vector3 targetNextPos = new Vector3(hitInfo.point.x, targetHeight / 2f, hitInfo.point.z);
                    //CreationTarget.transform.position = targetNextPos;
                    CreationTarget.transform.position = targetNextPos;
                    
                    Debug.Log("Hit Position: " + hitInfo.point);
                }
                hitObj = hitInfo;
            }
        }
    }

    void CreateNewObject(int value)
    {
        switch (value)
        {
            case 1:
                GameObject sphere = Instantiate(Resources.Load("Sphere")) as GameObject;
                sphere.transform.position = new Vector3(hitObj.point.x, 0.5f, hitObj.point.z);
                
                sphere.GetComponent<Renderer>().material.color = new Color(1, 1, 1);

                Debug.Log("Hit point: " + hitObj.point + ", Sphere location: " + sphere.transform.position);
                

                Dropdown.value = 0;
                break;

            case 2:
                GameObject cube = Instantiate(Resources.Load("Cube")) as GameObject;
                cube.transform.position = new Vector3(hitObj.point.x, 0.5f, hitObj.point.z);
                
                cube.GetComponent<Renderer>().material.color = new Color(1, 1, 1);

                Debug.Log("Hit point: " + hitObj.point + ", Cube location: " + cube.transform.position);
                

                Dropdown.value = 0;
                break;

            case 3:
                GameObject cylinder = Instantiate(Resources.Load("Cylinder")) as GameObject;
                cylinder.transform.position = new Vector3(hitObj.point.x, 2f, hitObj.point.z);
                
                cylinder.GetComponent<Renderer>().material.color = new Color(1, 1, 1);

                Debug.Log("Hit point: " + hitObj.point + ", Cyliner location: " + cylinder.transform.position);
                

                Dropdown.value = 0;
                break;
        }
    }
}
