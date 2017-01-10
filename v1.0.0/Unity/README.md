# PIQO Unity SDK
Public specifications, samples and documentation on **PIQO Unity SDK**.


##How can I use PIQO Unity SDK?(Quick Tutorial)
To use **PIQO Unity SDK** you can download the `.aar` file from *Bin* folder. Refer to the *sample* folder for usage.

##How can I use PIQO Unity SDK?(More Thorough Tutorial)
First, you need to have an existing Unity application.
You can add this line to your dependencies in build file:


Download `.aar` from *Bin* folder above. Now add the downloaded `.aar` file to your project:

 1. add the downloaded `.aar` file to your project through ***Assets->Import Package->Custom Package***
 
 2. Add Your app Id into `AndroidManifest.xml`.
 ```xml
 <meta-data android:name="Appson-Identity-App-Id" android:value="YOUR_APP_ID" />
 ```
 
 3. Link purchase buttons to purchase function (like sample in `PIQOPlugin.cs`).
 ```C#
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
 ```
 Replace `productCode`, `productItemCode` and `privateKey` with your product info which defined in the [PIQO developer panel][1].
 
 4. Check your application bundle id.
 
 5. In case your app does not already ask for Internet permission you should add the following line to the `AndroidManifest.xml` file under application tag:
 ```xml
 <uses-permission android:name="android.permission.INTERNET" />
 <uses-permission android:name="android.permission.ACCESS_COARSE_LOCATION"/><!-- Optional --> 
 ```
 

A sample project can be found in **sample** folder. 

[1]: https://pg.appson.ir/