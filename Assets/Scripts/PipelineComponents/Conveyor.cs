using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyor : PipelineComponent
{
    public List<Transform> ConveyorBlocks;
    public int Speed;

    public override void Use(GameObject resource)
    {
        Sequence sequence = DOTween.Sequence();
        for (int i = 1; i < ConveyorBlocks.Count; i ++)
        {
            Collider collider = ConveyorBlocks[i].GetComponent<Collider>();
            Vector3 position = new Vector3(collider.bounds.center.x, collider.bounds.center.y + collider.bounds.extents.y + resource.GetComponent<Collider>().bounds.extents.y, collider.bounds.center.z);
            sequence.Append(resource.transform.DOMove(position, Speed));
        }
        sequence.OnComplete(() => GoToNext(resource));
    }
}
