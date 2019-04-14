using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System;
using ScottGarland;  //BigInteger Lib
using System.Text;



namespace GLCore
{
	public enum InteractionType
	{
		teleport,
		interactNPC,
		mining
	}

	// For information Type of a CountSetter panel
	public enum countSetType
	{
		discard,
		use,
		buy,
		sell
	}

	// For information Type of a Decision Reminder panel
	public enum decisionType
	{
		discard,
		expandInvSlot,
		buy,
		sell,
		use
	}

	public static class QuartersRadian
	{
		//Diagonal Degree
		public const float RU_radian = 0.7071068f;  //45 degree : Equal to Mathf.Cos(45f * Mathf.Deg2Rad)
		public const float LU_radian = -0.7071068f;  //135 degree : Equal to Mathf.Cos(135f * Mathf.Deg2Rad)
		public const float LD_radian = -0.7071068f;  //225 degree : Equal to Mathf.Cos(225f * Mathf.Deg2Rad)
		public const float RD_radian = 0.7071068f;  //315 degree : Equal to Mathf.Cos(315f * Mathf.Deg2Rad)

		public const float sightRadian = 0.4226182f;  //65 degree : Equal to Mathf.Cos(65f * Mathf.Deg2Rad)
	}

	// Message info of simple message popup ui
	public class MessagePopUpInfo
	{
		public string title;
		public string content;

		public MessagePopUpInfo(string title = null, string content = null)
		{
			this.title = title;
			this.content = content;
		}		
	}



	#region Custom Exception Def

	public class EmployeeListNotFoundException : Exception
	{
		public EmployeeListNotFoundException()
		{
		}

		public EmployeeListNotFoundException(string message)
			: base(message)
		{
		}

		public EmployeeListNotFoundException(string message, Exception inner)
			: base(message, inner)
		{
		}
	}

	public class OreTypeNotFoundException : Exception
	{
		public OreTypeNotFoundException()
		{
		}

		public OreTypeNotFoundException(string message)
			: base(message)
		{
		}

		public OreTypeNotFoundException(string message, Exception inner)
			: base(message, inner)
		{

		}
	}

	#endregion
}
