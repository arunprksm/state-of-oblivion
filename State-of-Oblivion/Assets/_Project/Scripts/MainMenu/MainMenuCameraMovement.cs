using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCameraMovement : MonoBehaviour
{
    [SerializeField] private float mainCameraSpeed;

    private void Update()
    {
        transform.Translate(Vector2.right * Time.deltaTime * mainCameraSpeed);
    }
}
