using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using static Resource;

public class PipelineComponentFiller : MonoBehaviour
{
    public TextMeshProUGUI PipelineComponentProperty;

    public void Fill(string key, object value)
    {
        PipelineComponentProperty.text = key + " " + value;
    }

}
