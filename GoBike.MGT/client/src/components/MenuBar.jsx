import React, { Component } from "react";
import { connect } from "react-redux";
import { withRouter } from "react-router-dom";
import { onAgentLogout } from "../actions/Action";
import Nav from "react-bootstrap/Nav";
import Navbar from "react-bootstrap/Navbar";
import "../css/Button.css";

//#region Css
const menuBar = {
  backgroundColor: "#2b5971",
  borderRadius: "0px"
};

const menuLink = {
  fontFamily: "Noto Sans TC",
  fontSize: "16px"
};

//#endregion

class MenuBar extends Component {
  constructor(props) {
    super(props);
    this.handleLogout = this.handleLogout.bind(this);
  }

  handleLogout(evt) {
    const { onAgentLogout } = this.props;
    const isLogout = confirm("確定要登出嗎?");
    if (isLogout) {
      onAgentLogout();
      this.props.history.push("/Login");
    }
  }

  render() {
    return (
      <Navbar expand="lg" variant="dark" style={menuBar}>
        <Nav className="mr-auto">
          <Nav.Link href="#Home" style={menuLink}>
            首頁
          </Nav.Link>
          <Nav.Link href="#Home/Account" style={menuLink}>
            帳號管理
          </Nav.Link>
          <Nav.Link href="#Home/Member" style={menuLink}>
            會員管理
          </Nav.Link>
        </Nav>
        <Nav>
          <Nav.Item>
            <button className="Logout" onClick={this.handleLogout}>
              Logout
            </button>
          </Nav.Item>
        </Nav>
      </Navbar>
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

/**
 * 繫結 Redux Action
 * @param {function} dispatch
 */
function mapDispatchToProps(dispatch) {
  return {
    onAgentLogout: () => dispatch(onAgentLogout())
  };
}

export default withRouter(connect(mapStateToProps, mapDispatchToProps)(MenuBar));
