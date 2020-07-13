using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Extensions
{
	public static class RectTransformExtensions
	{
		public static Rect GetRectInWorld (this RectTransform rectTrs)
		{
			Vector2 min = rectTrs.TransformPoint(rectTrs.rect.min);
			Vector2 max = rectTrs.TransformPoint(rectTrs.rect.max);
			return Rect.MinMaxRect(min.x, min.y, max.x, max.y);
		}
		
		public static Vector2 GetCenterInCanvasNormalized (this RectTransform rectTrs, RectTransform canvasRectTrs)
		{
			return canvasRectTrs.GetRectInWorld().ToNormalizedPosition(rectTrs.GetRectInWorld().center);
		}

		public static Rect GetRectInCanvasNormalized (this RectTransform rectTrs, RectTransform canvasRectTrs)
		{
			Rect output = rectTrs.GetRectInWorld();
			Rect canvasRect = canvasRectTrs.GetRectInWorld();
			Vector2 outputMin = canvasRect.ToNormalizedPosition(output.min);
			Vector2 outputMax = canvasRect.ToNormalizedPosition(output.max);
			return Rect.MinMaxRect(outputMin.x, outputMin.y, outputMax.x, outputMax.y);
		}
	}
}