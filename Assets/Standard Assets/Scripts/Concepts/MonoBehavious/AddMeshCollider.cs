using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace PlunderMouse
{
	public class AddMeshCollider : MonoBehaviour
	{
        void Start ()
        {
            gameObject.AddComponent<MeshCollider>();
            Destroy(this);
        }
	}
}