using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravellingBall : MonoBehaviour
{
    public float speed = 5;
    public float maxLife;
    public Vector3 mDir;
    private float curLife = 0;
    private float prev_life;
    public float prev_speed;
    GameObject shadow, shadow_line, barrier;

    public Dictionary<GameObject, bool[]> inRange;

    // Start is called before the first frame update
    void Start()
    {
        barrier = GameObject.Find("TheBarrier");
        // PonBarrier = new GameObject();
        inRange = new Dictionary<GameObject, bool[]>(); // List<bool> [0]: inRange or not, [1]: has projBall or not
        gameObject.GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        // Debug.Log(gameObject.name + "=> speed: " + speed.ToString() + ", maxLife: " + maxLife.ToString() + ", mDir: " + mDir.ToString());
        foreach(GameObject bigline in GameObject.FindGameObjectsWithTag("BigLine"))
        {
            AddBigLine(bigline);
        }
        // ProjBigLine();
    }
    void Update()
    {   
        // projection
        ProjBigLine();

        ProjectOnBarrier();
        // if ball collides on the barrier -> reflect (change direction)
        OnCollide();
        curLife += Time.deltaTime;
        if (curLife > maxLife)
        {   
            // Debug.Log( gameObject.name + "=> curLife: " + curLife.ToString() + "maxLife: "+ maxLife.ToString() + ", Destory the ball");
            Destroy(transform.gameObject);
            curLife = 0;
        }
        // moving
        transform.position += mDir * speed * Time.deltaTime;
       
        
    }

    public void AddBigLine(GameObject bigline)
    {
        Vector3 h = distanceFromBigLine(bigline);
        if (h.magnitude < 10f) {
            
            CreateProjectBallLine(bigline);
            
            inRange.Add(bigline, new bool[] {true, true});
        } else {
            inRange.Add(bigline, new bool[] {false, false});
        }
    }
    Vector3 distanceFromBigLine(GameObject BigLine)
    {
        float radius = BigLine.GetComponent<BigLine>().radius / 2f;
        Transform Po = BigLine.GetComponent<BigLine>().Po;
        Vector3 va = transform.position - Po.position;
        Vector3 v = BigLine.transform.up;
        float d = Vector3.Dot(va, v);
        Vector3 ph = Po.position + d * v;
        Vector3 h = transform.position - ph;
        return h;
    }
    // Update is called once per frame
    
    
    void ProjBigLine()
    {
        List<GameObject> f2t = new List<GameObject>(); // value from false to true
        List<GameObject> t2f = new List<GameObject>(); // value from true to false
        foreach (KeyValuePair<GameObject, bool[]> pair in inRange)
        {
            if (pair.Key != null) {
                if (!pair.Value[0] & !pair.Value[1]) // prev not in range, and no proj ball
                {
                    Vector3 h = distanceFromBigLine(pair.Key);
                    if (h.magnitude < 10f)
                    {
                        f2t.Add(pair.Key);
                      // Debug.Log("Ball at " + transform.position.ToString() + " is " + h.magnitude.ToString() 
                        // + " away from the bigLine at " + pair.Key.transform.position.ToString() + "(" + pair.Value[0] + ", " + pair.Value[1] + ")");
                        
                        
                    }
                } else if(pair.Value[0] & pair.Value[1]) { 
                    // prev in range with proj, but now diff
                    Vector3 h = distanceFromBigLine(pair.Key);
                    if (h.magnitude > 9.9f)
                    {
                        t2f.Add(pair.Key);
                        
                    }
                }
                
            }
        }
        foreach(GameObject g in t2f)
        {
            // Debug.Log("For ball at " + transform.position.ToString() +": bigLine at " + g.transform.position.ToString() + "(" + inRange[g][0] + ", " + inRange[g][1] + ")");
            inRange[g][0] = false;
            // Debug.Log("For ball at " + transform.position.ToString() +": bigLine at " + g.transform.position.ToString() + "(" + inRange[g][0] + ", " + inRange[g][1] + ") after change");
        }
        foreach(GameObject g in f2t)
        {
            
          // Debug.Log("For ball at " + transform.position.ToString() +": bigLine at " + g.transform.position.ToString() + "(" +  inRange[g][0] + ", " + inRange[g][1] + ")");
            inRange[g][0] = true;
            CreateProjectBallLine(g);
            inRange[g][1] = true;
          // Debug.Log("For ball at " + transform.position.ToString() +": bigLine at " + g.transform.position.ToString() + "(" +  inRange[g][0] + ", " + inRange[g][1] + ") after change");
        }
        
        
    }

    public void DestroyBigLine(GameObject bigline)
    {
        inRange.Remove(bigline);
    }
    void ProjectOnBarrier()
    {
        // find barrier
        GameObject barrier = GameObject.Find("TheBarrier");
        Vector3 Barrier_vn = barrier.GetComponent<Barrier>().normalDir;

        Vector3 barrier_pos = barrier.transform.position;
        // check whether the ball is in front of barrier

        Vector3 vt = transform.position - barrier_pos;
        if (Vector3.Dot(vt.normalized, Barrier_vn) > 0){
            // find Pon: projection location of the ball on the barrier
            float D = Vector3.Dot(barrier_pos, Barrier_vn);
            float d = Vector3.Dot(transform.position, Barrier_vn);
            Vector3 Pon = transform.position - (d-D-0.1f/2f) * Barrier_vn;
            // Debug.Log("Pon is " + Pon.ToString());
            // check whether the ball projection is in the circle
            if ((Pon - barrier_pos).magnitude < 6f) 
            {   
                
                if (shadow == null)
                {
                    // Debug.Log("create shadow child");
                    shadow = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    shadow.GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                    shadow.GetComponent<Renderer>().material.color = Color.black;
                    shadow.tag = "shadow";
                    
                }
                
                shadow.transform.up = Barrier_vn;
                shadow.transform.position = Pon;
                float multipler = 2f/(d-D);
                if (multipler > 1f){
                    shadow.transform.localScale = new Vector3(1f, 0.1f, 1f);
                } else if (multipler < 0.1f) {
                    shadow.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                } else {
                    shadow.transform.localScale = new Vector3(multipler, 0.1f, multipler);
                }
                if (shadow_line == null)
                {
                    shadow_line = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                    shadow_line.name = "ShadowLine";
                    shadow_line.GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                    shadow_line.GetComponent<Renderer>().material.color = Color.black;
                    shadow_line.AddComponent<Line>();
                    // PonBarrier.transform.position = Pon;
                    shadow_line.GetComponent<Line>().SetParam(shadow.transform, gameObject.transform);
                }

            } else {
                // for the existing shadow outside the circle, destroy
                Destroy(shadow_line);
                shadow_line = null;
                Destroy(shadow);
                shadow = null;

            }
        } else {
            // ball on the back of the barriei
            // all shadow should be destroyed
            foreach(Transform child in gameObject.transform)
            {
                if (child.gameObject.tag == "shadow")
                {
                    Destroy(child.gameObject);
                }
            }
        }
        
        
    }

    void CreateProjectBallLine(GameObject bigline)
    {
        if (bigline != null)
        {
            Vector3 po = bigline.gameObject.GetComponent<BigLine>().Po.position;
            float radius = bigline.gameObject.GetComponent<BigLine>().radius;
            Vector3 bigline_vn = bigline.transform.up;
            Vector3 v1 = transform.position - po;
            float d = Vector3.Dot(v1, bigline_vn);
            Vector3 pon = po + d * bigline_vn;
            
            // Vector3 h = distanceFromBigLine(bigline);
            GameObject pb = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            pb.GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            pb.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            pb.transform.position = pon + (transform.position - pon).normalized * radius / 2f;
            pb.AddComponent<ProjectBall>();
            pb.name = "ProjBall";
            pb.tag = "LineProjBall";
            
            GameObject pl = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            pl.GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            pl.GetComponent<Renderer>().material.color = Color.black;
            pl.name = "ProjLine";
            pl.AddComponent<Line>();

            Line lineScript = pl.GetComponent<Line>();
            lineScript.SetParam(pb.transform, gameObject.transform);

            ProjectBall pbScript = pb.GetComponent<ProjectBall>();
            pbScript.SetParam(gameObject.transform, bigline.transform, pl.transform);
     
            
        }
       
    }

   
    
    void OnCollide()
    {
        // find the normal vector
        Vector3 vn;
        float D;
        Vector3 pon = Vector3.zero;
        // Debug.Log(boundary.name);
        
        vn = barrier.GetComponent<Barrier>().normalDir;
        // margin = 0.1f / 2f; // sphere.transform.localscale.z / 2f
        D = Vector3.Dot(barrier.transform.position, vn);
        if (shadow_line != null | shadow != null)
        {
            if ((transform.position - shadow.transform.position).magnitude < 0.1f)
                {
                    pon = shadow.transform.position;
                    mDir = ReflectDir(D, vn, pon);
                    // Debug.DrawLine(pon, pon + 3f * mDir, Color.green, 1f);
                    
                }
        }
    }

    Vector3 ReflectDir(float D, Vector3 vn, Vector3 pon) 
    {
        float d = 1f;
        Vector3 p1 = pon - d * mDir;
        float h = Vector3.Dot(p1, vn) - D;
        Vector3 pm = pon + h * vn;
        Vector3 p2 = pm + (pm - p1);
        Vector3 new_dir = (p2 - pon).normalized;
        // Debug.Log("New Direction: " + new_dir);
        return new_dir;
    }

    public void SetSpeed(float s) 
    {
        speed = s; 
    //Debug.Log("speed to " + speed.ToString()); 
    }
    public void SetDirection(Vector3 d) {mDir = d;}
    public void SetMaxLife(float l) {maxLife = l;}


}
