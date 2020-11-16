using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowSplitter : PipelineComponent
{
    public override void Use(GameObject resource)
    {
        GoToNext(resource);
    }
}
