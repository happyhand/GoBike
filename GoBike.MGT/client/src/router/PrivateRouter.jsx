import React, { Component } from "react";
import { connect } from "react-redux";
import { Route, Redirect } from "react-router-dom";
import { onAgentLogin, onAgentLogout } from "../actions/Action";

class PrivateRouter extends Component {
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

/**
 * 繫結 Redux State
 * @param {object} state
 */
function mapStateToProps(state) {
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

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(PrivateRouter);
