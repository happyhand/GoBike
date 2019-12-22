import { AnyAction } from "redux";

export const onAgentLogin = (): AnyAction => {
  return {
    type: "AGENT_LOGIN"
  };
};

export const onAgentLogout = (): AnyAction => {
  return {
    type: "AGENT_LOGOUT"
  };
};

export const onLoginAction = (isValid: boolean, isLogin: boolean): AnyAction => {
  return {
    type: "LOGIN_ACTION",
    isValid: isValid,
    isLogin: isLogin
  };
};

export const onChangeMenu = (menuKey: string): AnyAction => {
  return {
    type: "CHANGE_MENU",
    menuKey: menuKey
  };
};

export const onLoadHomeTodayData = (isLoading: boolean, data: number[]): AnyAction => {
  return {
    type: "LOAD_HOME_TODAY_DATA",
    isLoading: isLoading,
    data: data
  };
};

export const onLoadHomeChartData = (isLoading: boolean, data: any[]): AnyAction => {
  return {
    type: "LOAD_HOME_CHART_DATA",
    isLoading: isLoading,
    data: data
  };
};

export const onLoadHomeChartDate = (startDate: Date, endDate: Date): AnyAction => {
  return {
    type: "LOAD_HOME_CHART_DATE",
    startDate: startDate,
    endDate: endDate
  };
};
