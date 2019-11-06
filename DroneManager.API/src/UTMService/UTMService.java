package UTMService;

import UTMService.Clients.TokenClient;
import UTMService.Clients.TrackingClient;
import okhttp3.*;
import org.springframework.boot.context.properties.EnableConfigurationProperties;
import org.springframework.context.annotation.Configuration;

@Configuration
@EnableConfigurationProperties
public class UTMService {

    private final OkHttpClient httpClient;
    private final String token;

    public UTMService() {
        var client = new OkHttpClient();

        var tokenClient = new TokenClient(client);

        this.token = tokenClient.Auth().getAccessToken();

        httpClient = new OkHttpClient.Builder()
            .addInterceptor(chain -> {
                Request original = chain.request();
                Request.Builder builder = original.newBuilder().method(original.method(), original.body());
                builder.header("Authorization", "Bearer " + token);
                return chain.proceed(builder.build());
            })
            .build();
    }


    public TrackingClient clientTracking() {
        return new TrackingClient(httpClient);
    }
}
