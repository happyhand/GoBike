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

export const onLoginLoading = (value: Boolean): AnyAction => {
  return {
    type: "LOGIN_LOADING",
    isLoading: value
  };
};

export const onLoginValid = (value: Boolean): AnyAction => {
  return {
    type: "LOGIN_VALID",
    isValid: value
  };
};
