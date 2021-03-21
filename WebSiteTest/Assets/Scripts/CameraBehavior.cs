using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraBehavior : MonoBehaviour
{
    private Camera cam;
    public Slider camSlider;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        cam.orthographicSize = 25;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AdjustCamSize()
    {
        cam.orthographicSize = camSlider.value;
    }
}
