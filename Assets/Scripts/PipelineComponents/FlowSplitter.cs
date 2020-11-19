using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowSplitter : PipelineComponent
{
    [Header("Component-specific mandatory parameters")]
    [Tooltip("The list of weights associated to each path of the flow splitter")]
    public List<float> Weighs;

    public GameObject FeedbackArrow;

    public override void Use(GameObject resource)
    {
        GoToNext(resource);
    }

    public override void GoToNext(GameObject resource)
    {
        if (Next != null && Next.Count != 0)
        {
            System.Random rnd = new System.Random();
            double random = rnd.NextDouble();
            float treshold = 0;
            int selectedIndex = 0;
            for (int i = 0; i < Weighs.Count; i ++)
            {
                if (random >= treshold && random < treshold + Weighs[i])
                {
                    selectedIndex = i;
                }
                treshold += Weighs[i];
            }

            Vector3 temp = Next[selectedIndex].GetComponent<PipelineComponent>().StartPosition;
            Vector3 targetPosition = new Vector3(temp.x, temp.y + resource.GetComponent<Collider>().bounds.extents.y, temp.z);
            resource.transform.DOMove(targetPosition, 1).OnComplete(() => Next[selectedIndex].GetComponent<PipelineComponent>().Use(resource));
        }
    }

    protected override void FormatDetails()
    {
        base.FormatDetails();
        PipelineComponentProperties.Add("Weighs: ", string.Join(", ", Weighs));
    }
}
