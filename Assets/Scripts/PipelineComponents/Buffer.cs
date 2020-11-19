using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buffer : PipelineComponent
{
    [Header("Component-specific mandatory parameters")]
    [Tooltip("The interval of time required before a buffered resource is sent back into the pipeline")]
    public int BufferInterval;

    [Header("Visual Elements")]
    [Tooltip("The transform where the resource is buffered")]
    public Transform BufferStartTransform;
    [Tooltip("Reference to the timer that is shown on top of the Buffer")]
    public GameObject FeedbackTimer;

    private List<GameObject> _bufferedResources = new List<GameObject>();
    private Vector3 _currentTopOfBufferPosition;
    private bool _isBuffering;

    protected override void Awake()
    {
        base.Awake();

        _currentTopOfBufferPosition = new Vector3(BufferStartTransform.position.x, BufferStartTransform.position.y + BufferStartTransform.GetComponent<Collider>().bounds.extents.y, BufferStartTransform.position.z);
    }

    public override void Use(GameObject resource)
    {
        Sequence sequence = DOTween.Sequence();
        _currentTopOfBufferPosition = new Vector3(_currentTopOfBufferPosition.x, _currentTopOfBufferPosition.y + resource.GetComponent<Collider>().bounds.extents.y * 2f, _currentTopOfBufferPosition.z);
        sequence.Append(resource.transform.DOMoveY(_currentTopOfBufferPosition.y + resource.GetComponent<Collider>().bounds.extents.y, 0.2f));
        sequence.Append(resource.transform.DOMove(new Vector3(_currentTopOfBufferPosition.x, _currentTopOfBufferPosition.y + resource.GetComponent<Collider>().bounds.extents.y, _currentTopOfBufferPosition.z), 0.2f));
        _bufferedResources.Add(resource);
    }

    void Update()
    {
        if (_bufferedResources.Count > 0 && !_isBuffering)
        {
            _isBuffering = true;

            FeedbackTimer.GetComponent<Timer>().StartTimer(BufferInterval);
            StartCoroutine(Timer(BufferInterval, () => 
            {
                GameObject toRemove = _bufferedResources[_bufferedResources.Count - 1];

                Sequence sequence = DOTween.Sequence();
                sequence.Append(toRemove.transform.DOMove(new Vector3(StartPosition.x, toRemove.transform.position.y, StartPosition.z), 0.2f));
                sequence.Append(toRemove.transform.DOMove(new Vector3(StartPosition.x, StartPosition.y + toRemove.GetComponent<Collider>().bounds.extents.y, StartPosition.z), 0.5f));
                sequence.OnComplete(() => GoToNext(toRemove));

                _currentTopOfBufferPosition = new Vector3(_currentTopOfBufferPosition.x, _currentTopOfBufferPosition.y - toRemove.GetComponent<Collider>().bounds.extents.y * 2f, _currentTopOfBufferPosition.z);
                _bufferedResources.Remove(toRemove);
                _isBuffering = false;
            }));
        }
    }

    protected override void FormatDetails()
    { 
        base.FormatDetails();
        PipelineComponentProperties.Add("Buffer interval: ", BufferInterval);
    }
}
