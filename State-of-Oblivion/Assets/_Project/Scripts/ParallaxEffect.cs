using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    private float length, startPos;
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private float parallaxEffect;


    private void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void FixedUpdate()
    {
        float temp = (mainCamera.transform.position.x * (1 - parallaxEffect));
        float distance = mainCamera.transform.position.x * parallaxEffect;
        transform.position = new Vector3(startPos + distance, transform.position.y,transform.position.z);

        if (temp > startPos + length)
        {
            startPos += length;
        }
        else if (temp < startPos - length)
        {
            startPos -= length;
        }
    }
}
