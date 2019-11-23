import { get, action, observable, toJS } from "mobx";
import axios from "axios";

class DroneStore {
  @observable showDroneOptions = false;
  @observable drones = [];

  fetchDrones = () => {
    axios
      .get(
        `https://healthdrone.unifly.tech/api/operators/38dc3c7a-915e-4409-b6d9-a0dc409d8808/uases/`,
        {
          headers: {
            Authorization:
              "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ0cmFja2luZ0FsdGl0dWRlRmlsdGVyIjo0MDAwLjAsImF1ZCI6WyJ1c2VyTWFuYWdlbWVudFNlcnZpY2UiXSwiY2hhbm5lbHMiOlsiXCJvcGVyYXRvcjplNjMzM2ZjNC04YTcwLTQ5MDYtYWM2OC02N2YzYTFhYjdkMmZcIjpbXCJzdWJzY3JpYmVcIl0iLCJcIm9wZXJhdG9yOmU2MzMzZmM0LThhNzAtNDkwNi1hYzY4LTY3ZjNhMWFiN2QyZjoqXCI6W1wic3Vic2NyaWJlXCJdIiwiXCJvcGVyYXRvcjplNjMzM2ZjNC04YTcwLTQ5MDYtYWM2OC02N2YzYTFhYjdkMmY6dWFzOipcIjpbXCJzdWJzY3JpYmVcIl0iLCJcImZsaWdodFwiOltcInN1YnNjcmliZVwiXSIsIlwibmVhcmJ5OipcIjpbXCJzdWJzY3JpYmVcIl0iLCJcImFkc2I6bmVhcmJ5OipcIjpbXCJzdWJzY3JpYmVcIl0iLCJcImFkc2JcIjpbXCJzdWJzY3JpYmVcIl0iLCJcIm9wZXJhdG9yOjM4ZGMzYzdhLTkxNWUtNDQwOS1iNmQ5LWEwZGM0MDlkODgwOFwiOltcInN1YnNjcmliZVwiXSIsIlwib3BlcmF0b3I6MzhkYzNjN2EtOTE1ZS00NDA5LWI2ZDktYTBkYzQwOWQ4ODA4OipcIjpbXCJzdWJzY3JpYmVcIl0iLCJcIm9wZXJhdG9yOjM4ZGMzYzdhLTkxNWUtNDQwOS1iNmQ5LWEwZGM0MDlkODgwODp1YXM6KlwiOltcInN1YnNjcmliZVwiXSIsIlwiZmxpZ2h0XCI6W1wic3Vic2NyaWJlXCJdIiwiXCJuZWFyYnk6KlwiOltcInN1YnNjcmliZVwiXSIsIlwiYWRzYjpuZWFyYnk6KlwiOltcInN1YnNjcmliZVwiXSIsIlwiYWRzYlwiOltcInN1YnNjcmliZVwiXSJdLCJ1c2VyX25hbWUiOiJGUkhFTDE4QFNUVURFTlQuU0RVLkRLIiwic2NvcGUiOlsicmVhZCJdLCJleHAiOjE1NzQ0NTcwOTgsImp0aSI6IjFkMzg2ZWM1LWRiNmQtNDMwNS1hZmM2LWU0NTczYjI0NDVkZSIsImNsaWVudF9pZCI6InNkdUhlYWx0aERyb25lQ29ubmVjdCIsInVzaWQiOiIwNGRjNzAzNS1kMzI2LTRkM2UtOTJjMy1hZjA3MmYxNmYyYTgifQ.6pcMEzX7UXNTUypNOPDCB_OuJjj82HOI3MAWrXUyzIQ"
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
        console.log("sadas");
        this.drones[key].flightStatus = status;
      }
    });
  };

  @action onClick = () => {
    this.showDroneOptions = true;
  };
  @action closeDroneOptions = () => {
    this.showDroneOptions = false;
  };
}

const droneStore = new DroneStore();
export default droneStore;
