using UnityEngine;

namespace PlunderMouse
{
	public class Water : MonoBehaviour
	{
		public virtual void OnTriggerEnter (Collider other)
		{
			CharacterController controller = other.GetComponentInParent<CharacterController>();
			if (controller != null)
			// {
				controller.enabled = false;
			// 	PlayerMouse playerMouse = other.GetComponent<PlayerMouse>();
			// 	if (playerMouse != null)
			// 		playerMouse.isSwimming = true;
			// }
		}
		
		public virtual void OnTriggerExit (Collider other)
		{
			CharacterController controller = other.GetComponentInParent<CharacterController>();
			if (controller != null)
			// {
				controller.enabled = true;
			// 	PlayerMouse playerMouse = other.GetComponent<PlayerMouse>();
			// 	if (playerMouse != null)
			// 		playerMouse.isSwimming = false;
			// }
		}
	}
}