using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlotHandler : MonoBehaviour
{
    private List<GameObject> _plots;

    void Start()
    {
        _plots = new List<GameObject>();
        int childCount = transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform childTransform = transform.GetChild(i);
            if (childTransform.gameObject.activeInHierarchy)
            {
                _plots.Add(childTransform.gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject GetClosestPlot(Vector3 pos, ref float minDistSq)
    {
        GameObject closest = null;
        foreach (GameObject plot in _plots)
        {
           // Vector3 globalpos = plot.transform.parent.TransformPoint(plot.transform.position + Vector3.up * 0.5f + Vector3.right * 0.5f);
            float distSq = (pos - (plot.transform.position + Vector3.up * 0.5f + Vector3.right * 0.5f)).sqrMagnitude;
            if (distSq < minDistSq)
            {
                minDistSq = distSq;
                closest = plot;
            }
        }
        return closest;
    }
}
