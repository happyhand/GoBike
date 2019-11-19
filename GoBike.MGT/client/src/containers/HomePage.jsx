import React, { Component } from "react";
import { connect } from "react-redux";
import { Switch } from "react-router-dom";
import AccountManagerPage from "./AccountManagerPage";
import PrivateRouter from "../router/PrivateRouter";

class HomePage extends Component {
  constructor(props) {
    super(props);
  }

  render() {
    return (
      <div>
        <h2>This Is Home Page</h2>
        <Switch>
          <PrivateRouter path="/Home/Account" component={AccountManagerPage} />
        </Switch>
      </div>
    );
  }
}

/**
 * 繫結 Redux State
 * @param {object} state
 */
function mapStateToProps(state) {
  return state;
}

export default connect(mapStateToProps)(HomePage);
