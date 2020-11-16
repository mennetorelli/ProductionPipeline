using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class QualityAssurance : PipelineComponent
{
    public override void Use(GameObject resource)
    {
        GoToNext(resource);
    }

    protected abstract bool Check(GameObject resource);
}
