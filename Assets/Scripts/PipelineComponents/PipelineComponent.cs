using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public abstract class PipelineComponent : MonoBehaviour, ISelectable
{
    [Header("General mandatory parameters")]
    [Tooltip("The list of next components in the pipeline")]
    public List<GameObject> Next;

    [Header("General optional parameters")]
    [Tooltip("The transform where the resource is moved when entering the component of the pipeline. If left unspecified, it will be the transform relative to the component")]
    public Transform StartTrasform;

    [HideInInspector]
    public Vector3 StartPosition;

    public Dictionary<string, object> PipelineComponentProperties;

    protected virtual void Awake()
    {
        // Ititialize StartTrasform if has not been specified by the user
        if (StartTrasform == null)
        {
            StartTrasform = transform;
        }

        // The position where the resource is moved is on of the upper face of the specified object (it must have a collider)
        Collider collider = StartTrasform.GetComponent<Collider>();
        StartPosition = new Vector3(collider.bounds.center.x, collider.bounds.center.y + collider.bounds.extents.y, collider.bounds.center.z);

        FormatComponentDetails();
    }

    /// <summary>
    /// Performes some computation with the resource
    /// </summary>
    /// <param name="resource"></param>
    public abstract void Use(GameObject resource);

    /// <summary>
    /// Sends the resources to the next pipeline component
    /// </summary>
    /// <param name="resource"></param>
    public virtual void GoToNext(GameObject resource)
    {
        if (Next != null && Next.Count != 0)
        {
            Vector3 temp = Next[0].GetComponent<PipelineComponent>().StartPosition;
            Vector3 targetPosition = new Vector3(temp.x, temp.y + resource.GetComponent<Collider>().bounds.extents.y, temp.z);
            resource.transform.DOMove(targetPosition, 1).OnComplete(() => Next[0].GetComponent<PipelineComponent>().Use(resource));
        }
    }

    /// <summary>
    /// Timer coroutine
    /// </summary>
    /// <param name="time"></param>
    /// <param name="call">The callback to be executed after the timer expires</param>
    /// <returns></returns>
    protected virtual IEnumerator Timer(float time, UnityAction call = null)
    {
        yield return new WaitForSeconds(time);
        call?.Invoke();
    }

    public void Selected()
    {
        ShowDetailsManager.Instance.PipelineComponentSelected(PipelineComponentProperties);
    }

    /// <summary>
    /// Format the details of the pipeline component to be shown on the UI
    /// </summary>
    protected virtual void FormatComponentDetails()
    {
        PipelineComponentProperties = new Dictionary<string, object>()
        {
            { "Component: ", gameObject.name },
            { "Next steps in pipeline: ", string.Join(", ", Next.Select(x => x.name)) }
        };
    }

}
