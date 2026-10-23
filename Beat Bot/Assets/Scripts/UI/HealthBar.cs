using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    // Slider variable that shows Player's current health
    public Slider healthSlider;

    // Update slider to match health amount
    public void SetSlider(float amount) 
    {
        healthSlider.value = amount;
    }

    public void SetSliderMax(float amount) {
        healthSlider.maxValue = amount;
        SetSlider(amount);
    }
}
