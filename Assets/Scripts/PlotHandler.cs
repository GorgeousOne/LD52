using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlotHandler : MonoBehaviour
{
    private List<PlotContent> _plots;

    void OnEnable() {
        _plots = gameObject.GetComponentsInChildren<PlotContent>().ToList();
    }

    public PlotContent GetClosestPlot(Vector3 pos, ref float minDistSq)
    {
        PlotContent closest = null;
        foreach (PlotContent plot in _plots)
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
