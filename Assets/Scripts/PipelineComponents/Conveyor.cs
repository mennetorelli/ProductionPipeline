using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyor : PipelineComponent
{
    [Header("Component-specific mandatory parameters")]
    [Tooltip("The speed of the conveyor, in m/s")]
    public float Speed = 1;

    [Header("Component-specific optional parameters")]
    [Tooltip("The list of blocks of the conveyor that will be traversed in order by the resource. If left unspecified, it will be initialized as the sequence of child elements of the component")]
    public List<Transform> ConveyorBlocks;

    protected override void Awake()
    {
        // If the starting block has not been specified, take the first child
        if (StartTrasform == null)
        {
            StartTrasform = transform.GetChild(0);
        }

        base.Awake();

        // If a sequence of blocks of has not been specified, take all the blocks in the conveyor
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
        // Traverse all the conveyor blocks specified in ConveyorBlocks list
        Sequence sequence = DOTween.Sequence();
        for (int i = 1; i < ConveyorBlocks.Count; i ++)
        {
            Collider collider = ConveyorBlocks[i].GetComponent<Collider>();
            Vector3 position = new Vector3(collider.bounds.center.x, collider.bounds.center.y + collider.bounds.extents.y + resource.GetComponent<Collider>().bounds.extents.y, collider.bounds.center.z);
            float duration = Vector3.Distance(ConveyorBlocks[i - 1].transform.position, ConveyorBlocks[i].transform.position) / Speed;
            sequence.Append(resource.transform.DOMove(position, duration).SetEase(Ease.Linear));
        }
        sequence.OnComplete(() => GoToNext(resource));
    }

    protected override void FormatComponentDetails()
    {
        base.FormatComponentDetails();
        PipelineComponentProperties.Add("Conveyor speed: ", Speed);
        PipelineComponentProperties.Add("Number of blocks: ", ConveyorBlocks.Count);
    }
}
