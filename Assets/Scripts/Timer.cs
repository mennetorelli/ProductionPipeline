using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText;

    private float _timeLeft = 0;
    private bool _counting;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (_counting)
        {
            _timeLeft -= Time.deltaTime;
            timerText.text = (_timeLeft).ToString("0");
            if (_timeLeft < 0)
            {
                _counting = false;
                gameObject.SetActive(false);
            }
        }
        
    }

    public void StartTimer(float timer)
    {
        _timeLeft = timer;
        gameObject.SetActive(true);
        _counting = true;
    }

}
