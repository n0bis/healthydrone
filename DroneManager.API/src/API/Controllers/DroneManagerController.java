package API.Controllers;

import DroneSimulator.DroneSimulator;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestMethod;
import org.springframework.web.bind.annotation.RestController;


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
    public ResponseEntity createDrone() {
        droneSimulator.createDrone();

        return new ResponseEntity(HttpStatus.OK);
    }

    @RequestMapping(path= "sendDroneOnMission", method = RequestMethod.GET)
    public ResponseEntity sendDroneOnMission() {
        String message = droneSimulator.sendDroneOnMission();

        if (message != null)
            return new ResponseEntity(message, HttpStatus.BAD_REQUEST);

        return new ResponseEntity(HttpStatus.OK);
    }


}
