using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Resource : MonoBehaviour, ISelectable
{
    public List<ResourceProperties> Properties = new List<ResourceProperties>();
    
    [HideInInspector]
    public Vector3 ResourceScale;

    private void Awake()
    {
        ResourceScale = transform.localScale;
    }

    public class ResourceProperties
    {
        public string Type;
        public string ID;
        public KeyValuePair<Type, string> Value;
        public Color Color;

        public ResourceProperties(string Type, string ID, KeyValuePair<Type, string> Value, Color Color)
        {
            this.Type = Type;
            this.ID = ID;
            this.Value = Value;
            this.Color = Color;
        }
    }

    public void Selected()
    {
        ShowDetailsManager.Instance.ResourceSelected(gameObject);
    }
}
