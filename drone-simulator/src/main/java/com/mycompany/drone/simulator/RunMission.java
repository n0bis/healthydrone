/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package com.mycompany.drone.simulator;

import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.github.dockerjava.api.DockerClient;
import com.github.dockerjava.api.command.CreateContainerResponse;
import com.github.dockerjava.core.DefaultDockerClientConfig;
import com.github.dockerjava.core.DockerClientBuilder;
import io.mavsdk.mission.Mission;
import io.mavsdk.System;
import java.io.IOException;
import java.util.ArrayList;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import java.util.List;
import java.util.concurrent.CountDownLatch;
import java.time.*;
import okhttp3.FormBody;
import okhttp3.MediaType;
import okhttp3.OkHttpClient;
import okhttp3.Request;
import okhttp3.RequestBody;
import okhttp3.Response;

/**
 *
 * @author madsfalken
 */
public class RunMission {

    private static final Logger logger = LoggerFactory.getLogger(RunMission.class);

    public static void main(String[] args) throws IOException {
        Runtime.getRuntime().exec("./mavsdk_server_macos");
        authUTMService();
        spinUpDocker();
        /*java.lang.System.out.println("Starting example: mission...");
        logger.debug("Starting example: mission...");

        List<Mission.MissionItem> missionItems = new ArrayList<>();
        missionItems.add(generateMissionItem(55.370421, 10.436873));*/
        /*missionItems.add(generateMissionItem(47.398036222362471, 8.5450146439425509));
        missionItems.add(generateMissionItem(47.397825620791885, 8.5450092830163271));
        missionItems.add(generateMissionItem(47.397832880000003, 8.5455939999999995));*/

        // new System("host", 12412);
        /*System drone = new System();
 
        drone.getAction().disarm();
        
        drone.getAction().setMaximumSpeed(0.1f);*/
        
        /*drone.getTelemetry()
                .getPosition().doOnEach(position -> updateFlight(position.getValue().getLatitudeDeg(), position.getValue().getLongitudeDeg())).subscribe();*/
        
        /*drone.getTelemetry()
                .getPosition().subscribe(position -> updateFlight(position.getLatitudeDeg(), position.getLongitudeDeg()));
        
        drone.getMission()
                .setReturnToLaunchAfterMission(true)
                .andThen(drone.getMission().uploadMission(missionItems)
                        .doOnComplete(() -> java.lang.System.out.println("Upload succeeded")))
                .andThen(drone.getAction().arm())
                .andThen(drone.getAction().takeoff())
                    .doOnComplete(() -> java.lang.System.out.println("Taking off..."))
                .andThen(drone.getMission().startMission()
                        .doOnComplete(() -> java.lang.System.out.println("Mission started")))
                .subscribe();
        
        CountDownLatch latch = new CountDownLatch(1);
        drone.getMission()
                .getMissionProgress()
                .filter(progress -> progress.getCurrentItemIndex() == progress.getMissionCount())
                .take(1)
                .subscribe(ignored -> latch.countDown());

        try {
            latch.await();
        } catch (InterruptedException ignored) {
            
        }*/
    }

    public static Mission.MissionItem generateMissionItem(double latitudeDeg, double longitudeDeg) {
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
    
    public static final MediaType JSON = MediaType.get("application/json; charset=utf-8");
    public static final MediaType FORM = MediaType.parse("multipart/form-data");
    private static String token;
    private static OkHttpClient client = new OkHttpClient();
    
    public static void authUTMService() {
        //var headers = Headers.of("");
        var body = new FormBody.Builder()
                .add("username", "")
                .add("password", "")
                .add("grant_type", "password")
                .build();
        var reuqest = new Request.Builder()
                .url("https://healthdrone.unifly.tech/oauth/token")
                .addHeader("Authorization", "Basic ")
                .post(body)
                .build();
        
        try (var response = client.newCall(reuqest).execute()) {
            var getToken = parseResponse(response);
            token = getToken.getAccessToken();
        } catch (IOException ex) {
            ex.printStackTrace();
        }
    }
    
    public static void updateFlight(double latitude, double longitude) throws JsonProcessingException {
        var track = new Track(Instant.now().toString(), new Location(longitude, latitude), 1, new Location(10.326345, 55.470852), 1, 3.5, 2, 90, 0.1, 15.0, 89, "simulation");
        var jsonBody = mapper.writeValueAsBytes(track);
        var body = RequestBody.create(JSON, jsonBody);
        var request = new Request.Builder()
                .url("https://healthdrone.unifly.tech/api/uasoperations/fb99e77b-4bfb-4a98-a32c-9717544b3903/uases/f7785735-5700-4e1c-a766-5ae7cbb4a4e3/track")
                .addHeader("Authorization", "Bearer " + token)
                .post(body)
                .build();
        
        try (Response response = client.newCall(request).execute()) {
            java.lang.System.out.println(response.code());
            java.lang.System.out.println(response.body().string());
        } catch (IOException ex) {
            ex.printStackTrace();
        }
    }
    
    private static ObjectMapper mapper = new ObjectMapper();
    
    private static TokenHolder parseResponse(Response response) {
        try (var body = response.body()) {
            String payload = body.string();
            return mapper.readValue(payload, TokenHolder.class);
        } catch (IOException ex) {
            ex.printStackTrace();
            return null;
        }
    }
    
    private static void spinUpDocker() {
        DefaultDockerClientConfig.Builder config = DefaultDockerClientConfig.createDefaultConfigBuilder();
        DockerClient dockerClient = DockerClientBuilder
            .getInstance(config)
            .build();
        
        CreateContainerResponse container = dockerClient.createContainerCmd("jonasvautherin/px4-gazebo-headless:v1.9.2")
            .withEnv("PX4_HOME_LAT=55.3686619", "PX4_HOME_LON=10.4300876", "PX4_HOME_ALT=10")
            .exec();

         dockerClient.startContainerCmd(container.getId()).exec();
    }
}
