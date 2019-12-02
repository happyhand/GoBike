import React, { Component } from "react";
import { Route, Redirect } from "react-router-dom";
import PropTypes from "prop-types";

interface IProp {
  exact: boolean;
  path: string;
  component: any;
}

export default class PrivateRouter extends Component<IProp> {
  static propTypes: {
    exact: PropTypes.Validator<boolean>;
    path: PropTypes.Validator<string>;
    component: PropTypes.Validator<any>;
  };
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

PrivateRouter.propTypes = {
  exact: PropTypes.bool.isRequired,
  path: PropTypes.string.isRequired,
  component: PropTypes.any.isRequired
};
