using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SourceReceiver : PipelineComponent
{
    [Header("Visual Elements")]
    [Tooltip("Reference to the feedback GameObject that is shown on top of the SourceReceiver")]
    public GameObject FeedbackOk;

    public override void Use(GameObject resource)
    {
        FeedbackOk.SetActive(true);
        StartCoroutine(Timer(2f, () => FeedbackOk.SetActive(false)));
        resource.transform.DOScale(0, 1).OnComplete(() => Destroy(resource));
    }
}
