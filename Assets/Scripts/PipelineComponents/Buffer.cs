using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buffer : PipelineComponent
{
    [Header("Component-specific mandatory parameters")]
    [Tooltip("The interval of time required before a buffered resource is sent back into the pipeline")]
    public int BufferInterval = 10;

    [Header("Visual Elements")]
    [Tooltip("The transform where the resource is buffered")]
    public Transform BufferedResourcesTransform;
    [Tooltip("Reference to the timer that is shown on top of the Buffer")]
    public GameObject FeedbackTimer;

    private List<GameObject> _bufferedResources = new List<GameObject>();
    private Vector3 _currentTopOfBufferPosition;
    private bool _isBuffering;

    protected override void Awake()
    {
        base.Awake();

        _currentTopOfBufferPosition = new Vector3(BufferedResourcesTransform.position.x, BufferedResourcesTransform.position.y + BufferedResourcesTransform.GetComponent<Collider>().bounds.extents.y, BufferedResourcesTransform.position.z);
    }

    public override void Use(GameObject resource)
    {
        // Add the resource to the buffer
        Sequence sequence = DOTween.Sequence();
        _currentTopOfBufferPosition = new Vector3(_currentTopOfBufferPosition.x, _currentTopOfBufferPosition.y + resource.GetComponent<Collider>().bounds.extents.y * 2f, _currentTopOfBufferPosition.z);
        sequence.Append(resource.transform.DOMoveY(_currentTopOfBufferPosition.y, 0.2f));
        sequence.Append(resource.transform.DOMove(_currentTopOfBufferPosition, 0.2f));
        _bufferedResources.Add(resource);
    }

    void Update()
    {
        // If the bufer is not empty, every BufferInterval seconds remove one resource and send it back to the pipeline
        if (_bufferedResources.Count > 0 && !_isBuffering)
        {
            _isBuffering = true;

            FeedbackTimer.GetComponent<Timer>().StartTimer(BufferInterval);
            StartCoroutine(Timer(BufferInterval, () => 
            {
                GameObject toRemove = _bufferedResources[_bufferedResources.Count - 1];

                Sequence sequence = DOTween.Sequence();
                sequence.Append(toRemove.transform.DOMove(new Vector3(StartPosition.x, _currentTopOfBufferPosition.y, StartPosition.z), 0.2f));
                sequence.Append(toRemove.transform.DOMoveY(StartPosition.y + toRemove.GetComponent<Collider>().bounds.extents.y, 0.2f));
                sequence.OnComplete(() => GoToNext(toRemove));

                _currentTopOfBufferPosition = new Vector3(_currentTopOfBufferPosition.x, _currentTopOfBufferPosition.y - toRemove.GetComponent<Collider>().bounds.extents.y * 2f, _currentTopOfBufferPosition.z);
                _bufferedResources.Remove(toRemove);
                _isBuffering = false;
            }));
        }
    }

    protected override void FormatComponentDetails()
    { 
        base.FormatComponentDetails();
        PipelineComponentProperties.Add("Buffer interval: ", BufferInterval);
    }
}
