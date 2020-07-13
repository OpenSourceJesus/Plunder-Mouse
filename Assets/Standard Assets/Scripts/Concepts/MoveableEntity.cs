using UnityEngine;

namespace PlunderMouse
{
	public class MoveableEntity : MonoBehaviour
	{
		public Transform trs;
		public CharacterController controller;
		public Rigidbody rigid;
		public float moveSpeed;
		public float multiplyRigidMoveSpeed;
		public float gravity;
		[HideInInspector]
		public float yVel;
		[HideInInspector]
		public Vector3 move;
	}
}