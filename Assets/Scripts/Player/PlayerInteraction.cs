using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour {
    //dont look at this
	[SerializeField] private float interactDist = 1.00f;
	[SerializeField] private SpawnerHandler spawnerHandler;
    [SerializeField] private ItemHandler itemHandler;
    [SerializeField] private PlotHandler plotHandler;
    [SerializeField] private Bucket bucketHandler;

    public Item heldItem = null;
    public Bucket heldBucket = null;
    private List<Transform> proximity = new List<Transform>();


    [SerializeField] private Transform indicator;
    Renderer script_renderer;

    void Start()
    {
        script_renderer = indicator.GetComponent<Renderer>();
        script_renderer.enabled = false;
    }

    // pls dont
    void Update() {
        float distancesqr = interactDist * interactDist;
        Bucket bucket = null;
        GameObject interactedPlot = plotHandler.GetClosestPlot(transform.position, ref distancesqr);
        Spawner interactedSpawner = spawnerHandler.GetClosestSpawner(transform.position,ref distancesqr);
        Item interactedItem = itemHandler.GetClosestItem(transform.position,heldItem,ref distancesqr);
        if(heldBucket == null)
            bucket = bucketHandler.distancesqr(transform.position,ref distancesqr);

        //Stop
        if (interactedItem != null)//Horrible pls find a better way
        {
            script_renderer.enabled = true;
            indicator.position = interactedItem.gameobject.transform.position;
        }
        else if (bucket != null)
        {
            script_renderer.enabled = true;
            indicator.position = bucket.transform.position;
        }
        else if(interactedPlot != null){
            script_renderer.enabled = true;
            indicator.position = interactedPlot.transform.position + Vector3.up * 0.5f + Vector3.right * 0.5f;// parent.TransformPoint(interactedPlot.transform.position) + Vector3.up * 0.5f + Vector3.right * 0.5f;
        }
        else if(interactedSpawner != null){
            script_renderer.enabled = true;
            indicator.position = interactedSpawner.transform.position;
        }
        else{
            script_renderer.enabled = false;
        }

        if (Input.GetKeyUp(KeyCode.E)) {
			if(heldItem != null && heldBucket == null)
            {
                if(interactedPlot != null)
                {
                    PlotContent plotcontent = interactedPlot.GetComponent<PlotContent>();
                    plotcontent.PlantItem(heldItem);
                    heldItem.gameobject.SetActive(false);
                    heldItem = null;
                }
                else {
                    dropItem();
                }
            }
            else if(heldBucket != null && heldItem == null)
            {
                if (false)
                {//water

                }
                else if(interactedPlot != null)
                {
                    PlotContent plotcontent = interactedPlot.GetComponent<PlotContent>();
                    plotcontent.water(heldBucket);
                }
                else
                {
                    heldBucket = null;
                }
            }
            else
            {

                if (interactedItem != null)
                {
                    heldItem = interactedItem;
                }
                else if (interactedSpawner != null)
                {
                    heldItem = interactedSpawner.GetItem();
                }else if( bucket != null)
                {
                    heldBucket = bucket;
                }
            }
        }

        if (heldItem != null)
        {
			heldItem.gameobject.transform.position = transform.position + Vector3.up * 0.5f;
        }
        else if(heldBucket != null ){
            heldBucket.transform.position = transform.position + Vector3.up * 0.5f;
        }
    }
    //well i told you to stop
	private void dropItem(){
		heldItem = null;
	}
    private Transform getCLosest(Vector3 pos)
    {
        Transform closest = null;
        float minDistSq = interactDist * interactDist;

        foreach (Transform t in proximity)
        {
            float distSq = (pos - t.position).sqrMagnitude;

            if (distSq < minDistSq)
            {
                minDistSq = distSq;
                closest = t;
            }
        }
        return closest;
    }
}
