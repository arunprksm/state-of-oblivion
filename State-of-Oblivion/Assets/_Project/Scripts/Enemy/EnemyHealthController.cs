using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EnemyHealthController : MonoBehaviour
{
    [SerializeField] private Slider slider;
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
