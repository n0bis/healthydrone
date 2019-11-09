package UTMService.Clients;

import UTMService.Models.Location;
import UTMService.Models.Response;
import UTMService.Models.Track;
import com.fasterxml.jackson.core.type.TypeReference;
import okhttp3.OkHttpClient;

import java.io.IOException;
import java.time.Instant;

public class TrackingClient extends BaseClient<Response> {

    public TrackingClient(OkHttpClient client) {
        super(client, new TypeReference<Response>() {
        });
    }

    public void updateFlight(String droneId, String operationId, double latitude, double longitude) {
        var track = new Track(Instant.now().toString(), new Location(longitude, latitude), new Location(10.326345, 55.470852), 3.5, "simulation");
        try {
            this.setUrl("/api/uasoperations/" + operationId + "/uases/" + droneId + "/track").setMethod("POST").executeJson(track);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}
