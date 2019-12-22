import { isNullOrUndefined } from "util";
export default class Utility {
  static getDateFormat(date: Date, format: string) {
    if (isNullOrUndefined(format) || format == "") {
      return date.toLocaleString();
    }

    format = format.replace("yyyy", date.getFullYear().toString());
    format = format.replace("MM", Utility.getNumberString(date.getMonth() + 1, 2));
    format = format.replace("dd", Utility.getNumberString(date.getDate(), 2));

    format = format.replace("HH", Utility.getNumberString(date.getHours(), 2));
    format = format.replace(
      "hh",
      date.getHours() < 12
        ? "AM " + Utility.getNumberString(date.getHours(), 2)
        : date.getHours() == 12
        ? "PM 12"
        : "PM " + Utility.getNumberString(date.getHours() - 12, 2)
    );
    format = format.replace("mm", Utility.getNumberString(date.getMinutes(), 2));
    format = format.replace("ss", Utility.getNumberString(date.getSeconds(), 2));
    return format;
  }

  static getNumberString(number: number, digit: number): string {
    let ns = number.toString();
    while (ns.length < digit) {
      ns = "0" + ns;
    }

    return ns;
  }

  static getDateGroups(startDate: Date, endDate: Date): string[] {
    let sDate = new Date(startDate.getFullYear(), startDate.getMonth(), startDate.getDate());
    let eDate = new Date(endDate.getFullYear(), endDate.getMonth(), endDate.getDate());
    let ds = [];
    while (sDate <= eDate) {
      let date = sDate.getDate();
      ds.push(date == 1 ? sDate.getMonth() + 1 + "/1" : sDate.getDate().toString());
      sDate.setDate(date + 1);
    }

    return ds;
  }
}
