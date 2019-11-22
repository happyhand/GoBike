import React, { Component } from "react";
import { Route, Redirect } from "react-router-dom";

export default class PrivateRouter extends Component {
  constructor(props) {
    super(props);
  }

  render() {
    const { exact, path, component: Component } = this.props;
    const isLogin = Boolean(localStorage.getItem("isLogin"));
    if (isLogin) {
      return (
        <Route exact={exact} path={path}>
          <Component></Component>
        </Route>
      );
    }

    return <Redirect to="/Login"></Redirect>;
  }
}
