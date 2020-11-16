using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Assembler : PipelineComponent
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
        OutputResource.AddComponent<Resource>();
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

        

        if (resourcesToAssemble.Count == 0)
        {
            GoToNext(OutputResource);
            StartCoroutine(Timer2());
        }
    }

    protected abstract GameObject BuildResources(List<GameObject> resources, Vector3 startPosition);

    void Update()
    {
        if (!isAssembling)
        {
            isAssembling = true;
            StartCoroutine(Timer());
        }
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(AssemblerInterval);
        isAssembling = false;
    }

    IEnumerator Timer2()
    {
        yield return new WaitForSeconds(0.5f);
        CreateNewEmptyOutputResource();
    }
}
