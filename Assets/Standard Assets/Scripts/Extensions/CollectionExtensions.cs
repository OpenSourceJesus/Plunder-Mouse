using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Extensions
{
	public static class CollectionExtensions 
	{
		public static List<T> ToList<T> (this T[] array)
		{
			List<T> output = new List<T>();
			output.AddRange(array);
			return output;
		}

		public static T[] Add<T> (this T[] array, T element)
		{
			List<T> output = array.ToList();
			output.Add(element);
			return output.ToArray();
		}

		public static T[] Remove<T> (this T[] array, T element)
		{
			List<T> output = array.ToList();
			output.Remove(element);
			return output.ToArray();
		}

		public static T[] RemoveAt<T> (this T[] array, int index)
		{
			List<T> output = array.ToList();
			output.RemoveAt(index);
			return output.ToArray();
		}

		public static T[] AddRange<T> (this T[] array, IEnumerable<T> array2)
		{
			List<T> output = array.ToList();
			output.AddRange(array2);
			return output.ToArray();
		}

		public static bool Contains<T> (this T[] array, T element)
		{
			foreach (T obj in array)
			{
				if (obj == null)
				{
					if (element == null)
						return true;
				}
				else if (obj.Equals(element))
					return true;
			}
			return false;
		}

		public static int IndexOf<T> (this T[] array, T element)
		{
			for (int i = 0; i < array.Length; i ++)
			{
				if (array[i].Equals(element))
					return i;
			}
			return -1;
		}
		
		public static T[] Reverse<T> (this T[] array)
		{
			List<T> output = array.ToList();
			output.Reverse();
			return output.ToArray();
		}

		public static T[] AddArray<T> (this T[] array, Array array2)
		{
			List<T> output = array.ToList();
			for (int i = 0; i < array2.Length; i ++)
				output.Add((T) array2.GetValue(i));
			return output.ToArray();
		}

		public static string ToString<T> (this T[] array, string elementSeperator = ", ")
		{
            string output = "";
            foreach (T element in array)
                output += element.ToString() + elementSeperator;
			return output;
		}

		public static T[] RemoveEach<T> (this T[] array, IEnumerable<T> array2)
		{
			List<T> output = array.ToList();
			foreach (T element in array2)
				output.Remove(element);
			return output.ToArray();
		}

		public static T[] Insert<T> (this T[] array, T element, int index)
		{
			List<T> output = array.ToList();
			output.Insert(index, element);
			return output.ToArray();
		}

		public static int IndexOf<T> (this Array array, T element)
		{
			for (int index = 0; index < array.GetLength(0); index ++)
			{
				if (((T) array.GetValue(index)).Equals(element))
				{
					return index;
				}
			}
			return -1;
		}

		public static T[] _Sort<T> (this T[] array, IComparer<T> sorter)
		{
			List<T> output = array.ToList();
			output.Sort(sorter);
			return output.ToArray();
		}

		public static int Count (this IEnumerable enumerable)
		{
			int output = 0;
			IEnumerator enumerator = enumerable.GetEnumerator();
			while (enumerator.MoveNext())
				output ++;
			return output;
		}

		public static T Get<T> (this IEnumerable<T> enumerable, int index)
		{
			IEnumerator enumerator = enumerable.GetEnumerator();
			while (enumerator.MoveNext())
			{
				index --;
				if (index < 0)
					return (T) enumerator.Current;
			}
			return default(T);
		}

		public static float GetMin (this float[] array)
		{
			float min = array[0];
			for (int i = 1; i < array.Length; i ++)
			{
				if (array[i] < min)
					min = array[i];
			}
			return min;
		}

		public static float GetMax (this float[] array)
		{
			float max = array[0];
			for (int i = 1; i < array.Length; i ++)
			{
				if (array[i] > max)
					max = array[i];
			}
			return max;
		}

		public static List<T> _Add<T> (this List<T> list, T element)
		{
			list.Add(element);
			return list;
		}
		
		public static int Length<T> (this List<T> list)
		{
			return list.Count;
		}
		
		public static List<T> _RemoveAt<T> (this List<T> list, int index)
		{
			list.RemoveAt(index);
			return list;
		}
		
		public static List<T> _Remove<T> (this List<T> list, T element)
		{
			list.Remove(element);
			return list;
		}
		
		public static T[] _RemoveAt<T> (this T[] array, int index)
		{
			array = array.RemoveAt(index);
			return array;
		}
		
		public static T[] _Remove<T> (this T[] array, T element)
		{
			array = array.Remove(element);
			return array;
		}
		
		public static T[] _Add<T> (this T[] array, T element)
		{
			array = array.Add(element);
			return array;
		}

		public static T1[] GetKeys<T1, T2> (this Dictionary<T1, T2> dict)
		{
			List<T1> output = new List<T1>();
			IEnumerator keyEnumerator = dict.Keys.GetEnumerator();
			while (keyEnumerator.MoveNext())
				output.Add((T1) keyEnumerator.Current);
			return output.ToArray();
		}

		public static bool Contains_IList<T> (this IList<T> list, T element)
		{
			return list.Contains(element);
		}
	}
}