using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour {

	[SerializeField] private float interactDist = 1.00f;
	[SerializeField] private SpawnerHandler spawnerHandler;
    [SerializeField] private ItemHandler itemHandler;
    [SerializeField] private PlotHandler plotHandler;

    public Item heldItem = null;
    private List<Transform> proximity = new List<Transform>();


    [SerializeField] private Transform indicator;
    Renderer renderer;

    void Start()
    {
        renderer = indicator.GetComponent<Renderer>();
        renderer.enabled = false;
    }

    // Update is called once per frame
    void Update() {
        float distancesqr = interactDist * interactDist;
        GameObject interactedPlot = plotHandler.GetClosestPlot(transform.position, ref distancesqr);
        Spawner interactedSpawner = spawnerHandler.GetClosestSpawner(transform.position,ref distancesqr);
        Item interactedItem = itemHandler.GetClosestItem(transform.position,heldItem,ref distancesqr);
        
        if (interactedItem != null)//Horrible pls find a better way
        {
            renderer.enabled = true;
            indicator.position = interactedItem.gameobject.transform.position;
        }
        else if(interactedPlot != null){
            renderer.enabled = true;
            indicator.position = interactedPlot.transform.position + Vector3.up * 0.5f + Vector3.right * 0.5f;// parent.TransformPoint(interactedPlot.transform.position) + Vector3.up * 0.5f + Vector3.right * 0.5f;
        }
        else if(interactedSpawner != null){
            renderer.enabled = true;
            indicator.position = interactedSpawner.transform.position;
        }
        else{
            renderer.enabled = false;
        }
        if (Input.GetKeyUp(KeyCode.E)) {
			if(heldItem != null)
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
            else
            {
                if (interactedItem != null)
                {
                    heldItem = interactedItem;
                }
                else if (interactedSpawner != null)
                {
                    heldItem = interactedSpawner.GetItem();
                }
            }
        }
        if (heldItem != null)
        {
			heldItem.gameobject.transform.position = transform.position + Vector3.up * 0.5f;
        }
    }

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
