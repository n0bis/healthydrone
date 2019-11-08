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
    private Location pilotLocation;
    private double altitudeAGL;
    private String source;
}