using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SourceProducer : PipelineComponent
{
    public float Interval;
    public GameObject Resource;

    private Vector3 spawnPosition;

    // Start is called before the first frame update
    void Start()
    {
        Collider collider = GetComponent<Collider>();
        spawnPosition = new Vector3(collider.bounds.center.x, collider.bounds.center.y + collider.bounds.extents.y / 2, collider.bounds.center.z);

        Use(Resource);
    }

    public override void Use(GameObject resource)
    {
        GameObject instantiatedResource = Instantiate(resource, spawnPosition, Quaternion.identity);
        instantiatedResource.transform.DOScale(new Vector3(0, 0, 0), 1f).From().OnComplete(() => Next[0].GetComponent<PipelineComponent>().Use(instantiatedResource));
    }
}
