using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PIQOPlugin : MonoBehaviour {

	static public AndroidJavaObject AppsOnUnityObject = null;
	static public AndroidJavaClass PIQOUnityClass = null;
	static public AndroidJavaObject activity = null;
	static public AndroidJavaClass pluginClass = null;
	static public bool appsonInitialized = false;

	string fakeProductCode = "PRD-TEST-SDK";
	string fakeProductItemCode = "PRD-TEST-SDK-d2c439fa-67f5-4fa3-be06-ac651b593858";
	string fakePrivateKey = "YourPrivateKey";

	void Awake(){
		Debug.Log ("awake");
	}

	void Start() {
		Debug.Log ("start");
		Initinalize ();
	}

	public void Initinalize()
	{
		if (!appsonInitialized) {
			AndroidJavaClass unityPlayer = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
			activity = unityPlayer.GetStatic<AndroidJavaObject> ("currentActivity");

			pluginClass = new AndroidJavaClass ("ir.appson.PIQOlibrary.AppsonUnityPlugin");
			PIQOUnityClass = new AndroidJavaClass ("ir.appson.PIQOlibrary.Payment");
			if (pluginClass != null) {
				AppsOnUnityObject = pluginClass.CallStatic<AndroidJavaObject> ("getInstance");
				AppsOnUnityObject.CallStatic ("init", new object[1]{activity});
				appsonInitialized = true;
			}
		}
	}

	public void SamplePurchase() {
		ShowInfo (new PIQOListener (), fakeProductCode, fakeProductItemCode, fakePrivateKey);
	}

	public void ShowInfo(PIQOListener listener, string productCode, string productItemCode, string privateKey) {
		PIQOUnityClass.CallStatic ("showInfo", new object[4]{ listener, productCode, productItemCode, privateKey});
	}
		
	public class PIQOListener : AndroidJavaProxy
	{
		public PIQOListener() : base("ir.appson.PIQOlibrary.Interface.InfoListener") { }
		void onSuccess(string token)
		{
			Debug.Log ("PIQO Success");
		}

		void onFailed(string exceptionCode)
		{
			Debug.Log ("PIQO failed with error code : " + exceptionCode);
		}

		void onUserCancelled()
		{
			Debug.Log ("User Cancelled");
		}
	}
}