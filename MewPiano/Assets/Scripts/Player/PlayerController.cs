using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MEC;

public class PlayerController : MonoBehaviour
{
	public float walkSpeed = 2;
	public float runSpeed = 6;
	public float gravity = -12;
	public float jumpHeight = 1;

	[Range(0, 1)] public float airControlPercent;

	public float turnSmoothTime = 0.2f;
	float turnSmoothVelocity;

	public float speedSmoothTime = 0.1f;
	float speedSmoothVelocity;
	float currentSpeed;

	public float standJumpDelay = .15f;
	bool isJumping = false;
	float velocityY;
	float eulerY = 0;

	bool isRotationFixed = false;

	CharacterController controller;
	public Animator animator { get; set; }
	Transform cameraTransform;

	private void Start()
	{
		animator = GetComponentInChildren<Animator>();
		controller = GetComponent<CharacterController>();

		cameraTransform = this.transform.Search("LookTarget");
	}

	public bool isPathFinding = false;

	private void Update()
	{
		if (!isPathFinding)
		{
			moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
		}

		bool isRunning = Input.GetKey(KeyCode.LeftShift);

		Move(moveInput, isRunning);

		if (Input.GetKeyDown(KeyCode.Space))
		{
			Jump();
		}

		if (Input.GetMouseButtonDown(0))
		{
			isRotationFixed = true;

			eulerY = cameraTransform.eulerAngles.y;
		}

		if (Input.GetMouseButtonUp(0))
		{
			isRotationFixed = false;
		}

		float movement = (isRunning ? currentSpeed / runSpeed : currentSpeed / walkSpeed * 0.5f);

		if (movement < 0.02f) movement = 0;

		animator.SetFloat(Define.MOVEMENT, movement, speedSmoothTime, Time.deltaTime);
	}

	private void Move(Vector2 inputDir, bool running)
	{
		if (inputDir != Vector2.zero)
		{
			if (!isPathFinding)
			{
				float rotationY = isRotationFixed ? eulerY : cameraTransform.eulerAngles.y;

				float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + rotationY;

				transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, GetModifiedSmoothTime(turnSmoothTime));
			}
		}

		float targetSpeed = ((running) ? runSpeed : walkSpeed) * inputDir.magnitude;

		currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, GetModifiedSmoothTime(speedSmoothTime));

		velocityY += Time.deltaTime * gravity;

		Vector3 velocity = transform.forward * currentSpeed + Vector3.up * velocityY;

		controller.Move(velocity * Time.deltaTime);

		currentSpeed = new Vector2(controller.velocity.x, controller.velocity.z).magnitude;

		if (controller.isGrounded) velocityY = 0;
	}

	private void Jump()
	{
		if (isJumping) return;

		Timing.RunCoroutine(Co_Jump());
	}

	private IEnumerator<float> Co_Jump()
	{
		float jumpVelocity = Mathf.Sqrt(-2 * gravity * jumpHeight);
		float movement = animator.GetFloat(Define.MOVEMENT);
		bool isMoving = movement > 0.02f;

		isJumping = true;

		animator.SetInteger(Define.JUMP, isMoving ? Define.JUMPRUN : Define.JUMPSTAND);

		if (!isMoving) yield return Timing.WaitForSeconds(standJumpDelay);

		velocityY = jumpVelocity;

		yield return Timing.WaitUntilTrue(() => velocityY == 0);

		animator.SetInteger(Define.JUMP, 0);

		isJumping = false;
	}

	private float GetModifiedSmoothTime(float _smoothTime)
	{
		if (controller.isGrounded)
		{
			return _smoothTime;
		}

		if (airControlPercent == 0)
		{
			return float.MaxValue;
		}

		return _smoothTime / airControlPercent;
	}












	#region Movement

	public float rotationSpeed = 720f;
	public float stopDistance = 5;
	public Vector2 moveInput;

	CoroutineHandle handle_move;
	CoroutineHandle handle_rotate;

	public void FollowTarget(Transform _target)
	{
		Timing.KillCoroutines(handle_move);
		Timing.KillCoroutines(handle_rotate);

		handle_move = Timing.RunCoroutine(Co_FollowTarget(_target));
	}

	private IEnumerator<float> Co_FollowTarget(Transform _target)
	{
		stopDistance = 2f;

		TurnToTarget(_target, true);

		while (Vector3.Distance(this.transform.position, _target.transform.position) >= stopDistance)
		{
			yield return Timing.WaitForOneFrame;

			var moveDir = new Vector2(_target.position.x - this.transform.position.x, _target.position.y - this.transform.position.y).normalized * walkSpeed;

			moveInput = moveDir;
		}

		this.transform.position = _target.transform.position;

		moveInput = Vector2.zero;

		yield return Timing.WaitUntilTrue(() => Vector3.Distance(this.transform.position, _target.transform.position) >= stopDistance);

		yield return Timing.WaitForSeconds(.5f);

		FollowTarget(_target);
	}


	public void MoveToTarget(Transform _target)
	{
		isPathFinding = true;

		Timing.KillCoroutines(handle_move);
		Timing.KillCoroutines(handle_rotate);

		handle_move = Timing.RunCoroutine(Co_MoveToTarget(_target.position));
	}

	public void MoveToTarget(Vector3 _target)
	{
		isPathFinding = true;

		Timing.KillCoroutines(handle_move);
		Timing.KillCoroutines(handle_rotate);

		handle_move = Timing.RunCoroutine(Co_MoveToTarget(_target));
	}

	private IEnumerator<float> Co_MoveToTarget(Vector3 _target)
	{
		//TurnToTarget(_target);

		var moveDir = new Vector2(_target.x - this.transform.position.x, _target.y - this.transform.position.y).normalized * walkSpeed;

		moveInput = moveDir;

		yield return Timing.WaitUntilTrue(() => Vector3.Distance(this.transform.position, _target) <= stopDistance);

		moveInput = Vector2.zero;

		isPathFinding = false;
	}



	public void TurnToTarget(Transform _target, bool _isLookAt = false)
	{
		Timing.KillCoroutines(handle_rotate);

		handle_rotate = Timing.RunCoroutine(Co_TurnToTarget(_target, _isLookAt));
	}

	private IEnumerator<float> Co_TurnToTarget(Transform _target, bool _isLookAt = false)
	{
		if (!lookTarget)
		{
			while (Quaternion.Angle(this.transform.rotation, _target.transform.rotation) > 0.01f)
			{
				this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, _target.transform.rotation, rotationSpeed * Time.deltaTime);

				yield return Timing.WaitForOneFrame;
			}
		}

		if (!_isLookAt) yield break;

		while (true)
		{
			if (!lookTarget) lookTarget = true;

			Debug.Log("Looking Forever");

			this.transform.LookAt(_target);

			yield return Timing.WaitForOneFrame;
		}
	}

	bool lookTarget = false;


	public void TurnToTarget(Vector3 _target)
	{
		Timing.KillCoroutines(handle_rotate);

		handle_rotate = Timing.RunCoroutine(Co_TurnToTarget(_target));
	}

	private IEnumerator<float> Co_TurnToTarget(Vector3 _target)
	{
		var targetDir = _target - this.transform.position;
		var rotation = Quaternion.LookRotation(targetDir);

		while (Quaternion.Angle(this.transform.rotation, rotation) > 1f)
		{
			this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, rotation, rotationSpeed * Time.deltaTime);

			yield return Timing.WaitForOneFrame;
		}
	}

	public void Stop()
	{
		Timing.KillCoroutines(handle_move);
		Timing.KillCoroutines(handle_rotate);

		moveInput = Vector2.zero;

		isPathFinding = false;

		this.transform.LookAt(null);
	}


	public float gravityMultiplier = 10f;

	#endregion
}