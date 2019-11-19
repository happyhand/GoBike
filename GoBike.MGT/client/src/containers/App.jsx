import React, { Component } from "react";
import { Route, HashRouter, Switch } from "react-router-dom";
import PrivateRouter from "../router/PrivateRouter";
import HomePage from "./HomePage";
import LoginPage from "./LoginPage";
import ErrorPage from "./ErrorPage";
import "../css/App.css";

export default class App extends Component {
  constructor(props) {
    super(props);
  }

  render() {
    return (
      <HashRouter>
        <Switch>
          <Route exact path="/Login">
            <LoginPage />
          </Route>
          <PrivateRouter exact path="/" component={HomePage} />
          <PrivateRouter path="/Home" component={HomePage} />
          <Route path="*">
            <ErrorPage />
          </Route>
        </Switch>
      </HashRouter>
    );
  }
}
