using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Resource;

public class ResourceComponentFiller : MonoBehaviour
{
    public TextMeshProUGUI Type;
    public TextMeshProUGUI Code;
    public TextMeshProUGUI ID;
    public TextMeshProUGUI Color;

    public void Fill(ResourceProperties resProperties)
    {
        Type.text = Type.text + resProperties.Type;
        Code.text = Code.text + resProperties.ID;
        ID.text = ID.text + resProperties.X;
        Color.text = Color.text + resProperties.Color;
    }
}
