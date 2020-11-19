using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SimulationOptions : MonoBehaviour
{
    public TextMeshProUGUI Text;
    public Slider Slider;
    public Toggle Toggle;
    public List<Sprite> ToggleImages;

    private float _timeScale;
    private string _baseText;

    private void Awake()
    {
        float value = Slider.value;
        _timeScale = value;
        _baseText = Text.text;
        Text.text = _baseText + (int)(value * 100) + "%";
    }

    public void UpdateTimeScale()
    {
        float value = Slider.value;
        _timeScale = value;
        Time.timeScale = Toggle.isOn ? value : 0;
        Text.text = _baseText + (int) (value * 100) + "%";
    }

    public void PauseSimulation()
    {
        Time.timeScale = Toggle.isOn ? _timeScale : 0;
        Toggle.image.sprite = Toggle.isOn ? ToggleImages[0] : ToggleImages[1];
    }
}
