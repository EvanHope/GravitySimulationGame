using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleBehavior : MonoBehaviour
{
    private Camera cam;


    private float time = 0.0f;
    private float interpolationPeriod = 5f;

    private Rigidbody2D rb;

    //collision particles
    public ParticleSystem ps;

    // Start is called before the first frame update
    void Start()
    {
        cam = FindObjectOfType<Camera>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        if (time >= interpolationPeriod)
        {
            if (Vector3.Distance(transform.position, cam.transform.position) > 50)
            {
                Destroy(gameObject);
            }
            time = 0.0f;
        }
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        if(transform.localScale.magnitude > col.transform.localScale.magnitude)
        {
            transform.localScale += col.transform.localScale;
            ps.startColor = col.transform.GetComponent<SpriteRenderer>().material.color;
            //foreach (ContactPoint2D contact in col.)
            //{
            //    Instantiate(ps, contact.point, Quaternion.FromToRotation(Vector3.back, contact.normal));
            //}
            Instantiate(ps, col.transform.position, Quaternion.FromToRotation(transform.position, col.transform.position));
            Destroy(col.gameObject);
            rb.mass += col.GetComponent<Rigidbody2D>().mass;
        }
    }

}
