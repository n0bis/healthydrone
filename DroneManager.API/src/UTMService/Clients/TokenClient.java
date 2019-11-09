package UTMService.Clients;

import UTMService.Models.TokenHolder;
import com.fasterxml.jackson.core.type.TypeReference;
import okhttp3.OkHttpClient;

import java.io.IOException;
import java.util.Properties;

public class TokenClient extends BaseClient<TokenHolder> {

    @SuppressWarnings("SpringJavaInjectionPointsAutowiringInspection")
    public TokenClient(OkHttpClient client) {
        super(client, new TypeReference<TokenHolder>() {
        });
    }

    public TokenHolder Auth() {
        try (var input = TokenClient.class.getClassLoader().getResourceAsStream("/application.properties")) {
            var properties = new Properties();
            if (input != null)
                properties.load(input);

            String secret = "";//coalesce(properties.get("utm.secret").toString(), "");
            String username = "";//coalesce(properties.get("utm.username").toString(), "");
            String password = "";//coalesce(properties.get("utm.password").toString(), "");

            return this.setUrl("/oauth/token").setMethod("POST").executeForm(secret, username, password);

        } catch (IOException e) {
            e.printStackTrace();
            return null;
        }
    }

    public static <T> T coalesce(T one, T two) {
        return one != null ? one : two;
    }
}
