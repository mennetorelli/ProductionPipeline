using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public string Type;
    public Material Material;
    public List<ResourceProperties> Properties;
    
    [HideInInspector]
    public Vector3 ResourceScale;

    private void Awake()
    {
        ResourceScale = transform.localScale;
        Properties = new List<ResourceProperties>();
    }

    public void InitializeResourceProperties(int low, int high) 
    {
        string id = "qwerty";

        System.Random rnd = new System.Random();
        object x = rnd.Next(low, high);

        Color color = new Color(rnd.Next(0, 255), rnd.Next(0, 255), rnd.Next(0, 255));
        transform.GetComponent<MeshRenderer>().material =  new Material(Material)
        {
            color = color
        };

        Properties.Add(new ResourceProperties(Type, id, x, color));
    }

    public class ResourceProperties
    {
        public string Type;
        public string ID;
        public object X;
        public Color Color;

        public ResourceProperties(string Type, string ID, object X, Color Color)
        {
            this.Type = Type;
            this.ID = ID;
            this.X = X;
            this.Color = Color;
        }
    }

    void OnMouseDown()
    {
        ShowDetailsManager.Instance.ResourceSelected(Type, Properties);
    }
}
