package DroneSimulator;

import UTMService.UTMService;
import com.github.dockerjava.api.DockerClient;
import com.github.dockerjava.api.model.Network;
import com.github.dockerjava.api.model.Ports;
import com.github.dockerjava.core.DefaultDockerClientConfig;
import com.github.dockerjava.core.DockerClientBuilder;
import com.github.dockerjava.core.DockerClientConfig;
import io.mavsdk.System;
import io.mavsdk.mission.Mission;
import org.apache.commons.lang.StringUtils;

import java.util.ArrayList;
import java.util.List;
import java.util.concurrent.atomic.AtomicReference;

public class DroneSimulator {

    private final DockerClient dockerClient;
    private final UTMService utmService;
    private final List<System> drones = new ArrayList<>();

    public DroneSimulator() {
        DockerClientConfig config = DefaultDockerClientConfig.createDefaultConfigBuilder().build();
        this.dockerClient = DockerClientBuilder.getInstance(config).build();
        this.utmService = new UTMService();
    }

    public void createDrone() {
        var container = dockerClient
                .createContainerCmd("jonasvautherin/px4-gazebo-headless:v1.9.2")
                .withEnv("PX4_HOME_LAT=55.3686619", "PX4_HOME_LON=10.4300876", "PX4_HOME_ALT=10")
                .withTty(true)
                .exec();

        dockerClient.startContainerCmd(container.getId()).exec();

    }

    public String sendDroneOnMission() {
        AtomicReference<String> errorString = new AtomicReference<>("");
        List<Mission.MissionItem> missionItems = new ArrayList<>();
        missionItems.add(generateMissionItem(55.370421, 10.436873));

        var drone = new System();

        drone.getTelemetry()
                .getPosition().subscribe(position -> utmService.clientTracking().updateFlight(position.getLatitudeDeg(), position.getLongitudeDeg()));

        drone.getMission()
                .uploadMission(missionItems)
                .andThen(drone.getMission().setReturnToLaunchAfterMission(false))
                .andThen(drone.getAction().arm())
                .andThen(drone.getAction().takeoff())
                .andThen(drone.getMission().startMission())
                .doOnError(error -> errorString.set(error.getMessage()))
                .subscribe();

        return errorString.get().isEmpty() ? null : errorString.get();
    }

    private Mission.MissionItem generateMissionItem(double latitudeDeg, double longitudeDeg) {
        return new Mission.MissionItem(
                latitudeDeg,
                longitudeDeg,
                10f,
                10f,
                true,
                Float.NaN,
                Float.NaN,
                Mission.MissionItem.CameraAction.NONE,
                Float.NaN,
                1.0);
    }
}
