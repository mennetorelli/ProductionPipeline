using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class QualityAssurance : PipelineComponent
{
    [Header("Visual Elements")]
    [Tooltip("Reference to the feedback for accepted resource that is shown on top of the QualityAssurance")]
    public GameObject FeedbackOk;
    [Tooltip("Reference to the feedback for rejected resource that is shown on top of the QualityAssurance")]
    public GameObject FeedbackKo;

    public override void Use(GameObject resource)
    {
        Check(resource);
    }

    /// <summary>
    /// Performes the quality check on the resource
    /// </summary>
    /// <param name="resource"></param>
    protected abstract void Check(GameObject resource);
}
