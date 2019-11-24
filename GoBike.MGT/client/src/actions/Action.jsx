export const onAgentLogin = () => {
  return {
    type: "AGENT_LOGIN"
  };
};

export const onAgentLogout = () => {
  return {
    type: "AGENT_LOGOUT"
  };
};

export const onLoginLoading = value => {
  return {
    type: "LOGIN_LOADING",
    isLoading: value
  };
};

export const onLoginValid = value => {
  return {
    type: "LOGIN_VALID",
    isValid: value
  };
};
