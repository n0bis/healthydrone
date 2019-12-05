import { get, action, observable, toJS } from "mobx";
import axios from "axios";

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

  @observable drawData = {};

  @observable zoom = 3;
  @observable location = {
    latitude: 55.676098,
    longitude: 12.568337
  };

  setZoom = () => {
    this.zoom = 10;
  };

  setLocation = (longitude, latitude) => {
    this.location = {
      longitude: longitude,
      latitude: latitude
    };
  };

  @action onChange = data => {
    this.data = data;
  };

  setDraw = (a, b) => {
    this.data = {
      type: "FeatureCollection",
      features: [
        {
          id: "4292189fb5f95d003b31b6c31a4ba504",
          type: "Feature",
          properties: {},
          geometry: {
            coordinates: [a, b],
            type: "LineString"
          }
        }
      ]
    };
  };

  refreshData = () => {
    this.data = {};
  };

  setFligthPath = coordinates => {
    this.data = {
      type: "FeatureCollection",
      features: [
        {
          id: "f1c87cb952f0e836713dba229070e1cb",
          type: "Feature",
          properties: {},
          geometry: {
            coordinates: coordinates,
            type: "LineString"
          }
        }
      ]
    };
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
      },
      {
        name: "Falck",
        location: [10.40494, 55.35436]
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
