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
public class Track {
    private String timestamp;
    private Location location;
    private int accuracy;
    private Location pilotLocation;
    private double altitudeMSL;
    private double altitudeAGL;
    private int altitudeAccuracy;
    private double heading;
    private double speed;
    private double battery;
    private int rss;
    private String source;
}