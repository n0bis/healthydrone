package UTMService.Models;

import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

/**
 *
 * @author madsfalken
 */
@AllArgsConstructor
@NoArgsConstructor
@Getter
@Setter
public class Location {
    private double longitude;
    private double latitude;
    private double altitude;

    public Location(double longitude, double latitude) {
        this.longitude = longitude;
        this.latitude = latitude;
    }
}
