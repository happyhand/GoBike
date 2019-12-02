import React, { Component } from "react";
import Container from "react-bootstrap/Container";
import Row from "react-bootstrap/Row";
import Col from "react-bootstrap/Col";
import PieChart from "../components/PieChart";
import PieChartData from "../model/PieChartData";
import { Line } from "react-chartjs-2";

//#region Css
const pie = {
  textAlign: "center",
  color: "#666",
  fontSize: "16px"
};

//#endregion
interface IProp {}
export default class HomePage extends Component<IProp> {
  constructor(props: Readonly<IProp>) {
    super(props);
  }

  onLoadLoginData() {
    return {
      labels: ["January", "February", "March", "April", "May", "June", "July"],
      datasets: [
        {
          label: "My First dataset",
          fill: false,
          lineTension: 0,
          backgroundColor: "rgba(75,192,192,0.4)",
          borderColor: "rgba(75,192,192,1)",
          borderCapStyle: "butt",
          borderDash: [],
          borderDashOffset: 0.0,
          borderJoinStyle: "miter",
          pointBorderColor: "rgba(75,192,192,1)",
          pointBackgroundColor: "#fff",
          pointBorderWidth: 1,
          pointHoverRadius: 5,
          pointHoverBackgroundColor: "rgba(75,192,192,1)",
          pointHoverBorderColor: "rgba(220,220,220,1)",
          pointHoverBorderWidth: 2,
          pointRadius: 1,
          pointHitRadius: 10,
          data: [65, 59, 80, 81, 56, 55, 40]
        },
        {
          label: "My First dataset",
          fill: false,
          lineTension: 0.1,
          backgroundColor: "rgba(75,192,192,0.4)",
          borderColor: "rgba(75,192,192,1)",
          borderCapStyle: "butt",
          borderDash: [],
          borderDashOffset: 0.0,
          borderJoinStyle: "miter",
          pointBorderColor: "rgba(75,192,192,1)",
          pointBackgroundColor: "#fff",
          pointBorderWidth: 1,
          pointHoverRadius: 5,
          pointHoverBackgroundColor: "rgba(75,192,192,1)",
          pointHoverBorderColor: "rgba(220,220,220,1)",
          pointHoverBorderWidth: 2,
          pointRadius: 1,
          pointHitRadius: 10,
          data: [65, 59, 80, 81, 56, 55, 40]
        }
      ]
    };
  }

  onLoadRegisterData(): PieChartData[] {
    return [
      new PieChartData("Google", Math.round(Math.random() * 1000), "#FF6384"),
      new PieChartData("FB", Math.round(Math.random() * 1000), "#36A2EB"),
      new PieChartData("Local", Math.round(Math.random() * 1000), "#FFCE56")
    ];
  }

  onLoadTeamAreaData(): PieChartData[] {
    return [
      new PieChartData("台北", Math.round(Math.random() * 1000), "#FF6384"),
      new PieChartData("台中", Math.round(Math.random() * 1000), "#36A2EB"),
      new PieChartData("台南", Math.round(Math.random() * 1000), "#FFCE56"),
      new PieChartData("台東", Math.round(Math.random() * 1000), "#adffd1")
    ];
  }

  render() {
    return (
      <Container fluid>
        <Row>
          <Col md={3}>
            <Line data={this.onLoadLoginData()} />
          </Col>
        </Row>
        <Row>
          <Col md={3}>
            <div style={pie}>
              <span>總註冊來源分布圖</span>
              <PieChart data={this.onLoadRegisterData()}></PieChart>
            </div>
          </Col>
          <Col md={3}>
            <div style={pie}>
              <span>車隊地區分布圖</span>
              <PieChart data={this.onLoadTeamAreaData()}></PieChart>
            </div>
          </Col>
        </Row>
      </Container>
    );
  }
}
