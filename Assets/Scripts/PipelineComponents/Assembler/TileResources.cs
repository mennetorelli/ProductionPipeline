using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileResources : Assembler
{
    protected override GameObject BuildResources(List<GameObject> resources, Vector3 startPosition)
    {
        
        // Debug.Log(resources.Count);

        GameObject assembledResource = Instantiate(new GameObject("BaseBase"), StartTrasform.position, StartTrasform.rotation);
        resources[0].transform.parent = assembledResource.transform;
        resources[1].transform.parent = assembledResource.transform;
        resources[0].transform.position = new Vector3(resources[0].GetComponent<Collider>().bounds.extents.x, 0, 0);
        resources[1].transform.position = new Vector3(-resources[1].GetComponent<Collider>().bounds.extents.x, 0, 0);
        return assembledResource;
    }
}
