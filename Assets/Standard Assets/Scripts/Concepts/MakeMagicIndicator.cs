using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;
using GameDevJourney;

namespace PlunderMouse
{
	public class MakeMagicIndicator : UpdateWhileEnabled
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

        public override void OnEnable ()
        {
            base.OnEnable ();
			magicIndicator = ObjectPool.Instance.SpawnComponent<MagicIndicator>(magicIndicatorPrefab, default(Vector3), default(Quaternion), MagicLocater.Instance.trs);
            magicIndicator.trs.localScale = magicIndicatorPrefab.trs.localScale;
			magicIndicator.trs.localEulerAngles = Vector3.right * 90;
        }

        public override void DoUpdate ()
        {
            magicIndicator.SetOrientation (trs);
			magicIndicator.gameObject.SetActive(magicIndicator.trs.localPosition.sqrMagnitude <= .25f);
        }

        public override void OnDisable ()
        {
            base.OnDisable ();
            if (magicIndicator != null)
            {
                magicIndicator.gameObject.SetActive(false);
                // ObjectPool.Instance.Despawn (magicIndicator.prefabIndex, magicIndicator.gameObject, magicIndicator.trs);
                Destroy(magicIndicator.gameObject);
            }
        }
	}
}