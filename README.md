# healthydrone


## Drone-simulator
Will start a docker container for a drone and a docker container for a server to control the drone


## Ups
The API is currently not stopping the docker containers upon stopping, so it would be a good idea to stop them with these commandos (first one stop all docker containers and second removes all docker containers)
docker stop $(docker ps -a -q)
docker rm $(docker ps -a -q)
