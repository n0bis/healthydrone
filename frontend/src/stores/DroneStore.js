import { get, action, observable, toJS } from "mobx";
import axios from "axios";

class DroneStore {
  @observable showDroneOptions = false;
  @observable drones = [
    {
      group_name: "Aktive",
      group_status: "active",
      drones: [
        {
          id: 1,
          name: "test"
        },
        {
          id: 2,
          name: "test"
        }
      ]
    },
    {
      group_name: "Parkeret",
      group_status: "parked",
      drones: [
        {
          id: 3,
          name: "test"
        },
        {
          id: 4,
          name: "test"
        }
      ]
    }
  ];

  @action onClick = () => {
    this.showDroneOptions = true;
  };
  @action closeDroneOptions = () => {
    this.showDroneOptions = false;
  };
}

const droneStore = new DroneStore();
export default droneStore;
