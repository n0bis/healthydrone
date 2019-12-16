import { get, action, observable, toJS } from "mobx";
import axios from "axios";

class DroneStore {
  @observable showDroneOptions = false;
  @observable drones = [];
  @observable dronesLocation = {};
  @observable drone = {};
  @observable droneManagers = [];

  fetchDrones = () => {
    const token = localStorage.getItem("authorization");

    axios
      .get(
        `https://healthdrone.unifly.tech/api/operators/38dc3c7a-915e-4409-b6d9-a0dc409d8808/uases/`,
        {
          headers: {
            Authorization: "Bearer " + token
          }
        }
      )
      .then(res => {
        const drones = res.data;

        drones.map(drone => {
          this.dronesLocation[drone.uniqueIdentifier] = {
            longitude: 10.40494,
            latitude: 55.35436,
            name: ""
          };

          this.setDroneManager(drone);
          this.setLocation(drone);
        });

        this.drones = drones;
      })
      .catch(error => {
        console.log(error);
      });
  };

  fetchManagers = () => {
    axios
      .get(`http://localhost:5000/manager`)
      .then(res => {
        const droneManagers = res.data;
        this.droneManagers = droneManagers;
      })
      .catch(error => {
        console.log(error);
      });
  };

  getDronePort = () => {
    const drone_id = this.drone.uniqueIdentifier;
    var port;

    this.droneManagers.map(drone => {
      //console.log(drone.droneId);
      if (drone.droneId == drone_id) {
        port = drone.port;
      }
    });

    return port;
  };

  stopDrone = () => {
    console.log(`stop: ${this.getDronePort()}`)
    axios.get(`http://localhost:${this.getDronePort()}/stop`);
  };

  sendHome = () => {
    axios.get(`http://localhost:${this.getDronePort()}/sendHome`);
  };

  putOnFlight = () => {
    const port = this.getDronePort();

    axios.post(`http://localhost:${port}/sendOnMission`, [
      {
        latitude: 55.474558,
        longitude: 10.325687
      },
      {
        latitude: 55.149776,
        longitude: 9.608144
      },
      {
        latitude: 55.261088,
        longitude: 9.162151
      }
    ]);
  };

  setDroneManager = drone => {
    const url = "http://localhost:5000/manager";
    axios.post(url, {
      id: drone.uniqueIdentifier,
      operationid: drone.uniqueIdentifier,
      velocity: 10.0,
      currentLocation: {
        latitude: 55.355853,
        longitude: 10.432952
      },
      homeLocation: {
        latitude: 55.355853,
        longitude: 10.432952
      }
    });
  };

  sendOnMission = coordinates => {
    const port = this.getDronePort();
    var cords = [];
    coordinates.map(cord => {
      // Reverse cords...
      cords.push({
        longitude: cord[0],
        latitude: cord[1]
      });
    });

    console.log(cords);

    axios.post(`http://localhost:${port}/sendOnMission`, cords).then(res => {
      return true;
    });
  };

  getDrone = (id) => {
    const drones = this.drones.filter(
      drone => drone.uniqueIdentifier === id
    );

    const drone = drones[0];
    //console.log(drone);
    if (typeof drone === "undefined") return false;

    return drone;
  };

  getFlightStatus = drone_id => {
    this.drones.map((drone, key) => {
      if (drone.uniqueIdentifier === drone_id) {
        return this.drones[key];
      }
    });
  };

  getDroneLocation = drone_id => {
    if (this.dronesLocation[drone_id]) {
      return this.dronesLocation[drone_id].name;
    } else {
      return "Søger..";
    }
  };

  getDroneLocationCords = drone_id => {
    if (this.dronesLocation[drone_id]) {
      return {
        longitude: this.dronesLocation[drone_id].longitude,
        latitude: this.dronesLocation[drone_id].latitude
      };
    }
  };

  updateDroneLocation = (drone_id, longitude, latitude) => {
    const token = localStorage.getItem("authorization");

    axios
      .get(
        `https://healthdrone.unifly.tech/api/map/locations?query=${latitude},${longitude}`,
        {
          headers: {
            Authorization: "Bearer " + token
          }
        }
      )
      .then(res => {

        this.dronesLocation[drone_id].name = res.data[0].name;
        this.dronesLocation[drone_id].latitude = latitude;
        this.dronesLocation[drone_id].longitude = longitude;
      })
      .catch(err => {
        console.log(err);
      });
  };

  setLocation = drone => {
    const latitude = this.dronesLocation[drone.uniqueIdentifier].latitude;
    const longitude = this.dronesLocation[drone.uniqueIdentifier].longitude;
    const token = localStorage.getItem("authorization");
    axios
      .get(
        `https://healthdrone.unifly.tech/api/map/locations?query=${latitude},${longitude}`,
        {
          headers: {
            Authorization: "Bearer " + token
          }
        }
      )
      .then(res => {
        this.dronesLocation[drone.uniqueIdentifier].name = res.data[0].name;
      })
      .catch(err => {
        console.log(err);
      });
  };

  setDroneStatus = (drone_id, status) => {
    this.drones.map((drone, key) => {
      if (drone.uniqueIdentifier === drone_id) {
        this.drones[key].flightStatus = status;
      }
    });
  };

  getDroneStatusColor = drone => ({
    "DANGER": "red",
    "IN_FLIGHT": "green",
    "LANDED": "grey"
  })[drone.flightStatus]

  setDroneStatusAndStop = (drone_id, status) => {
    this.drones.map((drone, key) => {
      if (drone.uniqueIdentifier === drone_id) {
        this.drones[key].flightStatus = status;
        this.drone = this.drones[key];
        stopDrone();
      }
    });
  };

  @action onClick = drone => {
    this.drone = drone;
    this.showDroneOptions = true;
  };
  @action closeDroneOptions = () => {
    this.showDroneOptions = false;
  };
}

const droneStore = new DroneStore();
export default droneStore;
