using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PeopleHandler : MonoBehaviour {
	
	[SerializeField] private float spawnInterval = 10;
	[SerializeField] private GameObject personPrefab;
	[SerializeField] private float walkDuration = 1;
	
	private List<Vector2> _waypoints = new();
	private Vector2 _spawnPoint;
	private Vector2 _exitPoint;
	
	private float _lastSpawn;
	private List<NpcController> _waitingPeople = new();
	
	private void OnEnable() {
		Transform[] children = gameObject.GetComponentsInChildren<Transform>();
		_waypoints = children.Select(transform1 => (Vector2) transform1.position).ToList();
		//remove yourself
		_waypoints.RemoveAt(0);
		_exitPoint = _waypoints[0];
		_spawnPoint = _waypoints.Last();
		_waypoints.RemoveAt(0);
		_waypoints.RemoveAt(_waypoints.Count - 1);
		
		_lastSpawn = Time.time - spawnInterval;
	}

	void Update() {
		if (_waitingPeople.Count < _waypoints.Count && Time.time - _lastSpawn >= spawnInterval) {
			_lastSpawn += spawnInterval;
			NpcController newPerson = Instantiate(personPrefab).GetComponent<NpcController>();
			newPerson.transform.position = _spawnPoint;
			newPerson.OnItemReceive.AddListener(_OnPersonItemReceive);
			newPerson.OnTargerReach.AddListener(_OnPeronTargetReach);
			_waitingPeople.Add(newPerson);
			newPerson.Walk(_waypoints.Count - 1, _waypoints.Last(), walkDuration);
		}
	}

	private void _OnPeronTargetReach(NpcController person) {
		//kills person if already exited
		if (person.queueIndex == -1) {
			Destroy(person.gameObject);
			return;
		}
		int idx = _waitingPeople.IndexOf(person);

		//moves person forward in empty queueueue
		if (idx < person.queueIndex) {
			person.Walk(person.queueIndex - 1, _waypoints[person.queueIndex - 1], walkDuration);
		//makes person say item
		} else if (person.queueIndex == 0) {
			person.SayItem();
		}
	}
	
	private void _OnPersonItemReceive(NpcController person) {
		_waitingPeople.Remove(person);
		person.Walk(-1, _exitPoint, walkDuration);

		foreach (NpcController other in _waitingPeople) {
			other.Walk(other.queueIndex - 1 , _waypoints[other.queueIndex - 1], walkDuration);
		}
	}
	
}