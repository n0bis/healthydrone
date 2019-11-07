package API;

import UTMService.UTMService;
import com.spotify.docker.client.DefaultDockerClient;
import com.spotify.docker.client.DockerClient;
import com.spotify.docker.client.LogStream;
import com.spotify.docker.client.exceptions.DockerCertificateException;
import com.spotify.docker.client.exceptions.DockerException;
import com.spotify.docker.client.messages.ContainerConfig;
import com.spotify.docker.client.messages.ExecCreation;
import com.spotify.docker.client.messages.HostConfig;
import org.modelmapper.ModelMapper;
import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.context.annotation.Bean;

import java.io.IOException;

@SpringBootApplication
public class Application {

    public static void main(String[] args) throws DockerCertificateException, DockerException, InterruptedException, IOException {
        SpringApplication.run(Application.class, args);
        //var utmService = new UTMService();
        //utmService.clientTracking().updateFlight(55.355848, 10.440672);

        var docker = DefaultDockerClient.fromEnv().build();

        docker.pull("jonasvautherin/px4-gazebo-headless:v1.9.2");

        var hostConfig = HostConfig.builder().build();

        var containerConfig = ContainerConfig.builder()
                .hostConfig(hostConfig)
                .image("jonasvautherin/px4-gazebo-headless:v1.9.2")
                .env("PX4_HOME_LAT=55.3686619", "PX4_HOME_LON=10.4300876", "PX4_HOME_ALT=10")
                .build();

        var creation = docker.createContainer(containerConfig);
        String id = creation.id();

        var info = docker.inspectContainer(id);
        System.out.println(info);
        docker.startContainer(id);

        var state = docker.inspectContainer(id).state();
        System.out.println(state);

        System.in.read();
    }

    @Bean
    public ModelMapper modelMapper() {
        return new ModelMapper();
    }
}
