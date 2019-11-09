package UTMService.Clients;

import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.core.type.TypeReference;
import com.fasterxml.jackson.databind.JsonMappingException;
import com.fasterxml.jackson.databind.ObjectMapper;
import okhttp3.*;

import java.io.IOException;

// TODO: change url to be modifiable
public class BaseClient<T> {

    public final MediaType JSON = MediaType.get("application/json; charset=utf-8");
    public final MediaType FORM = MediaType.parse("multipart/form-data");
    private String baseUrl = "https://healthdrone.unifly.tech";
    private String url;
    private  String method;
    private final OkHttpClient client;
    private final ObjectMapper mapper;
    private final TypeReference<T> tType;
    private Object body;

    BaseClient(OkHttpClient client, TypeReference<T> tType) {
        this.client = client;
        this.mapper = new ObjectMapper();
        this.tType = tType;
    }

    protected BaseClient<T> setUrl(String url) {
        this.url = url;
        return this;
    }

    protected BaseClient<T> setMethod(String method) {
        this.method = method;
        return this;
    }

    protected BaseClient<T> setBody(Object value) {
        this.body = value;
        return this;
    }

    protected Request createRequest() {
        var builder = new Request.Builder()
                .url(baseUrl + url)
                .method(method, createBody());
        builder.addHeader("Content-Type", JSON.toString());
        return builder.build();
    }

    protected T parseResponse(Response response) throws Exception {
        if (!response.isSuccessful()) {
            throw new Exception("Response failed with: " + response.code() + " error: " + response.message());
        }

        try (var body = response.body()) {
            String payload = body.string();
            if (payload.isEmpty())
                return null;

            return mapper.readValue(payload, tType);
        } catch (JsonMappingException e) {
            e.printStackTrace();
        } catch (JsonProcessingException e) {
            e.printStackTrace();
        } catch (IOException e) {
            e.printStackTrace();
        }
        return null;
    }

    protected RequestBody createBody() {
        try {
            var jsonBody = mapper.writeValueAsBytes(body);
            return RequestBody.create(JSON, jsonBody);
        } catch (JsonProcessingException e) {
            e.printStackTrace();
            return null;
        }
    }

    protected Request createFormRequest(String secret, String username, String password) {
        var builder = new Request.Builder()
                .url(baseUrl + url)
                .method(method, createFormBody(username, password))
                .addHeader("Authorization", "Basic " + secret);
        return builder.build();
    }

    protected RequestBody createFormBody(String username, String password) {
        var body = new FormBody.Builder()
                .add("username", username)
                .add("password", password)
                .add("grant_type", "password");
        return body.build();
    }

    protected T executeJson(Object value) throws IOException {
        this.setBody(value);
        var request = createRequest();
        try (var response = client.newCall(request).execute()) {
            return parseResponse(response);
        } catch (IOException e) {
            e.printStackTrace();
        } catch (Exception e) {
            e.printStackTrace();
        }
        return null;
    }

    protected T executeForm(String secret, String username, String password) throws IOException {
        var request = createFormRequest(secret, username, password);
        try (var response = client.newCall(request).execute()) {
            return parseResponse(response);
        } catch (IOException e) {
            e.printStackTrace();
        } catch (Exception e) {
            e.printStackTrace();
        }
        return null;
    }
}
