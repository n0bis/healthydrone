package API.Controllers;

import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestMethod;
import org.springframework.web.bind.annotation.RestController;

@RestController
@RequestMapping("/")
public class DroneManagerController {

    @RequestMapping(method = RequestMethod.GET)
    public ResponseEntity getDrones(){
        return new ResponseEntity(HttpStatus.OK);
    }
}
