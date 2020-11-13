using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyor : MonoBehaviour, IPipelineComponent
{
    public List<Transform> ConveyorBlocks;
    public int Speed;
    public GameObject Next;

    public void Use(GameObject resource)
    {
        Sequence sequence = DOTween.Sequence();
        foreach (Transform block in ConveyorBlocks)
        {
            Collider collider = block.GetComponent<Collider>();
            Vector3 position = new Vector3(collider.bounds.center.x, collider.bounds.center.y + collider.bounds.extents.y + resource.GetComponent<Collider>().bounds.extents.y, collider.bounds.center.z);
            sequence.Append(resource.transform.DOMove(position, Speed));
        }
    }
}
