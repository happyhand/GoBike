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

export const onLoginAction = (isValid: boolean, isLoading: boolean): AnyAction => {
  return {
    type: "LOGIN_ACTION",
    isValid: isValid,
    isLoading: isLoading
  };
};

export const onLoadHomeData = (isLoading: boolean, data: any): AnyAction => {
  return {
    type: "LOAD_HOME_DATA",
    isLoading: isLoading,
    data: data
  };
};
