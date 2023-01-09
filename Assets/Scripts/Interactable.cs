
using UnityEngine;

public enum InteractableType {
	Plot,
	Water,
	Person,

	SoulGrinder
}

public abstract class Interactable : MonoBehaviour {
	public InteractableType interactableType;
}