using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PipelineComponent : MonoBehaviour
{
    public Transform StartTrasform;
    public List<GameObject> Next;

    public abstract void Use(GameObject resource);
}
