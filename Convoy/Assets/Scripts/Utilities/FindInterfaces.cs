using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace com.limphus.utilities
{
	public static class FindInterfaces
	{
		public static List<T> ListOf<T>()
		{
			List<T> interfaces = new List<T>();
			GameObject[] rootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();

			foreach (var rootGameObject in rootGameObjects)
			{
				T[] childrenInterfaces = rootGameObject.GetComponentsInChildren<T>();
				foreach (var childInterface in childrenInterfaces)
				{
					interfaces.Add(childInterface);
				}
			}

			return interfaces;
		}
	}
}