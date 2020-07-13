using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FastMemberCore;
using System.Reflection;

public static class ReflectionUtilities
{
	public static T GetMember<T> (this object obj, string memberPath)
	{
		string[] memberNames = memberPath.Split('.');
		for (int i = 0; i < memberNames.Length; i ++)
		{
			string memberName = memberNames[i];
			ObjectAccessor objectAccessor = ObjectAccessor.Create(obj);
			obj = objectAccessor[memberName];
		}
		return (T) obj;
	}
	
	public static void SetMember<T> (this object obj, string memberPath, T value)
	{
		string[] memberNames = memberPath.Split('.');
		string memberName = "";
		object _obj = obj;
		for (int i = 0; i < memberNames.Length; i ++)
		{
			memberName = memberNames[i];
			if (i < memberNames.Length - 1)
			{
				ObjectAccessor objectAccessor = ObjectAccessor.Create(_obj);
				_obj = objectAccessor[memberName];
			}
		}
		TypeAccessor typeAccessor = TypeAccessor.Create(_obj.GetType());
		typeAccessor[_obj, memberName] = value;
	}
	
	public static object InvokeMember (this object obj, string memberPath, BindingFlags bindingFlags, params object[] args)
	{
		string[] memberNames = memberPath.Split('.');
		string memberName = "";
		object _obj = obj;
		for (int i = 0; i < memberNames.Length; i ++)
		{
			memberName = memberNames[i];
			if (i < memberNames.Length - 1)
			{
				ObjectAccessor objectAccessor = ObjectAccessor.Create(_obj);
				_obj = objectAccessor[memberName];
			}
		}
		return _obj.GetType().InvokeMember (memberName, bindingFlags, null, _obj, args);
	}
}