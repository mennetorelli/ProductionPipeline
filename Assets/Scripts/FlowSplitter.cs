using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowSplitter : PipelineComponent
{
    public override void Use(GameObject resource)
    {
        Collider collider = GetComponent<Collider>();
        Vector3 position = new Vector3(collider.bounds.center.x, collider.bounds.center.y + collider.bounds.extents.y + resource.GetComponent<Collider>().bounds.extents.y, collider.bounds.center.z);
        resource.transform.DOMove(position, 1f).OnComplete(() =>
        {
            System.Random rnd = new System.Random();
            GameObject next = Next[rnd.Next(0, Next.Count - 1)];
            next.GetComponent<PipelineComponent>().Use(resource);
        });
    }
}
