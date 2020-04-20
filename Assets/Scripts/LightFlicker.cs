using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Experimental.Rendering.Universal;


public class LightFlicker : MonoBehaviour
{
    private Light2D lightSource;

    public float minRange = 1.0f;
    public float maxRange = 11.0f;

    public float flickerSpeed = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        lightSource = GetComponent<Light2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float time = Mathf.PingPong(Time.time * flickerSpeed, 1);
        lightSource.intensity = Mathf.Lerp(minRange, maxRange, time);

    }
}
