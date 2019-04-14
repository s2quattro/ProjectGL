/*
*	Original Code :
*	https://forum.unity.com/threads/how-to-load-an-array-with-jsonutility.375735/#post-3039397
*	
*/
using UnityEngine;



public static class JsonParser
{
	#region json Loader

	/// <summary>
	/// read an unwrapped JSON and extract Objects
	/// </summary>
	/// <typeparam name="T">get Object array Type</typeparam>
	/// <param name="jsonArray">text of TextAsset</param>
	/// <returns>Object array</returns>
	public static T[] fromUnwrappedJson<T>(string jsonArray)
	{
		jsonArray = wrapjsonArray(jsonArray); 
		return importObjectFromJson<T>(jsonArray);
	}

	/// <summary>
	/// read a JSON wrapped by "objects" object and extract Objects
	/// </summary>
	/// <typeparam name="T">get Object array Type</typeparam>
	/// <param name="jsonArray">text of TextAsset</param>
	/// <returns>Object array</returns>
	public static T[] importObjectFromJson<T>(string jsonObject)
	{
		Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(jsonObject);
		return wrapper.objects;
	}

	//Json warpper
	private static string wrapjsonArray(string jsonArray)
	{
		//!Caution! THE NAME OF WRAPPER OBJECT MUST BE SAME WITH MEMBER NAME IN WRAPPER CLASS!
		return "{ \"objects\": " + jsonArray + "}";
	}
	#endregion

	
	#region json Saver

	/// <summary>
	/// save Object to JSON 
	/// </summary>
	/// <typeparam name="T">save Object array Type</typeparam>
	/// <param name="array">and specific Object array</param>
	/// <returns></returns>
	public static string ToJson<T>(T[] array)
	{
		Wrapper<T> wrapper = new Wrapper<T>();
		wrapper.objects = array;
		return JsonUtility.ToJson(wrapper);
	}	
	public static string ToJson<T>(T[] array, bool prettyPrint)
	{
		Wrapper<T> wrapper = new Wrapper<T>();
		wrapper.objects = array;
		return JsonUtility.ToJson(wrapper, prettyPrint);
	}
	#endregion



	/// <summary>
	/// Json Wrapper Class Def
	/// </summary>
	/// <typeparam name="T"></typeparam>
	[System.Serializable]
	private class Wrapper<T>
	{
		public T[] objects;
	}
}