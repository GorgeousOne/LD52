using UnityEngine;

public  enum ToolType {
	Bucket,
	Scythe
}

public abstract class Tool : MonoBehaviour {
	
	public ToolType toolType;
	public abstract void Interact(Interactable i);
}