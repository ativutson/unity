using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingHeathBar : MonoBehaviour
{
    [SerializeField] private Slider Slider;

    public void updateHealthBar(float currentValue, float maxValue)
    {
        Slider.value = currentValue / maxValue;
    }
}
