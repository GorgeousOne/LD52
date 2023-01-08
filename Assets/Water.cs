using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : Interactable {
	private void OnEnable() {
		interactableType = InteractableType.Water;
	}
}