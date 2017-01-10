# PIQO Android SDK
Public specifications, samples and documentation on **PIQO Android SDK**.


##How can I use PIQO Android SDK?(Quick Tutorial)
To use **PIQO Android SDK** you can download the `.aar` file from *Bin* folder. Refer to the *sample* folder for usage.

##How can I use PIQO Android SDK?(More Thorough Tutorial)
First, you need to have an existing android application.
You can add this line to your dependencies in build file:

`compile 'ir.appson:PIQOlibrary:1.0.4'`

Or

Download `.aar` from *Bin* folder above. Now add the downloaded `.aar` file to your project. Steps in *Android Studio*:

 1. Copy `.aar` file to ***libs*** folder.
 2. Choose ***New Module*** option under ***File->New*** menu.
 3. Import copied `.aar` file.
 4. Go to ***File->Project Settings***.
 5. Under ***Modules*** in left menu, select ***app***.
 6. Go to ***Dependencies*** tab.
 7. Click the ***+*** button in the upper right corner.
 8. Select ***Module Dependency***.
 9. Select the new module from the list.
 

Now you have to add your *AppId* to `AndroidManifest.xml` file inside `<application/>` tag:
```xml
<manifest>
<application>
...
<meta-data android:name="Appson-Identity-App-Id" android:value="MY_APP_ID" />
</application>
...
</manifest>
```

Replace `MY_APP_ID` with your *ApplicationId*.  Then you need to call `Appson.init` method in `onCreate()` method of your activity and pass a context to this method:
```java
import ir.appson.identitylibrary.Appson;
import ir.appson.identitylibrary.Interface.InfoListener;
import ir.appson.identitylibrary.Payment;
public class MainActivity extends Activity{
@Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        Appson.init(MainActivity.this);
        }
}
```
Now you can use `Payment.sohwInfo` method to show payment process to the user. You have to pass a `InfoListener` object to `Payment.showInfo()` method. `InfoListener` has three methods:

 - `onSuccess(String token)`: Payment successful. 
 - `onFailed(String exceptionCode)` : Payment Failed
 - `onUserCancelled()`: The user has cancelled the Payment.

Here is a sample of `InfoListener` and `Payment.showInfo()` usage:
```java
        Payment.showInfo(new InfoListener() {
                    @Override
                    public void onSuccess(String token) {
                        Toast.makeText(MainActivity.this, token, Toast.LENGTH_SHORT).show();
                    }

                    @Override
                    public void onFailed(String exceptionCode) {
                        Toast.makeText(MainActivity.this, exceptionCode, Toast.LENGTH_SHORT).show();
                    }

                    @Override
                    public void onUserCancelled() {

                    }
                },productCode , productItemCode, privateKey);}
```
Replace `productCode`, `productItemCode` and `privateKey` with your product info which defined in the [PIQO developer panel][1].

A sample project can be found in **sample** folder. 

[1]: https://pg.appson.ir/