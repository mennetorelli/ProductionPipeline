using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Resource;

public class ShowDetailsManager : MonoBehaviour
{
    public GameObject PipelineComponent;
    public GameObject PipelineComponentPrefab;
    public Transform PipelineComponentContainer;
    public GameObject Resource;
    public Transform ResourceComponentContainer;
    public GameObject ResourceComponent;
    public TextMeshProUGUI ResourceComponentText;

    private string resourceComponentBaseText;

    public static ShowDetailsManager Instance
    {
        get;
        private set;
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        resourceComponentBaseText = ResourceComponentText.text;
    }

    public void PipelineComponentSelected(Dictionary<string, object> details)
    {
        for (int i = 0; i < PipelineComponentContainer.childCount; i++)
        {
            Destroy(PipelineComponentContainer.GetChild(i).gameObject);
        }

        PipelineComponent.SetActive(true);
        foreach (var detail in details)
        {
            GameObject component = Instantiate(PipelineComponentPrefab, PipelineComponentContainer.transform);
            component.GetComponent<PipelineComponentFiller>().Fill(detail.Key, detail.Value);
        }
    }

    public void ResourceSelected(string type, List<ResourceProperties> properties)
    {
        ResourceComponentText.text = resourceComponentBaseText + type;
        for (int i = 0; i < ResourceComponentContainer.childCount; i++)
        {
            Destroy(ResourceComponentContainer.GetChild(i).gameObject);
        }

        Resource.SetActive(true);
        foreach (ResourceProperties prop in properties)
        {
            GameObject res = Instantiate(ResourceComponent, ResourceComponentContainer.transform);
            res.GetComponent<ResourceComponentFiller>().Fill(prop);
        }
    }
}
