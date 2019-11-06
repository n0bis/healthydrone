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
        super(client,"https://healthdrone.unifly.tech/api/uasoperations/fb99e77b-4bfb-4a98-a32c-9717544b3903/uases/f7785735-5700-4e1c-a766-5ae7cbb4a4e3/track", "POST", new TypeReference<Response>() {
        });
    }

    public void updateFlight(double latitude, double longitude) {
        var track = new Track(Instant.now().toString(), new Location(longitude, latitude), 1, new Location(10.326345, 55.470852), 1, 3.5, 2, 90, 0.1, 15.0, 89, "simulation");
        try {
            this.executeJson(track);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}
