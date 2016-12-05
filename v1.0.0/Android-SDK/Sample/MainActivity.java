
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.TextView;
import android.widget.Toast;

import ir.appson.PIQOlibrary.Appson;
import ir.appson.PIQOlibrary.Interface.InfoListener;
import ir.appson.PIQOlibrary.Payment;

public class MainActivity extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        Appson.init(MainActivity.this);

        TextView ProductOnfoButton = (TextView) findViewById(R.id.info_button);
        final String fakeProductCode = "PRODUCT_CODE";
        final String fakeProductItemCode = "PRODUCT_ITEM_CODE";
        final String fakePublicKey = "YOUR_PUBLIC_KEY";

        ProductOnfoButton.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
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
                },fakeProductCode , fakeProductItemCode, fakePublicKey);}
        });
    }
}
