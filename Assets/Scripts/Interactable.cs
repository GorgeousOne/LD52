
using UnityEngine;

public enum InteractableType {
	Plot,
	Water,
	Person
}

public abstract class Interactable : MonoBehaviour {
	public InteractableType interactableType;
}