import { get, action, observable, toJS } from "mobx";
import axios from "axios";
import { observer } from "mobx-react-lite";
import { FormControlLabel } from "@material-ui/core";
var qs = require("qs");

class MapStore {
  @observable isLoading = false;
  @observable landing = null;
  @observable dronePos = [-122.36777130126949, 37.76914513791027];
  @observable b;
  @observable data = {
    type: "FeatureCollection",
    features: []
  };
  @observable dronesData = [];
  @observable landingPoints = [];

  /*
  @observable data = {
    type: "FeatureCollection",
    features: [
      {
        id: 1,
        type: "Feature",
        geometry: {
          type: "Point",
          coordinates: [-77.032, 38.913]
        },
        properties: {
          title: "Mapbox",
          description: "Washington, D.C."
        }
      }
    ]
  };
  */

  @action onChange = data => {
    this.data = data;
  };

  fetchLandingPoints = () => {
    this.landingPoints = [
      {
        name: "Odense",
        location: [10.40237, 55.403756]
      },
      {
        name: "Svendbord",
        location: [10.607282, 55.067434]
      }
    ];
  };

  /*
   ** Drones logic
   */

  setDroneLocation = (drone_id, lan, lon) => {
    // Check if drones is in array, then update the location
    var exists;
    this.dronesData.map((drone, key) => {
      if (drone.drone_id == drone_id) {
        this.dronesData[key].location = [lan, lon];
        exists = true;
      }
    });

    if (exists) return;

    // Drone is not yet created in the map, so we create it here
    this.dronesData = [
      ...this.dronesData,
      {
        drone_id: drone_id,
        location: [-77.032, 38.913]
      }
    ];
  };

  /*
  @action setLanding = e => {
    const landing = this.landingSpots.filter(item => {
      return e.target.value == item.id;
    });

    //if (!landing[0]) return (this.landing = landingSpots[0].value);

    this.landing = landing[0].value;
    console.log(this.landing);
  };
  */

  @action startFligth = () => {
    alert("Flight started");
  };

  @action createFlight = () => {
    axios.post("http://localhost:8001/sendOnMission", {
      headers: {
        "Content-Type": "application/json"
      },
      data: JSON.stringify([
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
    });
  };
}

const mapStore = new MapStore();
export default mapStore;
