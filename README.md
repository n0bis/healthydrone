# healthydrone


## Drone-simulator
First 
Download mavsdk_server from https://github.com/mavlink/MAVSDK/releases or if you use mac just use the mavsdk_server found under drone-simulator
And then run it.

To start a drone run:
PX4 environment variables sets home coordinates of the drone

docker run --rm -it --env PX4_HOME_LAT=55.3686619
 --env PX4_HOME_LON=10.4300876
 --env PX4_HOME_ALT=10 jonasvautherin/px4-gazebo-headless:v1.9.2
