using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SourceReceiver : PipelineComponent
{
    public override void Use(GameObject resource)
    {
        resource.transform.DOScale(0, 1).OnComplete(() => Destroy(resource));
    }
}
