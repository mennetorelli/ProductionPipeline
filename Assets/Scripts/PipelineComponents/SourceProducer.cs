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

    // Start is called before the first frame update
    void Start()
    {
        Use(Resource);
    }

    void Update()
    {
        if (!isProducing)
        {
            isProducing = true;
            StartCoroutine(Timer());
        }
    }

    public override void Use(GameObject resource)
    {
        GameObject instantiatedResource = Instantiate(resource, StartPosition, Quaternion.identity);
        instantiatedResource.GetComponent<Resource>().InitializeResourceProperties(LowRange, HighRange);
        instantiatedResource.transform.DOScale(new Vector3(0, 0, 0), 1f).From().OnComplete(() => GoToNext(instantiatedResource));
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(ProductionInterval);
        Use(Resource);
        isProducing = false;
    }
}
