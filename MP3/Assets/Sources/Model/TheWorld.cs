using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheWorld : MonoBehaviour
{
    GameObject mSelected = null;
    // SliderWithEcho Speed, Interval, Life;
    public float speed ; 
    public float interval = 5; 
    public float maxLife;
    // [SerializeField] TravellingBall ballScript;
    private float curTime = 0;
    private float prev_time;

    // Start is called before the first frame update
    void Start()
    {
        // Speed.onValueChanged.AddListener();
        // Interval.onValueChanged.AddListener();
        // Life.onValueChanged.AddListener();
        CreateAimLineAt(new Vector3(-17f, 11.36f, 8f), new Vector3(17f, 11.3f, 8f));
        CreateBigLineAt(new Vector3(-9f, 2.5f, -7f), new Vector3(-9f, 2.5f, 24f));
        CreateBall();
    }

    // Update is called once per frame
    void Update()
    {
        curTime += Time.deltaTime;
        // Debug.Log("curTime: " + curTime.ToString());
        if (curTime > interval) 
        {
            // Debug.Log("create ball");
            CreateBall();
            curTime = 0;
        }
    }

    void CreateBall()
    {
        GameObject[] lines = GameObject.FindGameObjectsWithTag("LineSegment");
        foreach (GameObject aimline in lines)
        {
            
            
            Vector3 v = aimline.transform.up;
            float d = aimline.transform.localScale.y;
            // Vector3 pl = p0 + d * v;
            Vector3 pl = aimline.GetComponent<AimLine>().Pl.position;
            Vector3 pr = aimline.GetComponent<AimLine>().Pr.position;
            Vector3 n = (pr - pl).normalized;
            // Debug.Log("n: " + n.ToString() + ", v: " + v.ToString());
            
            // Debug.Log(" v: " + v.ToString() +  ", pl: " + pl.ToString());
            GameObject ball = Instantiate(Resources.Load("Ball")) as GameObject;
            
            ball.transform.position = pl;
            // Debug.Log(ball.name + " pos: " + ball.transform.position.ToString());
            TravellingBall ballScript = ball.GetComponent<TravellingBall>();
            // Debug.Log(ballScript.ToString());
            // ballScript.SetSpeed(speed);
            // ball.SetMaxLife(maxLife);
            // ball.GetComponent<TravellingBall>().SetDirection(v);
            ballScript.SetSpeed(speed);
            ballScript.SetMaxLife(maxLife);
            ballScript.SetDirection(n);
            // ball.AddComponent<ballScript>();
        }
        
    }
    public void SetSpeed(float s) {
        speed = s;
        // Debug.Log("TheWorld speed to " + speed.ToString()); 
        }
    public void SetInterval(float v) {
        interval = v; 
        // Debug.Log("TheWorld direction to " + interval.ToString()); 
        }
    public void SetMaxLife(float l) {
        maxLife = l; 
        // Debug.Log("TheWorld maxLife to " + maxLife.ToString()); 
        }

    
    public void CreateAimLineAt(Vector3 lp, Vector3 rp)
    {
    
        GameObject lpt = CreateEndPtAt(lp);
        GameObject rpt = CreateEndPtAt(rp);
        GameObject aimLine = Instantiate(Resources.Load("LineSegment")) as GameObject;
        aimLine.GetComponent<AimLine>().Pl = lpt.transform;
        aimLine.GetComponent<AimLine>().Pr = rpt.transform;
        
    }

    public void CreateBigLineAt(Vector3 Po, Vector3 Pe){
        GameObject po = CreateEndPtAt(Po);
        GameObject pe = CreateEndPtAt(Pe);
        GameObject bigLine = Instantiate(Resources.Load("BigLine")) as GameObject;
        bigLine.GetComponent<BigLine>().Po = po.transform;
        bigLine.GetComponent<BigLine>().Pe = pe.transform;
        foreach(GameObject ball in GameObject.FindGameObjectsWithTag("Ball"))
        {
            TravellingBall scr = ball.GetComponent<TravellingBall>();
            scr.AddBigLine(bigLine);
        }
    }

    private GameObject CreateEndPtAt(Vector3 pos)
    {
        GameObject pt = Instantiate(Resources.Load("LineEndPt")) as GameObject;
        pt.layer = LayerMask.NameToLayer("EndPtLayer");
        pt.transform.position = pos;
        return pt;
    }

    public void DestroyBigLine(GameObject bigline)
    {
        foreach(GameObject ball in GameObject.FindGameObjectsWithTag("Ball"))
        {
            TravellingBall scr = ball.GetComponent<TravellingBall>();
            scr.DestroyBigLine(bigline);
        }
    }
    public void SetEndPtPosition(Vector3 pos){
        if (mSelected != null){
            // Debug.Log(mSelected.name + " to pos: " + pos.ToString());
            mSelected.transform.position = pos;
        }
    }

    public void SetSelected(ref GameObject g){
        if (mSelected != null)
        {
            ReleaseSelection();
        }
        mSelected = g;
        mSelected.GetComponent<Renderer>().material.color = Color.black;
        
    }
     
    public void ReleaseSelection()
    {
        if (mSelected != null)
        {
            mSelected.GetComponent<Renderer>().material.color = Color.red;
            mSelected = null;
        }
        
    }


}
