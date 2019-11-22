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

export const onCheckLoginValidated = value => {
  return {
    type: "CHECK_LOGIN_VALIDATED",
    validated: value
  };
};
