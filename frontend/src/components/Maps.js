import React, { Component } from "react";
import MapGL, { NavigationControl, Layer, Feature } from "@urbica/react-map-gl";
import Draw from "@urbica/react-map-gl-draw";
import { inject, observer } from "mobx-react";
import "mapbox-gl/dist/mapbox-gl.css";
import "@mapbox/mapbox-gl-draw/dist/mapbox-gl-draw.css";
/*
const Map = ReactMapboxGl({
  accessToken:
    "pk.eyJ1IjoiYXNkaW9qYXNvZGoiLCJhIjoiY2syMzNrYW40MDZwYjNicmVzd2lmN3RsNiJ9.iwnj30EcPWFknoJfWczWJg",
  center: [-77.04, 38.907]
});
*/

@inject("mapStore")
@observer
class Maps extends Component {
  componentDidMount() {
    const { setDroneLocation } = this.props.mapStore;
    setDroneLocation(1);
  }

  onDrawCreate = ({ features }) => {
    console.log(features);
  };

  onDrawUpdate = ({ features }) => {
    console.log(features);
  };

  createFlight = () => {
    alert(1);
    this.setState({
      data: {
        type: "FeatureCollection",
        features: [
          {
            type: "Feature",
            properties: {},
            geometry: {
              coordinates: [
                [55.676098, 12.568337],
                [-122.36777130126949, 37.76914513791027]
              ],
              type: "LineString"
            }
          }
        ]
      }
    });
  };

  render() {
    const { data, isLoading, onChange, createFlight } = this.props.mapStore;
    console.log(data);
    const test = data;
    const tests = [
      {
        type: "Feature",
        properties: {},
        geometry: {
          coordinates: [
            [50.676098, 12.568337],
            [-2.36777130126949, 37.76914513791027]
          ],
          type: "LineString"
        }
      }
    ];
    return (
      <div style={{ height: "100%" }}>
        <MapGL
          style={{ width: "100%", height: "90%" }}
          mapStyle="mapbox://styles/mapbox/light-v9"
          accessToken={
            "pk.eyJ1IjoiYXNkaW9qYXNvZGoiLCJhIjoiY2syMzNrYW40MDZwYjNicmVzd2lmN3RsNiJ9.iwnj30EcPWFknoJfWczWJg"
          }
          latitude={55.676098}
          longitude={12.568337}
          zoom={8}
        >
          <NavigationControl showCompass showZoom position="top-right" />
          {!isLoading && (
            <Draw
              data={test}
              features={test}
              onChange={onChange}
              onDrawCreate={this.onDrawCreate}
              onDrawUpdate={this.onDrawUpdate}
            />
          )}
        </MapGL>

        <div>
          <p>{JSON.stringify(test)}</p>
          <button onClick={createFlight}>Create fligth</button>
        </div>
      </div>
    );
  }
}

export default Maps;
