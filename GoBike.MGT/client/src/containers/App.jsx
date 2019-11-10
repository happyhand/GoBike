import React, { Component } from "react";
import { HashRouter } from "react-router-dom";
import { renderRoutes } from "react-router-config";
import routeConfig from "../router/RouterConfig";
import "../css/App.css";

export default class App extends Component {
  constructor(props) {
    super(props);
  }

  render() {
    return <HashRouter>{renderRoutes(routeConfig)}</HashRouter>;
  }
}
