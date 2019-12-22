import { AnyAction } from "redux";
import { isNullOrUndefined } from "util";

/**
 * 這邊可以放組合的 Reducer
 */
// export default combineReducers({});
const homeState = {
  isLoadTodayData: undefined,
  isLoadChartData: undefined,
  todayData: undefined,
  chartData: undefined,
  startDate: undefined,
  endDate: undefined
};
const reducer = (state = {}, action: AnyAction): any => {
  switch (action.type) {
    case "AGENT_LOGIN":
      localStorage.setItem("login", "true");
      return state;
    case "AGENT_LOGOUT":
      localStorage.removeItem("login");
      return state;
    case "LOGIN_ACTION":
      return { isValid: action.isValid, isLogin: action.isLogin };
    case "CHANGE_MENU":
      return { menuKey: action.menuKey };
    case "LOAD_HOME_TODAY_DATA":
      homeState.isLoadTodayData = action.isLoading;
      homeState.todayData = action.data;
      return cloneHomeState();
    case "LOAD_HOME_CHART_DATA":
      homeState.isLoadChartData = action.isLoading;
      homeState.chartData = action.data;
      return cloneHomeState();
    case "LOAD_HOME_CHART_DATE":
      homeState.startDate = isNullOrUndefined(action.startDate) ? homeState.startDate : action.startDate;
      homeState.endDate = isNullOrUndefined(action.endDate) ? homeState.endDate : action.endDate;
      return cloneHomeState();
    default:
      return state;
  }
};

const cloneHomeState = () => {
  return {
    isLoadTodayData: homeState.isLoadTodayData,
    isLoadChartData: homeState.isLoadChartData,
    todayData: homeState.todayData,
    chartData: homeState.chartData,
    startDate: homeState.startDate,
    endDate: homeState.endDate
  };
};

export default reducer;
