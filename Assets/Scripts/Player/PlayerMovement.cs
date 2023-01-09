using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	[Header("Walk")]
	[SerializeField] private float Speed = 2f;
	[SerializeField] private float accelerateTime = 0.2f;

	private Rigidbody2D _rigid;
	private SpriteRenderer _renderer;
	
	private float _last_input_w;
	private float _last_input_h;
	

	private void OnEnable() {
		_rigid = GetComponent<Rigidbody2D>();
		_renderer = GetComponent<SpriteRenderer>();
	}

	// Update is called once per frame
	void Update() {
		_ReadInputs();
	}

	private void _ReadInputs() {
		_last_input_w = Input.GetAxisRaw("Horizontal");
		_last_input_h = Input.GetAxisRaw("Vertical");

		if (Math.Abs(_last_input_w) > 0) {
			_renderer.flipX = _last_input_w < 0;
		}
	}
	
	private void FixedUpdate() {
		_CalcMoveSpeed();
	}

	private void _CalcMoveSpeed() {
		Vector2 newVelocity = _rigid.velocity;
		Vector3 moveDir = Vector3.right * _last_input_w + Vector3.up * _last_input_h;
		moveDir.Normalize();
		
		if (moveDir.magnitude > 0.01) {
			newVelocity = GetAccelerated(moveDir, newVelocity.magnitude);
		}
		else {
			newVelocity = GetDecelerated(newVelocity);
		}
		_rigid.velocity = newVelocity;
	}
	
	private Vector2 GetAccelerated(Vector2 headedDir, float currentSpeed) {
    	float acceleration = Time.fixedDeltaTime / accelerateTime * Speed;
    	Vector2 newSpeed = headedDir.normalized * (currentSpeed + acceleration);
        return Vector2.ClampMagnitude(newSpeed, Speed);
	}
    
    private Vector2 GetDecelerated(Vector2 currentVel) {
        float deceleration = Time.fixedDeltaTime / accelerateTime * Speed;
        float currSpeed = currentVel.magnitude;
        
        if (Mathf.Abs(currSpeed) < deceleration) {
            return Vector2.zero;
        }
        Vector2 newSpeed = currentVel.normalized * (currSpeed - deceleration);
        return Vector2.ClampMagnitude(newSpeed, Speed);
    }
}