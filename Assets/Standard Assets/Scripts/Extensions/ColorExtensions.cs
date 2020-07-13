using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Extensions
{
	public static class ColorExtensions
	{
		public static Color SetAlpha (this Color c, float a)
		{
			return new Color(c.r, c.g, c.b, a);
		}
		
		public static Color AddAlpha (this Color c, float a)
		{
			return SetAlpha (c, c.a + a);
		}
		
		public static Color MultiplyAlpha (this Color c, float a)
		{
			return SetAlpha (c, c.a * a);
		}
		
		public static Color DivideAlpha (this Color c, float a)
		{
			return SetAlpha (c, c.a / a);
		}
		
		public static byte[] ToBytes (this Color color)
		{
			byte[] bytes = new byte[4];
			bytes[0] = (byte) (255 * color.r - 128);
			return bytes;
		}

		public static Color RandomColor ()
		{
			return new Color(Random.value, Random.value, Random.value);
		}
	}
}