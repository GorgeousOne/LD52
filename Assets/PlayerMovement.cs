using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	[SerializeField] private float Speed = 2f;
	
	private float _last_input_w;
	private float _last_input_h;
	
	
	[Header("Walk")]
	[SerializeField] private float accelerateTime = 0.2f;

	
	// Start is called before the first frame update
	void Start() { }
	
	//
	private void OnEnable() {
	}

	// Update is called once per frame
	void Update() {
		_ReadInputs();
	}

	private void _ReadInputs() {
		_last_input_w = Input.GetAxisRaw("Horizontal");
		_last_input_h = Input.GetAxisRaw("Vertical");
	}
	
	private void FixedUpdate() {
		Vector3 movement = Vector3.right * _last_input_w + Vector3.up * _last_input_h;
		movement.Normalize();
		transform.position += Time.fixedDeltaTime *  movement * Speed;
	}
}