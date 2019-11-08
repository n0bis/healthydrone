package API.Domain.Models;

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
    private double speed;
}
