import React, { Component } from "react";
import { Route, Redirect } from "react-router-dom";
import { connect } from "react-redux";
import { onAgentLogin, onAgentLogout } from "../actions/Action";

class PrivateRouter extends Component {
  constructor(props) {
    super(props);
    console.log(this.props);
  }

  render() {
    const { component: Component } = this.props;
    const isLogin = localStorage.getItem("isLogin");

    return (
      <Route
        render={props =>
          isLogin ? (
            <Component {...props} />
          ) : (
            <Redirect to={{ pathname: "/Login" }} />
          )
        }
      />
    );
  }
}

/**
 * 繫結 Redux State
 * @param {object} state
 */
function mapStateToProps(state) {
  console.log("PrivateRouter mapStateToProps >>>", state);
  return { isLogin: state.isLogin };
}

/**
 * 繫結 Redux Action
 * @param {function} dispatch
 */
function mapDispatchToProps(dispatch) {
  return {
    onAgentLogin: () => dispatch(onAgentLogin()),
    onAgentLogout: () => dispatch(onAgentLogout())
  };
}

export default connect(mapStateToProps, mapDispatchToProps)(PrivateRouter);
