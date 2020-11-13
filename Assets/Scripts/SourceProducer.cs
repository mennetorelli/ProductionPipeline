using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SourceProducer : MonoBehaviour, IPipelineComponent
{
    public float Interval;
    public GameObject Resource;
    public GameObject Next;

    private Vector3 spawnPosition;

    // Start is called before the first frame update
    void Start()
    {
        Collider collider = GetComponent<Collider>();
        spawnPosition = new Vector3(collider.bounds.center.x, collider.bounds.center.y + collider.bounds.extents.y / 2, collider.bounds.center.z);

        Use(Resource);
    }

    public void Use(GameObject resource)
    {
        GameObject instantiatedResource = Instantiate(resource, spawnPosition, Quaternion.identity);
        instantiatedResource.transform.DOScale(new Vector3(0, 0, 0), 1f).From().OnComplete(() => Next.GetComponent<IPipelineComponent>().Use(instantiatedResource));
    }

    // Update is called once per frame
    void Update()
    {

    }
}
