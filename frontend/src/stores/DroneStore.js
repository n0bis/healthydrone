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
        `https://healthdrone.unifly.tech/api/operators/38dc3c7a-915e-4409-b6d9-a0dc409d8808/uases/`,
        {
          headers: {
            Authorization:
              "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ0cmFja2luZ0FsdGl0dWRlRmlsdGVyIjo0MDAwLjAsImF1ZCI6WyJ1c2VyTWFuYWdlbWVudFNlcnZpY2UiXSwiY2hhbm5lbHMiOlsiXCJvcGVyYXRvcjplNjMzM2ZjNC04YTcwLTQ5MDYtYWM2OC02N2YzYTFhYjdkMmZcIjpbXCJzdWJzY3JpYmVcIl0iLCJcIm9wZXJhdG9yOmU2MzMzZmM0LThhNzAtNDkwNi1hYzY4LTY3ZjNhMWFiN2QyZjoqXCI6W1wic3Vic2NyaWJlXCJdIiwiXCJvcGVyYXRvcjplNjMzM2ZjNC04YTcwLTQ5MDYtYWM2OC02N2YzYTFhYjdkMmY6dWFzOipcIjpbXCJzdWJzY3JpYmVcIl0iLCJcImZsaWdodFwiOltcInN1YnNjcmliZVwiXSIsIlwibmVhcmJ5OipcIjpbXCJzdWJzY3JpYmVcIl0iLCJcImFkc2I6bmVhcmJ5OipcIjpbXCJzdWJzY3JpYmVcIl0iLCJcImFkc2JcIjpbXCJzdWJzY3JpYmVcIl0iLCJcIm9wZXJhdG9yOjM4ZGMzYzdhLTkxNWUtNDQwOS1iNmQ5LWEwZGM0MDlkODgwOFwiOltcInN1YnNjcmliZVwiXSIsIlwib3BlcmF0b3I6MzhkYzNjN2EtOTE1ZS00NDA5LWI2ZDktYTBkYzQwOWQ4ODA4OipcIjpbXCJzdWJzY3JpYmVcIl0iLCJcIm9wZXJhdG9yOjM4ZGMzYzdhLTkxNWUtNDQwOS1iNmQ5LWEwZGM0MDlkODgwODp1YXM6KlwiOltcInN1YnNjcmliZVwiXSIsIlwiZmxpZ2h0XCI6W1wic3Vic2NyaWJlXCJdIiwiXCJuZWFyYnk6KlwiOltcInN1YnNjcmliZVwiXSIsIlwiYWRzYjpuZWFyYnk6KlwiOltcInN1YnNjcmliZVwiXSIsIlwiYWRzYlwiOltcInN1YnNjcmliZVwiXSJdLCJ1c2VyX25hbWUiOiJGUkhFTDE4QFNUVURFTlQuU0RVLkRLIiwic2NvcGUiOlsicmVhZCJdLCJleHAiOjE1NzUwNTU1NTgsImp0aSI6IjUyOWUyYzZkLWJmMDctNGU4OC1iNDEyLTg1OWI2NGQ5NjY3OSIsImNsaWVudF9pZCI6InNkdUhlYWx0aERyb25lQ29ubmVjdCIsInVzaWQiOiJiNjlmMjMwMi00Mjg0LTQyOGEtOTc0OC0wZTBmMmRhZjY1YzUifQ.v50rWyTI_lAR5caiHlZUlnLyhEEO77Wd-rXc1H5QvW8"
          }
        }
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
