using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackResources : Assembler
{
    protected override GameObject BuildResources(List<GameObject> resources, Vector3 startPosition)
    {
        GameObject assembledResource = new GameObject();
        resources[0].transform.parent = assembledResource.transform;
        resources[1].transform.parent = assembledResource.transform;
        resources[0].transform.position = new Vector3(0, 0, 0);
        resources[1].transform.position = new Vector3(0, resources[0].GetComponent<Collider>().bounds.extents.y, 0);
        return assembledResource;
    }
}
