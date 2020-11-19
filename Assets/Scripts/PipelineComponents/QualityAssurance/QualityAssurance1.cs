﻿using DG.Tweening;
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
            FeedbackOk.SetActive(true);
            StartCoroutine(Timer(2f, () => FeedbackOk.SetActive(false)));
            Vector3 temp = Next[0].GetComponent<PipelineComponent>().StartPosition;
            Vector3 targetPosition = new Vector3(temp.x, temp.y + resource.GetComponent<Collider>().bounds.extents.y, temp.z);
            resource.transform.DOMove(targetPosition, 1).OnComplete(() => Next[0].GetComponent<PipelineComponent>().Use(resource));
        }
        else
        {
            FeedbackKo.SetActive(true);
            StartCoroutine(Timer(2f, () => FeedbackKo.SetActive(false)));
            Vector3 temp = Next[1].GetComponent<PipelineComponent>().StartPosition;
            Vector3 targetPosition = new Vector3(temp.x, temp.y + resource.GetComponent<Collider>().bounds.extents.y, temp.z);
            resource.transform.DOMove(targetPosition, 1).OnComplete(() => Next[1].GetComponent<PipelineComponent>().Use(resource));
        }
    }

    protected override void FormatDetails()
    {
        base.FormatDetails();
        PipelineComponentProperties.Add("Condition: ", "sum of values of resource > " + Treshold);
        PipelineComponentProperties.Add("Resource accepted goes to: ", Next[0].name);
        PipelineComponentProperties.Add("Resource discarded goes to: ", Next[1].name);
    }
}