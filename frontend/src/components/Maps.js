import React, { Component } from "react";
import MapGL, {
  NavigationControl,
  Layer,
  Feature,
  FeatureState,
  Marker,
  Source
} from "@urbica/react-map-gl";
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

const style = {
  padding: "10%",
  width: "100px",
  height: "100px",
  color: "#fff",
  cursor: "pointer",
  background: "#1978c8",
  borderRadius: "6px"
};

@inject("mapStore")
@observer
class Maps extends Component {
  componentDidMount() {
    const { fetchLandingPoints } = this.props.mapStore;
    fetchLandingPoints();
  }

  onDrawCreate = ({ features }) => {
    console.log(features);
  };

  onDrawUpdate = ({ features }) => {
    console.log(features);
  };

  onClick = event => {
    console.log(test.lngLat);
    const lng = event.lngLat.lng;
    const lat = event.lngLat.lat;
  };

  createFlight = () => {
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
    const {
      data,
      isLoading,
      onChange,
      createFlight,
      dronesData,
      landingPoints,
      zoom,
      location,
      drawData
    } = this.props.mapStore;

    console.log("asdas: ", drawData);

    return (
      <div style={{ height: "100%" }}>
        <MapGL
          style={{ width: "100%", height: "90%" }}
          mapStyle="mapbox://styles/mapbox/light-v9"
          accessToken={
            "pk.eyJ1IjoiYXNkaW9qYXNvZGoiLCJhIjoiY2syMzNrYW40MDZwYjNicmVzd2lmN3RsNiJ9.iwnj30EcPWFknoJfWczWJg"
          }
          latitude={location.latitude}
          longitude={location.longitude}
          zoom={zoom}
          onClick={this.onClick}
        >
          <NavigationControl showCompass showZoom position="top-right" />
          {dronesData.map(drone => (
            <Marker
              style={style}
              longitude={drone.location[0]}
              latitude={drone.location[1]}
            >
              <h1>DRONE</h1>
            </Marker>
          ))}

          {landingPoints.map(point => (
            <Marker
              style={style}
              longitude={point.location[0]}
              latitude={point.location[1]}
            >
              <h1>{point.name}</h1>
            </Marker>
          ))}

          {!isLoading && (
            <Draw
              data={data}
              features={data}
              onChange={onChange}
              onDrawCreate={this.onDrawCreate}
              onDrawUpdate={this.onDrawUpdate}
            />
          )}
        </MapGL>

        <div>
          <p>{JSON.stringify(data)}</p>
          <button onClick={createFlight}>Create fligth</button>
        </div>
      </div>
    );
  }
}

export default Maps;

/*
Draw a path 

<Source id="route" type="geojson" data={test} />
          <Layer
            id="route"
            type="line"
            source="route"
            layout={{
              "line-join": "round",
              "line-cap": "round"
            }}
            paint={{
              "line-color": "#888",
              "line-width": 2
            }}
          />


              const test = {
      type: "Feature",
      geometry: {
        type: "LineString",
        coordinates: [
          [55.676098, 12.568337],
          [-122.48348236083984, 37.83317489144141],
          [-122.48339653015138, 37.83270036637107],
          [-122.48356819152832, 37.832056363179625],
          [-122.48404026031496, 37.83114119107971],
          [-122.48404026031496, 37.83049717427869],
          [-122.48348236083984, 37.829920943955045],
          [-122.48356819152832, 37.82954808664175],
          [-122.48507022857666, 37.82944639795659],
          [-122.48610019683838, 37.82880236636284],
          [-122.48695850372314, 37.82931081282506],
          [-122.48700141906738, 37.83080223556934],
          [-122.48751640319824, 37.83168351665737],
          [-122.48803138732912, 37.832158048267786],
          [-122.48888969421387, 37.83297152392784],
          [-122.48987674713133, 37.83263257682617],
          [-122.49043464660643, 37.832937629287755],
          [-122.49125003814696, 37.832429207817725],
          [-122.49163627624512, 37.832564787218985],
          [-122.49223709106445, 37.83337825839438],
          [-122.49378204345702, 37.83368330777276]
        ]
      }
    };
    */
