import { get, action, observable, toJS } from "mobx";
import axios from "axios";
import { observer } from "mobx-react-lite";
import { FormControlLabel } from "@material-ui/core";

class MapStore {
  @observable isLoading = false;
  @observable landing = null;
  @observable dronePos = [-122.36777130126949, 37.76914513791027];
  @observable b;
  @observable data = {
    type: "FeatureCollection",
    features: [
      {
        id: "test",
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

  @observable landingSpots = [
    {
      id: 1,
      name: "Svendbord",
      value: [-55.676098, 12.568337]
    },
    {
      id: 2,
      name: "København",
      value: [55.676098, 2.568337]
    }
  ];

  @action onChange = data => {
    this.data = data;
  };

  setDroneLocation = id => {
    const drones = this.data.features.filter(
      drone => drone.geometry.type === "Point"
    );

    drones.map(drone => {
      // ?
    });
  };

  @action setLanding = e => {
    const landing = this.landingSpots.filter(item => {
      return e.target.value == item.id;
    });

    //if (!landing[0]) return (this.landing = landingSpots[0].value);

    this.landing = landing[0].value;
    console.log(this.landing);
  };

  @action startFligth = () => {
    alert("Flight started");
  };

  @action createFlight = (a, b) => {
    /**/
    this.isLoading = true;

    var data = toJS(this.data);

    return console.log(data);

    data.features.push({
      type: "Feature",
      properties: {},
      geometry: {
        coordinates: [this.landing, [-122.36777130126949, 37.76914513791027]],
        type: "LineString"
      }
    });

    this.data = data;
    this.isLoading = false;

    //this.data.features = test;

    /*
    this.data.features.push({
      type: "Feature",
      properties: {},
      geometry: {
        coordinates: [
          [55.676098, 12.568337],
          [-122.36777130126949, 37.76914513791027]
        ],
        type: "LineString"
      }
    });*/
    //console.log(toJS(this.data));
  };
}

const mapStore = new MapStore();
export default mapStore;
