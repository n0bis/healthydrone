import React, { Component } from "react";
import ReactMapboxGl from "react-map-gl";
import { fromJS } from "immutable";
//import "@mapbox/mapbox-gl-draw/dist/mapbox-gl-draw.css";

import DrawControl from "react-mapbox-gl-draw";

//import "@mapbox/mapbox-gl-draw/dist/mapbox-gl-draw.css";
/*
const Map = ReactMapboxGl({
  accessToken:
    "pk.eyJ1IjoiYXNkaW9qYXNvZGoiLCJhIjoiY2syMzNrYW40MDZwYjNicmVzd2lmN3RsNiJ9.iwnj30EcPWFknoJfWczWJg"
});*/

class Maps extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      viewport: {
        width: "100%",
        height: "100%",
        latitude: 55.297879,
        longitude: 10.517822,
        zoom: 8,
        center: [-122.486052, 37.830348],
        mapboxApiAccessToken:
          "pk.eyJ1IjoiYXNkaW9qYXNvZGoiLCJhIjoiY2syMzNrYW40MDZwYjNicmVzd2lmN3RsNiJ9.iwnj30EcPWFknoJfWczWJg"
      }
    };
  }

  componentDidMount() {
    /*const map = this.reactMap.getMap();

    map.on("load", () => {
      map.on("mousemove", () => {
        alert(1);
      });
    });*/
  }

  render() {
    return (
      <ReactMapboxGl
        viewport={this.state.viewport}
        mapboxApiAccessToken="pk.eyJ1IjoiYXNkaW9qYXNvZGoiLCJhIjoiY2syMzNrYW40MDZwYjNicmVzd2lmN3RsNiJ9.iwnj30EcPWFknoJfWczWJg"
      ></ReactMapboxGl>
    );
  }
}

export default Maps;
