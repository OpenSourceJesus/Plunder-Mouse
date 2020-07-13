using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;

namespace PlunderMouse
{
	public class MakeMagicIndicator : MonoBehaviour, IUpdatable
	{
		public bool PauseWhileUnfocused
		{
			get
			{
				return true;
			}
		}
        public Transform trs;
		public MagicIndicator magicIndicatorPrefab;
		public MagicIndicator magicIndicator;

        public virtual void OnEnable ()
        {
			magicIndicator = GameManager.GetSingleton<ObjectPool>().SpawnComponent<MagicIndicator>(magicIndicatorPrefab, default(Vector3), default(Quaternion), GameManager.GetSingleton<MagicLocater>().trs);
            magicIndicator.trs.localScale = magicIndicatorPrefab.trs.localScale;
			magicIndicator.trs.localEulerAngles = Vector3.right * 90;
            GameManager.updatables = GameManager.updatables.Add(this);
        }

        public virtual void DoUpdate ()
        {
            magicIndicator.SetOrientation (trs);
			magicIndicator.gameObject.SetActive(magicIndicator.trs.localPosition.sqrMagnitude <= .25f);
        }

        public virtual void OnDisable ()
        {
            if (magicIndicator != null)
            {
                magicIndicator.gameObject.SetActive(false);
                // GameManager.GetSingleton<ObjectPool>().Despawn (magicIndicator.prefabIndex, magicIndicator.gameObject, magicIndicator.trs);
                Destroy(magicIndicator.gameObject);
            }
            GameManager.updatables = GameManager.updatables.Remove(this);
        }
	}
}