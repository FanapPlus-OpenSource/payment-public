package ir.appson.paymettest;

import android.os.Bundle;
import android.support.v7.app.AppCompatActivity;
import android.view.View;
import android.widget.TextView;
import android.widget.Toast;

import ir.appson.identitylibrary.Appson;
import ir.appson.identitylibrary.Interface.InfoListener;
import ir.appson.identitylibrary.Payment;

public class MainActivity extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        Appson.init(MainActivity.this);

        final String fakeProductCode = "PRD-TEST-SDK";
        final String fakeProductItemCode = "PRD-TEST-SDK-d2c439fa-67f5-4fa3-be06-ac651b593858";
        final String fakePrivateKey = "MIICdgIBADANBgkqhkiG9w0BAQEFAASCAmAwggJcAgEAAoGBAIPLzZvD3Dhkr4tByRSGufPe1B1AYkZtf1aXrSdKz7/hPLMSRsdCYCWutVKY/nLOi63e/rVeRAF4U3v7ZTOxj9FtfwcMDGsJxJYw2L5PE83RV5LPmmkK4tAoJeackiXpW2L20NoR91akafRHDCYeJzD88TzElWPrGRPzD2oqJkGlAgMBAAECgYByNz2hHrSLXp5OxZ0YH8Wo1VPnYbOJKz1ucpXJdmjh9bUfztftuNUP8v8KbLIeFmcwMA92aBHxYDChQnUqvldOPH0g/LPEUADVga1Cg4ppQ1q7AzJ18cPS4tx4xopI1xaLmO3lzuCIIjfwhfmu97U/dJ2FXJZcB/GELszOdzOGgQJBAMqJt4z0F/OKqAyd15TeFZbszQcsUBQ1uUYLSdHhhJgYkSOUApP6jqA0uIWtI8U74uXeglVdNouLV0QUEsTCJUUCQQCmlchAY32UWSj08Gft9KDr2RKN0C3Ch9ZEGBS3dNnMaDJdA6qEltvq++ZjrI9WmMuueBid7UGdY4URvo2I7YDhAkEAu/Hl6R/NZgsB/IswNQ2M/TuK2qAtQ0PDRJNPEjryfu01Kc28QrNcTJ//ptRPAESdPfAoA6z247EO7rat3/XE/QJAMHfL98/6rbrzS7DWzEksuPJDj7dOWRckpFNNU8NPy0VVwfJbHpC2E7yO39lJKyzxtiWDJA/v01ctVGhYzcosgQJANjrHCa/YNisyzT2d9hEzp1SAqCv9FwkGxXwUi50/JHjGBt/7NVBZd5hLIPL9EXyGtKV9u2klGXzVvVZPWf5klA==";


        TextView txtBuy = (TextView) findViewById(R.id.txtBuy);
        txtBuy.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {

                Payment.showInfo(new InfoListener() {
                    @Override
                    public void onSuccess(String token) {
//                        do something with token
                    }

                    @Override
                    public void onFailed(String exceptionCode) {
                        Toast.makeText(MainActivity.this, exceptionCode, Toast.LENGTH_SHORT).show();
                    }

                    @Override
                    public void onUserCancelled() {

                    }
                }, fakeProductCode, fakeProductItemCode, fakePrivateKey);
            }
        });
    }
}
