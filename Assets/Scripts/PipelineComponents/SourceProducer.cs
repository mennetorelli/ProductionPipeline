using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Resource;

public class SourceProducer : PipelineComponent
{
    [Header("Component-specific mandatory parameters")]
    [Tooltip("The interval of time required to produce a resource")]
    public float ProductionInterval = 5;
    [Tooltip("The resource to be produced")]
    public GameObject Resource;
    [Tooltip("The lenght of the alphanumeric ID assigned to the resource")]
    public int IDLength = 6;
    [Tooltip("The lowest integer value that can be assigned to the resource. N.B. either assign a couple of NumericValueLow and NumericValueHigh parameters, or a set of char parameter in CharValueSet.")]
    public int NumericValueLow;
    [Tooltip("The highest integer value that can be assigned to the resource. N.B. either assign a couple of NumericValueLow and NumericValueHigh parameters, or a set of char parameter in CharValueSet.")]
    public int NumericValueHigh;
    [Tooltip("The set of char values that can be assigned to the resource. N.B. either assign a couple of NumericValueLow and NumericValueHigh parameters, or a set of char parameter in CharValueSet.")]
    public List<char> CharValuesSet;

    [Header("Visual Elements")]
    [Tooltip("Reference to the timer GameObject that is shown on top of the SourceProducer")]
    public GameObject FeedbackTimer;

    private bool _isProducing;

    void Update()
    {
        if (!_isProducing)
        {
            _isProducing = true;
            FeedbackTimer.GetComponent<Timer>().StartTimer(ProductionInterval);
            StartCoroutine(Timer(ProductionInterval, () =>
            {
                Use(Resource);
                _isProducing = false;
            }));
        }
    }

    public override void Use(GameObject resource)
    {
        GameObject generatedResource = GenerateResource(resource);
        generatedResource.transform.DOScale(new Vector3(0, 0, 0), 1f).From().OnComplete(() => GoToNext(generatedResource));
    }

    public GameObject GenerateResource(GameObject resource)
    {
        // Creation of the new resource 
        GameObject instantiatedResource = Instantiate(resource, StartPosition, Quaternion.identity);
        instantiatedResource.name = resource.name;

        // Assignment of random alphanumeric ID
        System.Random rnd = new System.Random();
        string id = new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", IDLength)
          .Select(s => s[rnd.Next(s.Length)]).ToArray());

        // Assignment of random value, which can be a char or an integer
        KeyValuePair<Type, string> value = new KeyValuePair<Type, string>();
        if (CharValuesSet.Count == 0)
        {
            value = new KeyValuePair<Type, string>(typeof(int), rnd.Next(NumericValueLow, NumericValueHigh >= NumericValueLow ? NumericValueHigh : NumericValueLow).ToString());
        }
        else
        {
            value = new KeyValuePair<Type, string>(typeof(char), CharValuesSet[rnd.Next(CharValuesSet.Count)].ToString());
        }

        // Assignment of random color
        Color color = new Color((float)rnd.NextDouble(), (float)rnd.NextDouble(), (float)rnd.NextDouble());
        instantiatedResource.GetComponent<Renderer>().material.color = color;

        // Adding all the properties in the Resource Properties list
        Resource resData = instantiatedResource.GetComponent<Resource>();
        resData.Properties.Add(new ResourceProperties(resource.name, id, value, color));

        return instantiatedResource;
    }

    protected override void FormatComponentDetails()
    {
        base.FormatComponentDetails();
        PipelineComponentProperties.Add("Produced resource: ", Resource.name);
        PipelineComponentProperties.Add("Production interval: ", ProductionInterval);
    }
}
