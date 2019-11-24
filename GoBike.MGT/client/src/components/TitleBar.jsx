import React, { Component } from "react";
import { connect } from "react-redux";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faHome } from "@fortawesome/free-solid-svg-icons";
import Container from "react-bootstrap/Container";
import Row from "react-bootstrap/Row";
import Col from "react-bootstrap/Col";

<i class="fab fa-earlybirds"></i>;

//#region Css
const bar = {
  backgroundColor: "#2b5971",
  padding: "10px 0px 0px 20px"
};

const title = {
  fontFamily: "Noto Sans TC",
  fontSize: "24px",
  color: "#fff"
};

const col = {
  paddingRight: "0px"
};
//#endregion

class TitleBar extends Component {
  constructor(props) {
    super(props);
  }

  render() {
    return (
      <Container fluid style={bar}>
        <Row>
          <Col md="auto" style={col}>
            <FontAwesomeIcon icon={faHome} size="3x" color="#fff" />
          </Col>
          <Col md="auto" style={col}>
            <span style={title}>加樂設計</span>
          </Col>
        </Row>
      </Container>
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

export default connect(mapStateToProps)(TitleBar);
