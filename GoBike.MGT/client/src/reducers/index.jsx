import { combineReducers } from "redux";

const initState = {
  isLogin: false
};

/**
 * 這邊可以放組合的 Reducer
 */
// export default combineReducers({});
const reducer = (state = initState, action) => {
  switch (action.type) {
    case "AGENT_LOGIN":
      localStorage.setItem("isLogin", true);
      return { isLogin: true };
    case "AGENT_LOGOUT":
      localStorage.setItem("isLogin", false);
      return { isLogin: false };
    default:
      return state;
  }
};

export default reducer;
