import React, { Component } from "react";
import { Route, HashRouter, Switch } from "react-router-dom";
import PrivateRouter from "../router/PrivateRouter";
import ContainerPage from "./ContainerPage";
import LoginPage from "./LoginPage";
import ErrorPage from "./ErrorPage";
import "../css/App.css";

export default class App extends Component {
  constructor(props: Readonly<{}>) {
    super(props);
  }

  render() {
    return (
      <HashRouter>
        <Switch>
          <Route exact path="/Login">
            <LoginPage />
          </Route>
          <PrivateRouter exact={false} path="/" component={ContainerPage} />
          <Route path="*">
            <ErrorPage />
          </Route>
        </Switch>
      </HashRouter>
    );
  }
}
