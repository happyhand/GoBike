import React, { Component } from "react";
import { Dispatch } from "redux";
import { connect } from "react-redux";
import Container from "react-bootstrap/Container";
import Row from "react-bootstrap/Row";
import Col from "react-bootstrap/Col";
import { onChangeMenu } from "../actions/Action";

interface IProp {
  onChangeMenu: Function;
}

class MemberManagerPage extends Component<IProp> {
  static PAGE_PATH: string = "Member";
  constructor(props: Readonly<IProp>) {
    super(props);
  }

  componentDidMount() {
    const { onChangeMenu } = this.props;
    onChangeMenu("#" + MemberManagerPage.PAGE_PATH);
  }

  render() {
    return (
      <Container fluid>
        <Row>
          <Col>This Is Member Manager Page</Col>
        </Row>
      </Container>
    );
  }
}

/**
 * 繫結 Redux Action
 * @param {Dispatch} dispatch
 */
function mapDispatchToProps(dispatch: Dispatch) {
  return {
    onChangeMenu: (menuKey: string) => dispatch(onChangeMenu(menuKey))
  };
}

export default connect(null, mapDispatchToProps)(MemberManagerPage);
