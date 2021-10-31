using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainControl : MonoBehaviour
{
    public AimLine aimLine = null;
    public Camera MainCamera = null;
    public GameObject TheBarrier = null;
    public XFormControl xform = null;
    public TheWorld theWorld = null;
    public SliderWithEcho Speed=null, Interval=null, Life=null;
    public Button quit = null;
    public GameObject PauseScreen;
    public Button PauseButton;
    public Button ResumeButton;

    bool GamePaused;
    
    // bool isDraging;
    // Start is called before the first frame update
    void Start()
    {
        
        // Debug.Log(); 
        GamePaused = false;
        PauseScreen.SetActive(false);
        // PauseButton.SetActive(true);

        Speed.InitSliderRange(0.5f, 15f, 5f);
        Interval.InitSliderRange(0.5f, 4f, 1f);
        Life.InitSliderRange(1f, 15f, 10f);

        Speed.SetSliderListener(ChangeSpeed);
        Interval.SetSliderListener(ChangeInterval);
        Life.SetSliderListener(ChangeMaxLife);

        xform.SetSelectedObject(GameObject.Find("TheBarrier").transform);

        // button.onClick.AddListener(StopRestart);
        quit.onClick.AddListener(Quit);
        PauseButton.onClick.AddListener(PauseGame);
        ResumeButton.onClick.AddListener(ResumeGame);
    }

    // Update is called once per frame
    void Update()
    {
        if (GamePaused)
        {
            Time.timeScale = 0;
        } else {
            Time.timeScale = 1;
        }
        ProcessMouseEvents();
    }
    public void PauseGame(){
        GamePaused = true;
        PauseScreen.SetActive(true);
        // PauseButton.SetActive(false);
    }

    public void ResumeGame() 
    {
        GamePaused = false;
        PauseScreen.SetActive(false);
        // PauseButton.SetActive(true);
    }
    void Quit()
    {
        Application.Quit();
    }
 

    void ProcessMouseEvents()
    {
      GameObject selectedObj;
      Vector3 hitPt;
      if (Input.GetMouseButtonDown(0)) // click event
      {   
          if (MouseSelectObjectAt(out selectedObj, out hitPt, LayerMask.GetMask("EndPtLayer")))
            {
  
              if (selectedObj.name.Contains("LineEndPt"))
              {     
                //   Debug.Log("selected LineEndPt");
                  theWorld.SetSelected(ref selectedObj);
              } else {
                //   Debug.Log("Create a new aimLine");
                  Vector3 lp = new Vector3(-17f, hitPt.y, hitPt.z);
                  Vector3 rp = new Vector3(17f, hitPt.y, hitPt.z);
                  theWorld.CreateAimLineAt(lp, rp);
                  
              }
            } else if (MouseSelectObjectAt(out selectedObj, out hitPt, LayerMask.GetMask("LineLayer")))
            {
                if (selectedObj.tag == "BigLine"){
                    // Debug.Log("delete " + selectedObj.name);
                    DestroyBigLine(selectedObj);
                    
                } else if (selectedObj.tag == "LineSegment")
                {
                    // destory endpoints
                    Destroy(selectedObj.GetComponent<AimLine>().Pl.gameObject);
                    Destroy(selectedObj.GetComponent<AimLine>().Pr.gameObject);
                    // destory line
                    Destroy(selectedObj);
                    selectedObj = null;
                }
            } else if (MouseSelectObjectAt(out selectedObj, out hitPt, LayerMask.GetMask("WallLayer")))
            {
                
                if (selectedObj.name == "LeftWall"){
                    // Debug.Log("Create a new aimLine on the wall " + selectedObj.name);
                    Vector3 lp = new Vector3(-17f, hitPt.y, hitPt.z);
                    Vector3 rp = new Vector3(17f, hitPt.y, hitPt.z);
                    theWorld.CreateAimLineAt(lp, rp);
                } else if (selectedObj.name == "BackWall")
                {
                    // Debug.Log("Create a new BigLine on the wall");
                    Vector3 po = new Vector3(hitPt.x, hitPt.y, -7f);
                    Vector3 pe = new Vector3(hitPt.x, hitPt.y, 24f);
                    theWorld.CreateBigLineAt(po, pe);
                }
            } 
      } else if (Input.GetMouseButton(0))// Mouse rag
      {
            if (MouseSelectObjectAt(out selectedObj, out hitPt, LayerMask.GetMask("WallLayer")))
            {   
                theWorld.SetEndPtPosition(hitPt);
            }
      } else if (Input.GetMouseButtonUp(0)) // Mouse Release
      {
          // set selected location
          if (MouseSelectObjectAt(out selectedObj, out hitPt, LayerMask.GetMask("WallLayer")))
          {   
              theWorld.SetEndPtPosition(hitPt);
          }
          // release selection
          theWorld.ReleaseSelection();
      }
    
    }

    void DestroyBigLine(GameObject selectedObj)
    {
        if (selectedObj.tag == "BigLine")
        {
            Vector3 po = selectedObj.GetComponent<BigLine>().Po.position;
            Vector3 vn = selectedObj.transform.up;
            float radius = selectedObj.GetComponent<BigLine>().radius / 2f;
            Vector3 pon;
            float d;
            foreach(GameObject projBall in GameObject.FindGameObjectsWithTag("LineProjBall"))
            {
                d = Vector3.Dot(projBall.transform.position - po, vn);
                pon = po + d * vn;
                if ((projBall.transform.position - pon).magnitude < radius + 0.1f)
                {
                    // Debug.Log("Destory " + projBall.transform.position);
                    theWorld.DestroyBigLine(selectedObj);
                    Destroy(projBall);
                }
            }

            // destory endpoints
            Destroy(selectedObj.GetComponent<BigLine>().Po.gameObject);
            Destroy(selectedObj.GetComponent<BigLine>().Pe.gameObject);
            // destory line
            Destroy(selectedObj);
            selectedObj = null;
        }
        
    }


    bool MouseSelectObjectAt(out GameObject g, out Vector3 p, int layerMask)
    {
        RaycastHit hitInfo = new RaycastHit();
        bool hit = Physics.Raycast(MainCamera.ScreenPointToRay(Input.mousePosition), out hitInfo, Mathf.Infinity, layerMask);
        // Debug.Log("MouseSelect:" + layerMask + " Hit=" + hit);
        if (hit)
        {
            g = hitInfo.transform.gameObject;
            p = hitInfo.point;
        } else
        {
            g = null;
            p = Vector3.zero;
        }
        return hit;
    }

    void ChangeSpeed(float v) {theWorld.SetSpeed(v);}
    void ChangeInterval(float v) {theWorld.SetInterval(v);}
    void ChangeMaxLife(float v) {theWorld.SetMaxLife(v);}
}
