package UTMService.Clients;

import UTMService.Models.TokenHolder;
import com.fasterxml.jackson.core.type.TypeReference;
import okhttp3.OkHttpClient;

import java.io.IOException;
import java.util.Properties;

public class TokenClient extends BaseClient<TokenHolder> {

    @SuppressWarnings("SpringJavaInjectionPointsAutowiringInspection")
    public TokenClient(OkHttpClient client) {
        super(client,"https://healthdrone.unifly.tech/oauth/token", "POST", new TypeReference<TokenHolder>() {
        });
    }

    public TokenHolder Auth() {
        try (var input = TokenClient.class.getClassLoader().getResourceAsStream("application.properties")) {
            var properties = new Properties();
            properties.load(input);

            String secret = properties.get("utm.secret").toString();
            String username = properties.get("utm.username").toString();
            String password = properties.get("utm.password").toString();

            return this.executeForm(secret, username, password);

        } catch (IOException e) {
            e.printStackTrace();
            return null;
        }
    }
}
