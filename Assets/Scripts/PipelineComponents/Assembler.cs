using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Assembler : PipelineComponent
{
    [Header("Component-specific mandatory parameters")]
    [Tooltip("The interval of assembling a resource")]
    public int AssemblingInterval = 5;
    [Tooltip("The list of input resources required by the assembler")]
    public List<GameObject> InputResources;
    [Tooltip("The output resource produced by the assembler")]
    public GameObject OutputResource;

    [Header("Visual Elements")]
    [Tooltip("The transform where the (empty) output resource is spawned")]
    public Transform OutputResourceTransform;
    [Tooltip("Reference to the timer that is shown on top of the Assembler")]
    public GameObject FeedbackTimer;

    private bool _isAssembling;
    private List<GameObject> _resourcesToAssemble;
    private GameObject _currentOutputResource;

    protected override void Awake()
    {
        base.Awake();
        CreateNewEmptyOutputResource();
    }

    /// <summary>
    /// Creates a new empty output resource in OutputStartTransform's position
    /// </summary>
    void CreateNewEmptyOutputResource()
    {
        Vector3 finalPosition = new Vector3(OutputResourceTransform.position.x, OutputResourceTransform.position.y + OutputResourceTransform.GetComponent<Collider>().bounds.extents.y, OutputResourceTransform.position.z);
        _currentOutputResource = Instantiate(OutputResource, finalPosition, OutputResourceTransform.rotation);
        _resourcesToAssemble = _currentOutputResource.GetComponentsInChildren<Transform>().Where(x => x.tag == "EmptyResource").Select(x => x.gameObject).ToList();
        _currentOutputResource.name = OutputResource.name;
    }

    public override void Use(GameObject resource)
    {
        // Accepts the resource arriving to the assembler if it's correct, else discard the resource
        GameObject emptyResource = _resourcesToAssemble.Find(el => el.name == resource.name);
        if (emptyResource != null)
        {
            Destroy(resource.GetComponent<Collider>());
            Destroy(resource.GetComponent<Rigidbody>());
            resource.transform.position = emptyResource.transform.position;
            resource.transform.rotation = emptyResource.transform.rotation;
            resource.transform.localScale = emptyResource.transform.localScale;
            resource.transform.parent = emptyResource.transform.parent;
            emptyResource.transform.parent.GetComponent<Resource>().Properties.AddRange(resource.GetComponent<Resource>().Properties);
            Destroy(resource.GetComponent<Resource>());
            Destroy(emptyResource);
            _resourcesToAssemble.Remove(emptyResource);
        }
        else
        {
            resource.transform.DOScale(0, 1).OnComplete(() => Destroy(resource));
        }

        // If all the input resources have arrived, starts assembling
        if (_resourcesToAssemble.Count == 0 && !_isAssembling)
        {
            _isAssembling = true;
            FeedbackTimer.GetComponent<Timer>().StartTimer(AssemblingInterval);
            StartCoroutine(Timer(AssemblingInterval, () => 
            {
                GoToNext(_currentOutputResource);
                StartCoroutine(Timer(0.5f, () => 
                {
                    CreateNewEmptyOutputResource();
                    _isAssembling = false;
                }));
            }));
        }
    }

    protected override void FormatDetails()
    {
        base.FormatDetails();
        PipelineComponentProperties.Add("Input resources: ", string.Join(", ", InputResources.Select(x => x.name)));
        PipelineComponentProperties.Add("Output resource: ", OutputResource.name);
        PipelineComponentProperties.Add("Assembling interval: ", AssemblingInterval);
    }
}
