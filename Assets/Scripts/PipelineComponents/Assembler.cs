using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Assembler : PipelineComponent
{
    public int AssemblerInterval;
    public List<GameObject> InputResources;
    public GameObject OutputResourcePrefab;
    public Transform OutputStartTransform;

    private bool isAssembling;
    private List<GameObject> resourcesToAssemble;
    private GameObject OutputResource;

    protected override void Awake()
    {
        base.Awake();
        CreateNewEmptyOutputResource();
    }

    void CreateNewEmptyOutputResource()
    {
        Vector3 finalPosition = new Vector3(OutputStartTransform.position.x, OutputStartTransform.position.y + OutputStartTransform.GetComponent<Collider>().bounds.extents.y, OutputStartTransform.position.z);
        OutputResource = Instantiate(OutputResourcePrefab, finalPosition, OutputStartTransform.rotation);
        resourcesToAssemble = OutputResource.GetComponentsInChildren<Resource>().Select(x => x.gameObject).ToList();
        OutputResource.AddComponent<Resource>().Type = OutputResourcePrefab.name;
    }

    public override void Use(GameObject resource)
    {
        GameObject res = resourcesToAssemble.Find(el => el.GetComponent<Resource>().Type == resource.GetComponent<Resource>().Type);
            
        if (res != null)
        {
            Destroy(resource.GetComponent<Collider>());
            Destroy(resource.GetComponent<Rigidbody>());
            resource.transform.position = res.transform.position;
            resource.transform.rotation = res.transform.rotation;
            resource.transform.localScale = res.transform.localScale;
            resource.transform.parent = res.transform.parent;
            res.transform.parent.GetComponent<Resource>().Properties.AddRange(resource.GetComponent<Resource>().Properties);
            Destroy(resource.GetComponent<Resource>());
            Destroy(res);
            resourcesToAssemble.Remove(res);
        }
        else
        {
            resource.transform.DOScale(0, 1).OnComplete(() => Destroy(resource));
        }

        if (resourcesToAssemble.Count == 0 && !isAssembling)
        {
            isAssembling = true;
            StartCoroutine(Timer(AssemblerInterval, () => 
            {
                GoToNext(OutputResource);
                StartCoroutine(Timer(0.5f, () => 
                {
                    CreateNewEmptyOutputResource();
                    isAssembling = false;
                }));
            }));
        }
    }

    protected override void PipelineComponentDetails()
    {
        base.PipelineComponentDetails();
        PipelineComponentProperties.Add("Input resources: ", string.Join(", ", InputResources.Select(x => x.name)));
        PipelineComponentProperties.Add("Output resource: ", OutputResourcePrefab.name);
        PipelineComponentProperties.Add("Assembling interval: ", AssemblerInterval);
    }
}
