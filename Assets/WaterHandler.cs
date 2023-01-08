using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class WaterHandler : MonoBehaviour {
	private Water _water;
	
	private void OnEnable() {
		_water = gameObject.GetComponentInChildren<Water>();
	}

	public Water GetClosestWater(Vector2 pos, ref float minDistSq) {
		float distSq = pos.x - _water.transform.position.x;
		distSq *= distSq;
			
		if (distSq < minDistSq) {
			return _water;
		}
		return null;
	}
}