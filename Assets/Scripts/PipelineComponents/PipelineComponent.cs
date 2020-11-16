using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PipelineComponent : MonoBehaviour
{
    public List<GameObject> Next;

    public Transform StartTrasform;
    [HideInInspector]
    public Vector3 StartPosition;

    public abstract void Use(GameObject resource);

    protected virtual void Awake()
    {
        if (StartTrasform == null)
        {
            StartTrasform = transform;
        }
        Collider collider = StartTrasform.GetComponent<Collider>();
        StartPosition = new Vector3(collider.bounds.center.x, collider.bounds.center.y + collider.bounds.extents.y, collider.bounds.center.z);
    }

    public virtual void GoToNext(GameObject resource)
    {
        if (Next != null && Next.Count != 0)
        {
            System.Random rnd = new System.Random();
            int randomIndex = rnd.Next(0, Next.Count - 1);

            Vector3 temp = Next[randomIndex].GetComponent<PipelineComponent>().StartPosition;
            Vector3 targetPosition = new Vector3(temp.x, temp.y + resource.GetComponent<Collider>().bounds.extents.y, temp.z);
            resource.transform.DOMove(targetPosition, 1).OnComplete(() => Next[randomIndex].GetComponent<PipelineComponent>().Use(resource));
        }
    }

}
