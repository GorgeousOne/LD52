
using UnityEngine;
using UnityEngine.Events;

public class Scythe : Tool {
	
	public UnityEvent OnKill;
	private AudioSource _audio;

	void OnEnable() {
		_audio = GetComponent<AudioSource>();
		toolType = ToolType.Bucket;
	}

	public override void Interact(Interactable i) {
		Item harvested = null;
		
		switch (i.interactableType) {
			case InteractableType.Plot:
				PlotContent plot = i.GetComponent<PlotContent>();
				harvested = plot.Harvest();
				_audio.Play();
				break;
			case InteractableType.Person:
				NpcController npc = i.GetComponent<NpcController>();
				npc.Kill();
				_audio.Play();
				OnKill.Invoke();
				break;
		}

		if (harvested) {
			harvested.transform.position = transform.position;
		}
	}
}