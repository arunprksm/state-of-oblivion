using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HealthController : MonoBehaviour
{
    [SerializeField] private Slider slider;

    private static HealthController instance;
    internal static HealthController Instance { get { return instance; } }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
    internal void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }
    internal void SetHealth(int health)
    {
        slider.value = health;
    }
}