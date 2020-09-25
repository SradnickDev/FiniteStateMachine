using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace FSM
{
	public static class FSMHelper
	{
		/// <summary>
		/// Returns nearest T of given list, where T must derive from Component.
		/// </summary>
		/// <param name="list">List of T that is checked</param>
		/// <param name="origin">Start point for distance check</param>
		/// <param name="index">index of nearest T in list</param>
		/// <param name="maxRange">Optional: Default infinity, defines max distance.</param>
		/// <returns></returns>
		//----------------------------------------------------------------
		public static T FindNearest<T>(this List<T> list, Vector3 origin, out int index,
			float maxRange = Mathf.Infinity) where T : Component
		{
			var lastDistance = maxRange;
			index = -1;
			for (var i = 0; i < list.Count; i++)
			{
				var point = list[i];
				var distance = Vector3.Distance(origin, point.transform.position);
				if (distance < lastDistance)
				{
					lastDistance = distance;
					index = i;
				}
			}

			return index == -1 ? null : list[index];
		}

		/// <summary>
		/// Check if something is in a cone.
		/// </summary>
		/// <param name="origin">Start Transform, using always this forward vector to calc proper cone positioning.</param>
		/// <param name="target">Transform that will be checked.If its in cone.</param>
		/// <param name="radius">Radius of the Cone.</param>
		/// <param name="fieldOfView">FoV in percentage.</param>
		/// <returns>True if inside otherwise false.</returns>
		//----------------------------------------------------------------
		public static bool IsInsideCone(Transform origin, Transform target, float radius, float fieldOfView)
		{
			return IsInsideCone(origin, target, Vector3.zero, radius, fieldOfView);
		}

		/// <summary>
		/// Adds a line cast to check if the target is behind something.
		/// </summary>
		//----------------------------------------------------------------
		public static bool IsInsideConeLineHitTest(Transform origin, Transform target, float radius, float fieldOfView,
			LayerMask obstacleMask)
		{
			var blocked = Physics.Linecast(origin.position, target.position, obstacleMask);
			if (blocked) return false;

			return IsInsideCone(origin, target, Vector3.zero, radius, fieldOfView);
		}

		/// <summary>
		/// Adds a sphere cast to check if the target is behind something.
		/// </summary>
		//----------------------------------------------------------------
		public static bool IsInsideConeSphereHitTest(Transform origin, Transform target, float radius,
			float fieldOfView, LayerMask obstacleMask, float sphereRadius)
		{
			var dir = (target.position - origin.position).normalized;
			var ray = new Ray(origin.position, dir);

			var blocked = Physics.SphereCast(ray, sphereRadius, radius, obstacleMask);
			if (blocked) return false;

			return IsInsideCone(origin, target, Vector3.zero, radius, fieldOfView);
		}

		/// <summary>
		/// Check if something is in a cone.
		/// </summary>
		/// <param name="origin">Start Transform, using always this forward vector to calc proper cone positioning.</param>
		/// <param name="target">Transform that will be checked.If its in cone.</param>
		/// <param name="targetOffset">Is added to the target position.</param>
		/// <param name="radius">Radius of the Cone.</param>
		/// <param name="fieldOfView">FoV in percentage.</param>
		/// <returns>True if inside otherwise false.</returns>
		//----------------------------------------------------------------
		public static bool IsInsideCone(Transform origin, Transform target, Vector3 targetOffset, float radius,
			float fieldOfView)
		{
			var targetPos = target.position + targetOffset;
			var fwd = origin.transform.forward;
			var dir = (targetPos - origin.position).normalized;
			var angle = Vector3.Angle(dir, fwd);

			//small improved version of a distance check , instead of Vector3.Distance(pos1,pos2)
			//you also have to multiply the radius (radius * radius)
			var distance = (origin.position - targetPos).sqrMagnitude;

			return angle < fieldOfView / 2f && distance <= radius * radius;
		}

		/// <summary>
		/// Visualise the cone check.
		/// </summary>
		/// <param name="origin"></param>
		/// <param name="radius"></param>
		/// <param name="fieldOfView"></param>
		//----------------------------------------------------------------
		public static void DrawCone(Transform origin, float radius, float fieldOfView)
		{
			var lineColor = Color.black;
			fieldOfView = Mathf.Clamp(fieldOfView, 0, 360);
			var leftOffset = Quaternion.AngleAxis(-fieldOfView / 2, Vector3.up) * origin.forward;
			var rightOffset = Quaternion.AngleAxis(fieldOfView / 2, Vector3.up) * origin.forward;
#if UNITY_EDITOR
			Handles.color = lineColor;
			Handles.DrawWireArc(origin.position, Vector3.up, leftOffset, fieldOfView, radius);

			Handles.color = new Color(1f, 1f, 1f, 0.05f);
			Handles.DrawSolidArc(origin.position, Vector3.up, leftOffset, fieldOfView, radius);
#else
			Gizmos.DrawWireSphere(origin.position, radius);
#endif
			Gizmos.color = lineColor;
			Debug.DrawRay(origin.position,leftOffset * radius,lineColor);
			Debug.DrawRay(origin.position,rightOffset * radius,lineColor);
		}

		/// <summary>
		/// To Visualise a sphere-cast.
		/// </summary>
		//----------------------------------------------------------------
		public static void DrawSphereCast(Transform origin, Transform target, float radius)
		{
			var forward = origin.forward;
			var position = origin.position;
			var up = origin.up;
			var right = origin.right;

			var startCenter = position + (target.position - origin.position).normalized * radius;
			var endCenter = target.position - (target.position - origin.position).normalized * radius;

			//Start ,End Sphere
			Gizmos.DrawWireSphere(startCenter, radius);
			Gizmos.DrawWireSphere(endCenter, radius);

			//Top, Bottom Line
			Gizmos.DrawLine(startCenter + up * radius, endCenter + up * radius);
			Gizmos.DrawLine(startCenter - up * radius, endCenter - up * radius);

			//Left, Right Line
			Gizmos.DrawLine(startCenter + right * radius, endCenter + right * radius);
			Gizmos.DrawLine(startCenter - right * radius, endCenter - right * radius);
		}

		//----------------------------------------------------------------
		public static void Log(bool writeLog, string message)
		{
			if (writeLog)
			{
				var color = ColorUtility.ToHtmlStringRGB(new Color(0.2f, 0.5f, 0.7f));
				Debug.Log($"<color=#{color}>[FSM]</color> " + message);
			}
		}

		/// <summary>
		/// Returns the name of type.
		/// </summary>
		//----------------------------------------------------------------
		public static string Name<T>(this T target)
		{
			return target == null ? "" : target.GetType().Name;
		}
	}
}