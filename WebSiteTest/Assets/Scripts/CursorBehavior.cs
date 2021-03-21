using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorBehavior : MonoBehaviour
{
    public AudioManager audioManager;


    private Camera cam;
    public float scaleSpeed;
    public float mSpeed;
    public GameObject spawningParticle;
    public GameObject activeParticle;
    private GameObject sp;
    private GameObject ap;
    public float m;
    private float mStart;
    public float maxScale;
    public float maxMass;


    //force
    private Vector3 mpstart;
    private Vector3 mpend;
    private Vector3 mouseDirection;
    private float mouseDistance;
    public float Force;

    //UI
    [Range(0, 50)]
    public int segments = 50;
    [Range(0, 5)]
    public float xradius = 5;
    [Range(0, 5)]
    public float yradius = 5;
    LineRenderer lineCircle;

    public GameObject cursorIndicator;
    private GameObject CI;

    private float xradiusStart;
    private float yradiusStart;

    public float radiusGrowthSpeed;
    public float maxRadius;

    public GameObject line;
    private LineRenderer lr;

    public AnimationCurve AC;

    //Color
    private SpriteRenderer rend;
    public float colorChangeTime = 1.5f;
    private float t = 0;
    private Color startColor;
    private Color endColor;





    // Start is called before the first frame update
    void Start()
    {
        //line
        xradiusStart = xradius;
        yradiusStart = yradius;
        lineCircle = gameObject.GetComponent<LineRenderer>();
        lr = line.GetComponent<LineRenderer>();

        lineCircle.positionCount = (segments + 1);
        lineCircle.useWorldSpace = true;
        //CreatePoints();

        mStart = m;

        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = cam.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        if (Input.GetMouseButtonDown(0))
        {
            startColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1);
            //endColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1);
            sp = Instantiate(spawningParticle, pos, Quaternion.identity);
            rend = sp.GetComponent<SpriteRenderer>();
            mpstart = pos;

            audioManager.PlayAudio(1);

            CI = Instantiate(cursorIndicator, pos, Quaternion.identity);
            lr.enabled = true;
            lineCircle.enabled = true;
        }

        if(Input.GetMouseButton(0))
        {
            if(sp.transform.localScale.x < maxScale)
            sp.transform.localScale = new Vector3(sp.transform.localScale.x + scaleSpeed, sp.transform.localScale.y + scaleSpeed, 1);
            ColorChangerr();

            
            CreatePoints();
            
            if(xradius < maxRadius)
            {
                xradius += radiusGrowthSpeed;
                yradius += radiusGrowthSpeed;
            }

            //Speed which mass grows at
            if(m < maxMass)
            m += mSpeed;

            if (Vector2.Distance(pos,sp.transform.position) < xradius)
                CI.transform.position = pos;
            else
            {
                Vector2 direction = pos - sp.transform.position;
                direction = direction.normalized;
                Vector2 posV2 = new Vector2(sp.transform.position.x, sp.transform.position.y);
                CI.transform.position = (xradius * direction) + posV2;
            }
            
            lr.SetPosition(0, CI.transform.position);
            lr.SetPosition(1, sp.transform.position);
            //lr.endWidth = sp.transform.localScale.x - 0.5f;
        }


        if(Input.GetMouseButtonUp(0))
        {
            Destroy(CI);
            audioManager.StopAudio();
            //radius line
            lr.enabled = false;
            lineCircle.enabled = false;
            xradius = xradiusStart;
            yradius = yradiusStart;

            //mouse direction and distance
            Vector3 mouseDir = pos - mpstart;
            mouseDir.z = 0;
            mouseDir = mouseDir.normalized;
            mouseDistance = Vector3.Distance(pos, mpstart);

            //Checks if mouse distance is far enough for certain cases
            if (mouseDistance > xradius)
            {
                mouseDistance = xradius;
                audioManager.PlayAudio(0);
            }
            else if (mouseDistance < .5)
                mouseDistance = 0;
            else
                audioManager.PlayAudio(0);

            //Spawns active particle and gets components
            ap = Instantiate(activeParticle, mpstart, Quaternion.identity);
            ap.transform.localScale = sp.transform.localScale;
            ap.GetComponent<SpriteRenderer>().material.color = rend.material.color;
            Rigidbody2D apRB = ap.GetComponent<Rigidbody2D>();

            //deletes fake particle and applies mass and force to active particle
            apRB.mass = m;
            Destroy(sp);
            apRB.AddForce((-1 * mouseDir * Force) * (mouseDistance * 100));
            t = 0;
            m = mStart;
        }


        if(Input.GetMouseButtonDown(1))
        {
            
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if(hit)
                Destroy(hit.transform.gameObject);
        }

    }

    void ColorChangerr()
    {
        //rend.material.color = Color.Lerp(startColor, Color.white, t);
        rend.material.color = startColor;
        if (t < 1)
        {
            t += Time.deltaTime / colorChangeTime;
        }
    }


    void CreatePoints()
    {
        float x;
        float y;

        float angle = 20f;

        for (int i = 0; i < (segments + 1); i++)
        {
            x = (Mathf.Sin(Mathf.Deg2Rad * angle) * xradius) + mpstart.x;
            y = (Mathf.Cos(Mathf.Deg2Rad * angle) * yradius) + mpstart.y;

            lineCircle.SetPosition(i, new Vector3(x, y, 0));

            angle += (360f / segments);
        }
    }

}
