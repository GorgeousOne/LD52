
public class Scythe : Tool {
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
				break;
		}

		if (harvested) {
			harvested.transform.position = transform.position;
		}
	}
}