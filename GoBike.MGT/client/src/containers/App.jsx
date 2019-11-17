import React, { Component } from "react";
import { Route, HashRouter, Switch } from "react-router-dom";
import "../css/App.css";
import PrivateRouter from "../router/PrivateRouter";
import HomePage from "./HomePage";
import LoginPage from "./LoginPage";
import AccountManagerPage from "./AccountManagerPage";

export default class App extends Component {
  constructor(props) {
    super(props);
  }

  render() {
    return (
      <HashRouter>
        <Switch>
          <Route exact path="/Home">
            <HomePage />
          </Route>
          <Route path="/Login">
            <LoginPage />
          </Route>
          <Route path="/Home/Account">
            <AccountManagerPage />
          </Route>
        </Switch>
        <PrivateRouter component={HomePage} />
      </HashRouter>
    );
    // return <PrivateRouter component = {HomePage}></PrivateRouter>;
    // return <HomePage></HomePage>;
  }
}
