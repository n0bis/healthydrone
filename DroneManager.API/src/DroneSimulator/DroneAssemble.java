package DroneSimulator;

import com.github.dockerjava.api.DockerClient;
import io.mavsdk.System;
import lombok.AllArgsConstructor;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

@AllArgsConstructor
@NoArgsConstructor
@Getter
@Setter
public class DroneAssemble {

    private System drone;
    private DockerClient mavsdk;
    private DockerClient droneContainer;

}
