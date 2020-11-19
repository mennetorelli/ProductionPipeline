using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyor : PipelineComponent
{
    [Header("Component-specific mandatory parameters")]
    [Tooltip("The speed of the conveyor, in m/s")]
    public float Speed;

    [Header("Component-specific optional parameters")]
    [Tooltip("The list of blocks of the conveyor that will be traversed in order by the resource. If left unspecified, it will be initialized as the sequence of child elements of the component")]
    public List<Transform> ConveyorBlocks;

    protected override void Awake()
    {
        if (StartTrasform == null)
        {
            StartTrasform = transform.GetChild(0);
        }

        base.Awake();

        if (ConveyorBlocks.Count == 0)
        {
            for (int i = 0; i < transform.childCount; i ++)
            {
                ConveyorBlocks.Add(transform.GetChild(i));
            }
        }
    }

    public override void Use(GameObject resource)
    {
        Sequence sequence = DOTween.Sequence();
        for (int i = 1; i < ConveyorBlocks.Count; i ++)
        {
            Collider collider = ConveyorBlocks[i].GetComponent<Collider>();
            Vector3 position = new Vector3(collider.bounds.center.x, collider.bounds.center.y + collider.bounds.extents.y + resource.GetComponent<Collider>().bounds.extents.y, collider.bounds.center.z);
            sequence.Append(resource.transform.DOMove(position, 1/Speed));
        }
        sequence.OnComplete(() => GoToNext(resource));
    }

    protected override void FormatDetails()
    {
        base.FormatDetails();
        PipelineComponentProperties.Add("Conveyor speed: ", Speed);
        PipelineComponentProperties.Add("Number of blocks: ", ConveyorBlocks.Count);
    }
}
