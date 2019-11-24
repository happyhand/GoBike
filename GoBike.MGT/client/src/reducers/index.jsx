const initState = {
  isLoading: false,
  isValid: true
};

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
    case "LOGIN_LOADING":
      return { isValid: state.isValid, isLoading: action.isLoading };
    case "LOGIN_VALID":
      return { isValid: action.isValid, isLoading: state.isLoading };
    default:
      return state;
  }
};

export default reducer;
