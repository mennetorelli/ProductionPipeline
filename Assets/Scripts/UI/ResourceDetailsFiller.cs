using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Resource;

public class ResourceDetailsFiller : MonoBehaviour
{
    public TextMeshProUGUI Type;
    public TextMeshProUGUI ID;
    public TextMeshProUGUI Value;
    public TextMeshProUGUI Color;

    public void Fill(ResourceProperties resProperties)
    {
        Type.text = Type.text + resProperties.Type;
        ID.text = ID.text + resProperties.ID;
        Value.text = Value.text + resProperties.Value.Value;
        Color.color = resProperties.Color;
        Color.text = Color.text + resProperties.Color.ToString("F2");
    }
}
