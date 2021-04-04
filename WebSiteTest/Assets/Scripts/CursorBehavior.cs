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

    //Booleans for inputs
    private bool mouseDown = false;
    private bool mouseHeldDown = false;
    private bool mouseLifted = false;



    void Start()
    {
        //line
        xradiusStart = xradius;
        yradiusStart = yradius;
        lineCircle = gameObject.GetComponent<LineRenderer>();
        lr = line.GetComponent<LineRenderer>();

        lineCircle.positionCount = (segments + 1);
        lineCircle.useWorldSpace = true;

        mStart = m;

        cam = GetComponent<Camera>();
    }

    private void FixedUpdate()
    {
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        if (mouseDown == true)
        {
            SpawnParticle(mousePos);
            mouseDown = false;
            mouseHeldDown = true;
        }

        if(mouseHeldDown == true)
        {
            GrowParticle(mousePos);
        }

        if(mouseLifted == true)
        {
            mouseHeldDown = false;
            ReleaseParticle(mousePos);
            mouseLifted = false;
        }

    }

    private void SpawnParticle(Vector3 mousePosition)
    {
        startColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1);
        sp = Instantiate(spawningParticle, mousePosition, Quaternion.identity);
        rend = sp.GetComponent<SpriteRenderer>();
        mpstart = mousePosition;

        audioManager.PlayAudio(1);

        CI = Instantiate(cursorIndicator, mousePosition, Quaternion.identity);
        lr.enabled = true;
        lineCircle.enabled = true;
    }

    private void GrowParticle(Vector3 mousePosition)
    {
        if (sp.transform.localScale.x < maxScale)
            sp.transform.localScale = new Vector3(sp.transform.localScale.x + scaleSpeed, sp.transform.localScale.y + scaleSpeed, 1);


        ColorChanger();

        CreateCirclePoints();

        if (xradius < maxRadius)
        {
            xradius += radiusGrowthSpeed;
            yradius += radiusGrowthSpeed;
        }

        //Speed which mass grows at
        if (m < maxMass)
            m += mSpeed;

        if (Vector2.Distance(mousePosition, sp.transform.position) < xradius)
            CI.transform.position = mousePosition;
        else
        {
            Vector2 direction = mousePosition - sp.transform.position;
            direction = direction.normalized;
            Vector2 posV2 = new Vector2(sp.transform.position.x, sp.transform.position.y);
            CI.transform.position = (xradius * direction) + posV2;
        }

        lr.SetPosition(0, CI.transform.position);
        lr.SetPosition(1, sp.transform.position);
        //lr.endWidth = sp.transform.localScale.x - 0.5f;
    }

    private void ReleaseParticle(Vector3 mousePosition)
    {
        Destroy(CI);
        audioManager.StopAudio();
        //radius line
        lr.enabled = false;
        lineCircle.enabled = false;
        xradius = xradiusStart;
        yradius = yradiusStart;

        //mouse direction and distance
        Vector3 mouseDir = mousePosition - mpstart;
        mouseDir.z = 0;
        mouseDir = mouseDir.normalized;
        mouseDistance = Vector3.Distance(mousePosition, mpstart);

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


    //User inputs must be handled in update function to ensure no inputs are missed however physics updates must be handled in fixed update.
    //Using booleans is a simple work work around this problem.
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            mouseDown = true;
        if(Input.GetMouseButtonUp(0))
            mouseLifted = true;
        if(Input.GetMouseButtonDown(1))
            DeleteParticle();
    }

    private void DeleteParticle()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        if (hit)
            Destroy(hit.transform.gameObject);
    }

    void ColorChanger()
    {
        rend.material.color = startColor;
        if (t < 1)
        {
            t += Time.deltaTime / colorChangeTime;
        }
    }

    void CreateCirclePoints()
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
