
using UnityEngine.Events;

public class Scythe : Tool {
	
	public UnityEvent OnKill;

	void OnEnable() {
		toolType = ToolType.Bucket;
	}

	public override void Interact(Interactable i) {
		Item harvested = null;
		
		switch (i.interactableType) {
			case InteractableType.Plot:
				PlotContent plot = i.GetComponent<PlotContent>();
				harvested = plot.Harvest();
				break;
			case InteractableType.Person:
				NpcController npc = i.GetComponent<NpcController>();
				npc.Kill();
				OnKill.Invoke();
				break;
		}

		if (harvested) {
			harvested.transform.position = transform.position;
		}
	}
}