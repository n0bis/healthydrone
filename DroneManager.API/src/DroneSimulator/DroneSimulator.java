package DroneSimulator;

import API.Domain.Models.Drone;
import API.Resources.MissionResource;
import UTMService.UTMService;
import com.github.dockerjava.api.DockerClient;
import com.github.dockerjava.api.model.ExposedPort;
import com.github.dockerjava.api.model.Ports;
import com.github.dockerjava.core.DefaultDockerClientConfig;
import com.github.dockerjava.core.DockerClientBuilder;
import com.github.dockerjava.core.DockerClientConfig;
import io.mavsdk.System;
import io.mavsdk.mission.Mission;
import org.springframework.scheduling.annotation.Async;
import org.springframework.scheduling.annotation.AsyncResult;
import org.springframework.stereotype.Service;
import org.springframework.util.SocketUtils;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.concurrent.CompletableFuture;
import java.util.concurrent.ConcurrentHashMap;
import java.util.concurrent.Future;
import java.util.concurrent.atomic.AtomicReference;

@Service
public class DroneSimulator {

    private final DockerClient dockerClient;
    private final UTMService utmService;
    private final Map<String, DroneAssemble> drones = new ConcurrentHashMap<String, DroneAssemble>();

    public DroneSimulator() {
        DockerClientConfig config = DefaultDockerClientConfig.createDefaultConfigBuilder().build();
        this.dockerClient = DockerClientBuilder.getInstance(config).build();
        this.utmService = new UTMService();
    }

    @Async("asyncExecutor")
    public void createDrone(Drone drone) {
        ExposedPort tcp50051 = ExposedPort.tcp(50051);
        var port = SocketUtils.findAvailableTcpPort();
        Ports portsbinding = new Ports();
        portsbinding.bind(tcp50051, Ports.Binding.bindPort(port));

        var mavsdkContainer = dockerClient
                .createContainerCmd("mavsdk_server")
                .withExposedPorts(tcp50051)
                .withPortBindings(portsbinding)
                .withTty(true)
                .exec();

        dockerClient.startContainerCmd(mavsdkContainer.getId()).exec();

        /*var mavsdkIp = dockerClient.inspectContainerCmd(mavsdkContainer.getId())
                .exec().getNetworkSettings().getIpAddress();*/

        var droneContainer = dockerClient
                .createContainerCmd("px4-gazebo-headless")
                //.withCmd("0.0.0.0 " + mavsdkIp)
                .withEnv("PX4_HOME_LAT=" + drone.getHomeLocation().getLatitude(), "PX4_HOME_LON=" + drone.getHomeLocation().getLongitude(), "PX4_HOME_ALT=" + drone.getHomeLocation().getAltitude())
                .withTty(true)
                .exec();

        dockerClient.startContainerCmd(droneContainer.getId()).exec();

        var droneSystem = new System("localhost", port);

        this.drones.put(drone.getId(), new DroneAssemble(droneSystem, null, droneContainer));
    }

    @Async("asyncExecutor")
    public String sendDroneOnMission(String id, MissionResource resource) {
        AtomicReference<String> errorString = new AtomicReference<>("");
        List<Mission.MissionItem> missionItems = new ArrayList<>();
        resource.getMissions().forEach(mission -> missionItems.add(generateMissionItem(mission.getLatitude(), mission.getLongitude())));
        //missionItems.add(generateMissionItem(55.370421, 10.436873));

        var drone = this.drones.get(id).getDrone();

        drone.getTelemetry()
                .getPosition()
                .doOnError(error -> errorString.set(error.getMessage()))
                .subscribe(position -> utmService.clientTracking().updateFlight(id, resource.getOperationId(), position.getLatitudeDeg(), position.getLongitudeDeg()), (err) -> { err.printStackTrace(); errorString.set(err.getMessage());});

        drone.getMission()
                .uploadMission(missionItems)
                .andThen(drone.getMission().setReturnToLaunchAfterMission(resource.getReturnToHomeAfterMission()))
                .andThen(drone.getAction().arm())
                .andThen(drone.getAction().takeoff()
                    .doOnComplete(() -> java.lang.System.out.println("send take off notification")))
                .andThen(drone.getMission().startMission())
                .doOnError(error -> errorString.set(error.getMessage()))
                .doOnComplete(() -> java.lang.System.out.println("send land notification"))
                .subscribe(() -> { java.lang.System.out.println("completed"); }, (err) -> { err.printStackTrace(); errorString.set(err.getMessage());});

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
