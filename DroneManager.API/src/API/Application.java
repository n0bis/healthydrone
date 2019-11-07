package API;

import DroneSimulator.DroneSimulator;
import org.modelmapper.ModelMapper;
import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.context.annotation.Bean;

import java.io.IOException;

@SpringBootApplication
public class Application {

    public static void main(String[] args) {
        SpringApplication.run(Application.class, args);
        //var utmService = new UTMService();
        //utmService.clientTracking().updateFlight(55.355848, 10.440672);
    }

    @Bean
    public ModelMapper modelMapper() {
        return new ModelMapper();
    }

    @Bean
    public DroneSimulator droneSimulator() {
        return new DroneSimulator();
    }
}
