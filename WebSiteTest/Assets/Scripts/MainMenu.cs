using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject particlesHolder;
    public GameObject activeParticle;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Spawner());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LinkedIn()
    {
        Application.OpenURL("https://www.linkedin.com/in/evan-hope-5b5663197/");
    }
    public void Instagram()
    {
        Application.OpenURL("https://www.instagram.com/evanhopedesigns/");
    }

    public void Continue()
    {
        SceneManager.LoadScene("SandBox");
    }

    IEnumerator Spawner()
    {
        while(true)
        {
            int amount = Random.Range(3, 10);
            for (int i = 0; i < amount; i++)
            {
                float spawnY = Random.Range
                    (Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).y, Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)).y);
                float spawnX = Random.Range
                    (Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x, Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x);

                Vector2 spawnPosition = new Vector2(spawnX, spawnY);
                GameObject ap = (GameObject)Instantiate(activeParticle, spawnPosition, Quaternion.identity, particlesHolder.transform);
                float scale = Random.Range(2, 10);
                ap.transform.localScale = new Vector3(scale, scale, 1);

                Rigidbody2D aprb = ap.GetComponent<Rigidbody2D>();
                aprb.mass = scale * 1000;

                SpriteRenderer rend = ap.GetComponent<SpriteRenderer>();
                Color startColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1);
                rend.material.color = startColor;
                aprb.AddForce(new Vector2(Random.Range(15000, 25000), Random.Range(15000, 25000)));
            }
            yield return new WaitForSeconds(5);
            foreach(Transform o in particlesHolder.transform)
            {
                Destroy(o.gameObject);
            }
        }
    }
}
