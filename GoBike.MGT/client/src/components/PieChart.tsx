import React, { Component } from "react";
import { Doughnut } from "react-chartjs-2";
import PropTypes from "prop-types";
import PieChartData from "../model/PieChartData";

interface IProp {
  data: PieChartData[];
}
export default class PieChart extends Component<IProp> {
  static propTypes: { data: PropTypes.Validator<PieChartData[]> };
  constructor(props: Readonly<IProp>) {
    super(props);
  }

  render() {
    const { data } = this.props;
    let labels: Array<string> = [];
    let datas: number[] = [];
    let backgroundColors: string[] = [];
    let hoverBackgroundColors: string[] = [];
    data.forEach(item => {
      labels.push(item.label);
      datas.push(item.count);
      backgroundColors.push(item.color);
      hoverBackgroundColors.push(item.color);
    });
    let chartDatas = {
      labels: labels,
      datasets: [{ data: datas, backgroundColor: backgroundColors, hoverBackgroundColor: hoverBackgroundColors }]
    };

    return (
      <div>
        <Doughnut data={chartDatas} />
      </div>
    );
  }
}

PieChart.propTypes = {
  data: PropTypes.array.isRequired
};
