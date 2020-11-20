using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QualityAssurance1 : QualityAssuranceBase
{
    public int Treshold;

    protected override void Check(GameObject resource)
    {
        if (resource.GetComponent<Resource>().Properties.Select(x => x.Value).Aggregate(0, (acc, x) => acc + int.Parse(x.Value)) > Treshold)
        {
            // The resource satisfies the condition, go to Next[0]
            FeedbackOk.SetActive(true);
            StartCoroutine(Timer(2f, () => FeedbackOk.SetActive(false)));
            Vector3 temp = Next[0].GetComponent<PipelineComponent>().StartPosition;
            Vector3 targetPosition = new Vector3(temp.x, temp.y + resource.GetComponent<Collider>().bounds.extents.y, temp.z);
            resource.transform.DOMove(targetPosition, 1).OnComplete(() => Next[0].GetComponent<PipelineComponent>().Use(resource));
        }
        else
        {
            // The resource does not satisfy the condition, go to Next[1]
            FeedbackKo.SetActive(true);
            StartCoroutine(Timer(2f, () => FeedbackKo.SetActive(false)));
            Vector3 temp = Next[1].GetComponent<PipelineComponent>().StartPosition;
            Vector3 targetPosition = new Vector3(temp.x, temp.y + resource.GetComponent<Collider>().bounds.extents.y, temp.z);
            resource.transform.DOMove(targetPosition, 1).OnComplete(() => Next[1].GetComponent<PipelineComponent>().Use(resource));
        }
    }

    protected override void FormatComponentDetails()
    {
        base.FormatComponentDetails();
        PipelineComponentProperties.Add("Condition: ", "sum of values of resource > " + Treshold);
        PipelineComponentProperties.Add("Accepted resources accepted go to: ", Next[0].name);
        PipelineComponentProperties.Add("Discarded resources go to: ", Next[1].name);
    }
}
