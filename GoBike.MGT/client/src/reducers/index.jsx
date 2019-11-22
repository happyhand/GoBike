const initState = {};

/**
 * 這邊可以放組合的 Reducer
 */
// export default combineReducers({});
const reducer = (state = initState, action) => {
  switch (action.type) {
    case "AGENT_LOGIN":
      localStorage.setItem("isLogin", true);
      return state;
    case "AGENT_LOGOUT":
      localStorage.removeItem("isLogin");
      return state;
    case "CHECK_LOGIN_VALIDATED":
      return { validated: action.validated };
    default:
      return state;
  }
};

export default reducer;
