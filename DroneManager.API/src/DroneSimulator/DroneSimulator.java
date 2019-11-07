package DroneSimulator;

import UTMService.UTMService;
import com.github.dockerjava.api.DockerClient;
import com.github.dockerjava.core.DefaultDockerClientConfig;
import com.github.dockerjava.core.DockerClientBuilder;
import com.github.dockerjava.core.DockerClientConfig;
import io.mavsdk.System;
import io.mavsdk.mission.Mission;

import java.util.ArrayList;
import java.util.List;

public class DroneSimulator {

    private final DockerClient dockerClient;
    private final UTMService utmService;

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

    public void sendDroneOnMission() {
        List<Mission.MissionItem> missionItems = new ArrayList<>();
        missionItems.add(generateMissionItem(55.370421, 10.436873));

        var drone = new System();

        drone.getTelemetry()
                .getPosition().subscribe(position -> utmService.clientTracking().updateFlight(position.getLatitudeDeg(), position.getLongitudeDeg()));

        drone.getMission()
                .setReturnToLaunchAfterMission(true)
                .andThen(drone.getMission().uploadMission(missionItems))
                .andThen(drone.getAction().arm())
                .andThen(drone.getAction().takeoff())
                .andThen(drone.getMission().startMission())
                .subscribe();
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
