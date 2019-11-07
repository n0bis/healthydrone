package API;

import com.github.dockerjava.core.DefaultDockerClientConfig;
import com.github.dockerjava.core.DockerClientBuilder;
import com.github.dockerjava.core.DockerClientConfig;
import org.modelmapper.ModelMapper;
import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.context.annotation.Bean;

import java.io.IOException;

@SpringBootApplication
public class Application {

    public static void main(String[] args) throws IOException {
        SpringApplication.run(Application.class, args);
        //var utmService = new UTMService();
        //utmService.clientTracking().updateFlight(55.355848, 10.440672);

        DockerClientConfig config = DefaultDockerClientConfig.createDefaultConfigBuilder().build();
        var dockerClient = DockerClientBuilder.getInstance(config).build();

        var container = dockerClient
                .createContainerCmd("jonasvautherin/px4-gazebo-headless:v1.9.2")
                .withEnv("PX4_HOME_LAT=55.3686619", "PX4_HOME_LON=10.4300876", "PX4_HOME_ALT=10")
                .withTty(true)
                .exec();

        dockerClient.startContainerCmd(container.getId()).exec();

        System.in.read();
    }

    @Bean
    public ModelMapper modelMapper() {
        return new ModelMapper();
    }
}
