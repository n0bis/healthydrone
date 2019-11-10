package API.Controllers;

import API.Domain.Models.Drone;
import API.Resources.MissionResource;
import DroneSimulator.DroneSimulator;
import io.mavsdk.action.Action;
import io.mavsdk.mission.Mission;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.context.request.async.DeferredResult;

import java.io.IOException;
import java.util.concurrent.Callable;
import java.util.concurrent.ExecutionException;
import java.util.concurrent.Future;


@RestController
@RequestMapping("/")
public class DroneManagerController {

    @Autowired
    private DroneSimulator droneSimulator;

    @RequestMapping(method = RequestMethod.GET)
    public ResponseEntity getDrones(){
        return new ResponseEntity(HttpStatus.OK);
    }

    @RequestMapping(method = RequestMethod.POST)
    public DeferredResult<ResponseEntity> createDrone(@RequestBody Drone drone) throws InterruptedException {
        DeferredResult<ResponseEntity> result = new DeferredResult<>();

        new Thread(() -> {
            droneSimulator.createDrone(drone);
            result.setResult(new ResponseEntity<>(HttpStatus.OK));
        }).start();

        return result;
    }

    @RequestMapping(path= "/sendOnMission/{id}", method = RequestMethod.POST)
    public DeferredResult<ResponseEntity> sendDroneOnMission(@PathVariable("id") String id, @RequestBody MissionResource resource) throws ExecutionException, InterruptedException {
        //return () -> new ResponseEntity(droneSimulator.sendDroneOnMission(id, resource), HttpStatus.OK);
        DeferredResult<ResponseEntity> result = new DeferredResult<>();

        new Thread(() -> {
            String message = droneSimulator.sendDroneOnMission(id, resource);
            if (message != null) {
                result.setResult(new ResponseEntity(message, HttpStatus.BAD_REQUEST));
            } else {
                result.setResult(new ResponseEntity<>(HttpStatus.OK));
            }
        }).start();

        return result;
    }

}
