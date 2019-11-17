import React, { Component } from "react";
import { connect } from "react-redux";
import { renderRoutes } from "react-router-config";

class HomePage extends Component {
  constructor(props) {
    super(props);
  }

  render() {
    return (
      <div>
        <h2>This Is Home Page</h2>
      </div>
      // <div>
      //   Login: {isLogin.toString()}
      //   <button onClick={onAgentLogin}>login</button>
      //   <button onClick={onAgentLogout}>logout</button>
      // </div>
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
