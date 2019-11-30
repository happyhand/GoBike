import React, { Component } from "react";
import { Doughnut } from "react-chartjs-2";
import PropTypes from "prop-types";

export default class PieChart extends Component {
  constructor(props) {
    super(props);
  }

  render() {
    const { datas } = this.props;
    let labels = [];
    let datasets = [{ data: [], backgroundColor: [], hoverBackgroundColor: [] }];
    datas.forEach(data => {
      labels.push(data.label);
      datasets[0].data.push(data.data);
      datasets[0].backgroundColor.push(data.color);
      datasets[0].hoverBackgroundColor.push(data.color);
    });
    let chartDatas = {
      labels: labels,
      datasets: datasets
    };

    return (
      <div>
        <Doughnut data={chartDatas} />
      </div>
    );
  }
}

PieChart.propTypes = {
  datas: PropTypes.array.isRequired
};
