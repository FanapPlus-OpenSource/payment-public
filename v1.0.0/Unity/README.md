# PIQO Unity Plugin
Public specifications, samples and documentation on **PIQO Unity Plugin**.
- **PIQO** Unity Plugin works on Unity version 5+.


##How can I use PIQO Unity Plugin?
First, you need to have an existing Unity application.

 1. If you don't have any Android manifest file in your project yet, you can find a sample of that in ***Assets/Plugins/Android/*** in ***Sample*** folder above, and copy that to ***Assets/Plugins/Android/*** folder of your project. 

 2. Download **PIQO** Unity plugin from *Bin* folder and add it to your project through ***Assets->Import Package->Custom Package***. Then Import downloaded plugin to ***Assets/Plugins/Android*** folder in your project.
 
 3. Add Your app Id into `AndroidManifest.xml`.
 ```xml
 <meta-data android:name="Appson-Identity-App-Id" android:value="YOUR_APP_ID" />
 ```
 
 4. Link purchase buttons to purchase function (like sample in `PIQOPlugin.cs`).
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
 
 5. Check your application bundle id.
 
 6. In case your app does not already ask for Internet permission you should add the following line to the `AndroidManifest.xml` file under application tag:
 ```xml
 <uses-permission android:name="android.permission.INTERNET" />
 ```
 

A sample project can be found in **sample** folder. 

[1]: https://pg.appson.ir/