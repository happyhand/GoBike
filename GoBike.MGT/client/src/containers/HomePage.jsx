import React, { Component } from "react";
import { renderRoutes } from "react-router-config";

export default class HomePage extends Component {
  constructor(props) {
    super(props);
  }

  render() {
    return (
      <div>
        <h2>This Is Home Page</h2>
        {renderRoutes(this.props.route.routes)}
      </div>
    );
  }
}
