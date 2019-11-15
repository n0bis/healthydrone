# healthydrone


## Drone-simulator
First build the Dockerfile found in the root folder
`docker build -t <name> .`
And then run a container injecting all the environment variables or else make a docker-compose build injecting the variables.
To spin up a drone:

`docker run -p <port>:80 -e ENVIRONMENT=Development -e Drone__id="<id-of-the-drone>" -e Drone__operationid="<operation-id>" -e Drone__velocity="<speed of the drone>" -e Drone__location__latitude="<current-latitude-location>" -e Drone__location__longitude="<current-longitude-location>" -e Drone__homelocation__latitude="<home-latitude-location>" -e Drone__homelocation__longitude="<home-longitude-location>" -e UTM__clientid="<clientid>" -e UTM__clientsecret="<clientsecret>" -e UTM__username="<username>" -e UTM__password="<password>" dronesimulator:latest`

To be able to run multiple drones you can change the port for each drone, because each container will run a rest api.
Drone__id: the id of the drone from UTM
Drone__operationid: id of the operation where the drone is flying from UTM
Drone__velocity: the spped of the drone
Drone__location (latitude & longitude): the current location of the drone or where it will start
Drone__homelocation (latitude & longitude): the home location of the drone where the drone will fly to when sent home

UTM__clintid: the given clientid from UTM
UTM__clientsecret: the given clientsecret from UTM
UTM__username: a username to login with
UTM__password: the password for the user


### Note
Double __ will be converted to : for the appsettings.json file and ENVIRONMENT for which appsettings.json file to use
