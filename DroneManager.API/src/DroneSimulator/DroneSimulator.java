package DroneSimulator;

import API.Domain.Models.Drone;
import API.Resources.MissionResource;
import UTMService.UTMService;
import com.github.dockerjava.api.DockerClient;
import com.github.dockerjava.api.model.ExposedPort;
import com.github.dockerjava.core.DefaultDockerClientConfig;
import com.github.dockerjava.core.DockerClientBuilder;
import com.github.dockerjava.core.DockerClientConfig;
import io.mavsdk.System;
import io.mavsdk.mission.Mission;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.concurrent.atomic.AtomicReference;

public class DroneSimulator {

    private final DockerClient dockerClient;
    private final UTMService utmService;
    private final Map<String, DroneAssemble> drones = new HashMap<String, DroneAssemble>();

    public DroneSimulator() {
        DockerClientConfig config = DefaultDockerClientConfig.createDefaultConfigBuilder().build();
        this.dockerClient = DockerClientBuilder.getInstance(config).build();
        this.utmService = new UTMService();
    }

    public void createDrone(Drone drone) {
        var mavsdkContainer = dockerClient
                .createContainerCmd("mavsdk_server")
                .withTty(true)
                .exec();

        dockerClient.startContainerCmd(mavsdkContainer.getId()).exec();

        var mavsdkIp = dockerClient.inspectContainerCmd(mavsdkContainer.getId())
                .exec().getNetworkSettings().getIpAddress();

        var droneContainer = dockerClient
                .createContainerCmd("jonasvautherin/px4-gazebo-headless:v1.9.2")
                .withCmd("0.0.0.0 " + mavsdkIp)
                .withEnv("PX4_HOME_LAT=55.3686619", "PX4_HOME_LON=10.4300876", "PX4_HOME_ALT=10")
                .withTty(true)
                .exec();

        dockerClient.startContainerCmd(droneContainer.getId()).exec();

        var droneSystem = new System(mavsdkIp, 50051);
        //this.drones.put(drone.getId(), new DroneAssemble(droneSystem, hh, hh));
    }

    public String sendDroneOnMission(String id, MissionResource resource) {
        AtomicReference<String> errorString = new AtomicReference<>("");
        List<Mission.MissionItem> missionItems = new ArrayList<>();
        resource.getMissions().forEach(mission -> missionItems.add(generateMissionItem(mission.getLatitude(), mission.getLongitude())));
        //missionItems.add(generateMissionItem(55.370421, 10.436873));

        var drone = new System();

        drone.getTelemetry()
                .getPosition().subscribe(position -> utmService.clientTracking().updateFlight(position.getLatitudeDeg(), position.getLongitudeDeg()));

        drone.getMission()
                .uploadMission(missionItems)
                .andThen(drone.getMission().setReturnToLaunchAfterMission(resource.getReturnToHomeAfterMission()))
                .andThen(drone.getAction().arm())
                .andThen(drone.getAction().takeoff()
                    .doOnComplete(() -> java.lang.System.out.println("send take off notification")))
                .andThen(drone.getMission().startMission())
                .doOnError(error -> errorString.set(error.getMessage()))
                .doOnComplete(() -> java.lang.System.out.println("send land notification"))
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
