using UnityEngine;
using System.Collections.Generic;

namespace Extensions
{
	public static class LayerMaskExtensions
	{
		public static LayerMask Create (params string[] layerNames)
		{
			return NamesToMask(layerNames);
		}
	 
		public static LayerMask Create (params int[] layerNumbers)
		{
			return LayerNumbersToMask(layerNumbers);
		}
	 
		public static LayerMask NamesToMask (params string[] layerNames)
		{
			LayerMask ret = (LayerMask) 0;
			foreach (string name in layerNames)
				ret |= (1 << LayerMask.NameToLayer(name));
			return ret;
		}
	 
		public static LayerMask LayerNumbersToMask (params int[] layerNumbers)
		{
			LayerMask ret = (LayerMask) 0;
			foreach (int layer in layerNumbers)
				ret |= (1 << layer);
			return ret;
		}
	 
		public static LayerMask Inverse (this LayerMask original)
		{
			return ~original;
		}
	 
		public static LayerMask AddToMask (this LayerMask original, params string[] layerNames)
		{
			foreach (string layerName in layerNames)
				original |= (1 << LayerMask.NameToLayer(layerName));
			return original;
		}
	 
		public static LayerMask AddToMask (this LayerMask original, LayerMask layerMask)
		{
			return original.AddToMask(layerMask.MaskToNames());
		}
	 
		public static LayerMask RemoveFromMask (this LayerMask original, params string[] layerNames)
		{
			LayerMask invertedOriginal = ~original;
			return ~(invertedOriginal | NamesToMask(layerNames));
		}
	 
		public static string[] MaskToNames (this LayerMask original)
		{
			List<string> output = new List<string>();
			for (int i = 0; i < 32; i ++)
			{
				int shifted = 1 << i;
				if ((original & shifted) == shifted)
				{
					string layerName = LayerMask.LayerToName(i);
					if (!string.IsNullOrEmpty(layerName))
						output.Add(layerName);
				}
			}
			return output.ToArray();
		}
	 
		public static string MaskToString (this LayerMask original)
		{
			return MaskToString(original, ", ");
		}
	 
		public static string MaskToString (this LayerMask original, string delimiter)
		{
			return string.Join(delimiter, MaskToNames(original)) + delimiter;
		}
		
		public static bool MaskContainsLayer (this LayerMask original, int layer)
		{
			return original == (original | (1 << layer));
		}
	}
}