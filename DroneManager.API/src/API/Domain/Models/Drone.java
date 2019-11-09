package API.Domain.Models;

import UTMService.Models.Location;
import com.fasterxml.jackson.annotation.JsonProperty;
import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

@AllArgsConstructor
@NoArgsConstructor
@Getter
@Setter
public class Drone {

    @JsonProperty("uniqueIdentifier")
    private String id;
    private String model;
    private String nickname;
    private Double speed;
    private Location homeLocation;
}
