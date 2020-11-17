using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SourceProducer : PipelineComponent
{
    public float ProductionInterval;
    public GameObject Resource;
    public int LowRange;
    public int HighRange;

    private bool isProducing;

    void Update()
    {
        if (!isProducing)
        {
            isProducing = true;
            StartCoroutine(Timer(ProductionInterval, () =>
            {
                Use(Resource);
                isProducing = false;
            }));
        }
    }

    public override void Use(GameObject resource)
    {
        GameObject instantiatedResource = Instantiate(resource, StartPosition, Quaternion.identity);
        instantiatedResource.GetComponent<Resource>().InitializeResourceProperties(LowRange, HighRange);
        instantiatedResource.transform.DOScale(new Vector3(0, 0, 0), 1f).From().OnComplete(() => GoToNext(instantiatedResource));
    }

    protected override void PipelineComponentDetails()
    {
        base.PipelineComponentDetails();
        PipelineComponentProperties.Add("Produced resource: ", Resource.GetComponent<Resource>().Type);
        PipelineComponentProperties.Add("Production interval: ", ProductionInterval);
    }
}
