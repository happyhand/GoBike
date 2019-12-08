import React, { Component } from "react";
import { Doughnut } from "react-chartjs-2";
import PieChartData from "../model/PieChartData";
import { colors } from "../config/appconfig.json";
import { isNullOrUndefined } from "util";

interface IProp {
  datas: PieChartData[];
}

export default class PieChart extends Component<IProp> {
  constructor(props: Readonly<IProp>) {
    super(props);
  }

  render() {
    const { datas } = this.props;
    if (isNullOrUndefined(datas)) {
      return (
        <div>
          <span>Loading</span>
        </div>
      );
    }

    let data = {
      labels: datas.map(item => item.label),
      datasets: [
        {
          data: datas.map(item => item.count),
          backgroundColor: datas.map((item, index) => colors[index][0]),
          hoverBackgroundColor: datas.map((item, index) => colors[index][1]),
          hoverBorderWidth: 0
        }
      ]
    };
    return <Doughnut data={data} />;
  }
}
