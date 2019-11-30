import { get, action, observable, toJS } from "mobx";
import axios from "axios";

class DroneStore {
  @observable showDroneOptions = false;
  @observable drones = [];
  @observable drone = {};
  @observable droneManagers = [];

  fetchDrones = () => {
    axios
      .get(
        `https://healthdrone.unifly.tech/api/operators/38dc3c7a-915e-4409-b6d9-a0dc409d8808/uases/`
      )
      .then(res => {
        const drones = res.data;

        //axios.post("http://localhost:5000", )

        const url = "http://localhost:5000/manager";

        drones.map(drone => {
          console.log(drones);
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
      console.log(drone.droneId);
      if (drone.droneId == drone_id) {
        port = drone.port;
      }
    });

    return port;
  };

  stopDrone = () => {
    axios.get(`http://localhost:${this.getDronePort()}/stop`);
  };

  sendHome = () => {
    axios.get(`http://localhost:${this.getDronePort()}/sendHome`);
  };

  putOnFlight = (drone_id, path) => {
    const port = this.getDronePort();

    axios
      .post(`http://localhost:${port}/sendOnMission`, [
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
      ])
      .then(res => {
        this.drones = res.data;
      })
      .catch(error => {
        console.log(error);
      });
  };

  getDrone = () => {
    const drone_id = this.currentDrone;
    const drones = this.drones.filter(
      drone => drone.uniqueIdentifier === drone_id
    );

    const drone = drones[0];
    console.log(drone);
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

  setDroneStatus = (drone_id, status) => {
    this.drones.map((drone, key) => {
      if (drone.uniqueIdentifier === drone_id) {
        console.log("ASOIDJSA");
        this.drones[key].flightStatus = status;
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
