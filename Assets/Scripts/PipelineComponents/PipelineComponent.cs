using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public abstract class PipelineComponent : MonoBehaviour, ISelectable
{
    public List<GameObject> Next;

    public Transform StartTrasform;
    [HideInInspector]
    public Vector3 StartPosition;

    public Dictionary<string, object> PipelineComponentProperties;

    public abstract void Use(GameObject resource);

    protected virtual void Awake()
    {
        if (StartTrasform == null)
        {
            StartTrasform = transform;
        }
        Collider collider = StartTrasform.GetComponent<Collider>();
        StartPosition = new Vector3(collider.bounds.center.x, collider.bounds.center.y + collider.bounds.extents.y, collider.bounds.center.z);

        PipelineComponentDetails();
    }

    public virtual void GoToNext(GameObject resource)
    {
        if (Next != null && Next.Count != 0)
        {
            System.Random rnd = new System.Random();
            int randomIndex = rnd.Next(0, Next.Count);

            Vector3 temp = Next[randomIndex].GetComponent<PipelineComponent>().StartPosition;
            Vector3 targetPosition = new Vector3(temp.x, temp.y + resource.GetComponent<Collider>().bounds.extents.y, temp.z);
            resource.transform.DOMove(targetPosition, 1).OnComplete(() => Next[randomIndex].GetComponent<PipelineComponent>().Use(resource));
        }
    }

    protected virtual IEnumerator Timer(float time, UnityAction call = null)
    {
        yield return new WaitForSeconds(time);
        call?.Invoke();
    }

    public void Selected()
    {
        ShowDetailsManager.Instance.PipelineComponentSelected(PipelineComponentProperties);
    }

    protected virtual void PipelineComponentDetails()
    {
        PipelineComponentProperties = new Dictionary<string, object>()
        {
            { "Component: ", gameObject.name },
            { "Next steps in pipeline: ", string.Join(", ", Next.Select(x => x.name)) }
        };
    }

}
