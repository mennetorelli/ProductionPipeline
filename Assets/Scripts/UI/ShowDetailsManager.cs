using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Resource;

public class ShowDetailsManager : MonoBehaviour
{
    [Header("Component panel properties")]
    public GameObject ComponentPanel;
    public GameObject ComponentDetailsPrefab;
    public Transform ComponentDetailsContainer;

    [Header("Resource panel properties")]
    public GameObject ResourcePanel;
    public GameObject ResourceDetailsPrefab;
    public Transform ResourceDetailsContainer;
    public TextMeshProUGUI ResourcePanelText;

    private string _resourcePanelBaseText;

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

        _resourcePanelBaseText = ResourcePanelText.text;
    }

    public void PipelineComponentSelected(Dictionary<string, object> details)
    {
        for (int i = 0; i < ComponentDetailsContainer.childCount; i++)
        {
            Destroy(ComponentDetailsContainer.GetChild(i).gameObject);
        }

        ComponentPanel.SetActive(true);
        foreach (var item in details)
        {
            GameObject component = Instantiate(ComponentDetailsPrefab, ComponentDetailsContainer.transform);
            component.GetComponent<ComponentDetailsFiller>().Fill(item.Key, item.Value);
        }
    }

    public void ResourceSelected(GameObject resource)
    {
        ResourcePanelText.text = _resourcePanelBaseText + resource.name;
        for (int i = 1; i < ResourceDetailsContainer.childCount; i++)
        {
            Destroy(ResourceDetailsContainer.GetChild(i).gameObject);
        }

        ResourcePanel.SetActive(true);
        foreach (ResourceProperties prop in resource.GetComponent<Resource>().Properties)
        {
            GameObject res = Instantiate(ResourceDetailsPrefab, ResourceDetailsContainer.transform);
            res.GetComponent<ResourceDetailsFiller>().Fill(prop);
        }
    }

    public void DeactivatePanels()
    {
        for (int i = 0; i < transform.childCount; i ++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
