import { get, action, observable, toJS } from "mobx";
import axios from "axios";

class DroneStore {
  @observable showDroneOptions = false;
  @observable drones = [];
  @observable drone = {};

  fetchDrones = () => {
    axios
      .get(
        `https://healthdrone.unifly.tech/api/operators/38dc3c7a-915e-4409-b6d9-a0dc409d8808/uases/`,
        {
          headers: {
            Authorization:
              "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ0cmFja2luZ0FsdGl0dWRlRmlsdGVyIjo0MDAwLjAsImF1ZCI6WyJ1c2VyTWFuYWdlbWVudFNlcnZpY2UiXSwiY2hhbm5lbHMiOlsiXCJvcGVyYXRvcjozOGRjM2M3YS05MTVlLTQ0MDktYjZkOS1hMGRjNDA5ZDg4MDhcIjpbXCJzdWJzY3JpYmVcIl0iLCJcIm9wZXJhdG9yOjM4ZGMzYzdhLTkxNWUtNDQwOS1iNmQ5LWEwZGM0MDlkODgwODoqXCI6W1wic3Vic2NyaWJlXCJdIiwiXCJvcGVyYXRvcjozOGRjM2M3YS05MTVlLTQ0MDktYjZkOS1hMGRjNDA5ZDg4MDg6dWFzOipcIjpbXCJzdWJzY3JpYmVcIl0iLCJcImZsaWdodFwiOltcInN1YnNjcmliZVwiXSIsIlwibmVhcmJ5OipcIjpbXCJzdWJzY3JpYmVcIl0iLCJcImFkc2I6bmVhcmJ5OipcIjpbXCJzdWJzY3JpYmVcIl0iLCJcImFkc2JcIjpbXCJzdWJzY3JpYmVcIl0iXSwidXNlcl9uYW1lIjoiSk9MVU4xOEBTVFVERU5ULlNEVS5ESyIsInNjb3BlIjpbInJlYWQiXSwiZXhwIjoxNTc1MDU5NDkxLCJqdGkiOiI1N2Q2YTNmMi1lMGZhLTQyY2QtYmU2My0wYTYxZmQ3NjQ3Y2EiLCJjbGllbnRfaWQiOiJzZHVIZWFsdGhEcm9uZUNvbm5lY3QiLCJ1c2lkIjoiYzI3OTQxMzYtOWY0OS00NDI3LTg0ZWMtYzVjYTEwMWM2ZThjIn0.6vQH8Tu-XoecjWmNZK--zHr7rv8cyOaQzhhNcCtVuxE"
          }
        }
      )
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
