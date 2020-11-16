using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buffer : PipelineComponent
{
    public int BufferInterval;
    public Transform BufferStartTransform;

    private List<GameObject> bufferedObjects = new List<GameObject>();
    private Vector3 BufferCurrentPosition;
    private bool isBuffering;

    protected override void Awake()
    {
        base.Awake();

        BufferCurrentPosition = new Vector3(BufferStartTransform.position.x, BufferStartTransform.position.y + BufferStartTransform.GetComponent<Collider>().bounds.extents.y, BufferStartTransform.position.z);
    }

    public override void Use(GameObject resource)
    {
        resource.GetComponent<Collider>().enabled = false;
        resource.GetComponent<Rigidbody>().useGravity = false;
        resource.transform.DOMove(new Vector3(BufferCurrentPosition.x, BufferCurrentPosition.y + resource.GetComponent<Collider>().bounds.extents.y, BufferCurrentPosition.z), 0.5f);
        bufferedObjects.Add(resource);
        BufferCurrentPosition = new Vector3(BufferCurrentPosition.x, BufferCurrentPosition.y + resource.GetComponent<Collider>().bounds.extents.y, BufferCurrentPosition.z);
    }

    void Update()
    {
        if (bufferedObjects.Count > 0 && !isBuffering)
        {
            isBuffering = true;
            StartCoroutine(Timer());
        }
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(BufferInterval);
        GameObject toRemove = bufferedObjects[bufferedObjects.Count - 1];
        toRemove.transform.DOMove(new Vector3(StartPosition.x, StartPosition.y + toRemove.GetComponent<Collider>().bounds.extents.y, StartPosition.z), 0.5f).OnComplete(() => GoToNext(toRemove));
        toRemove.GetComponent<Collider>().enabled = true;
        toRemove.GetComponent<Rigidbody>().useGravity = true;
        bufferedObjects.Remove(toRemove);
        isBuffering = false;
    }

}
