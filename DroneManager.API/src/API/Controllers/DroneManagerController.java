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

import java.io.IOException;


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
    public ResponseEntity createDrone(@RequestBody Drone drone) {
        droneSimulator.createDrone(drone);

        return new ResponseEntity(HttpStatus.OK);
    }

    @RequestMapping(path= "/sendOnMission/{id}", method = RequestMethod.POST)
    public ResponseEntity sendDroneOnMission(@PathVariable("id") String id, @RequestBody MissionResource resource) {
        String message = droneSimulator.sendDroneOnMission(id, resource);

        if (message != null)
            return new ResponseEntity(message, HttpStatus.BAD_REQUEST);

        return new ResponseEntity(HttpStatus.OK);
    }

}
