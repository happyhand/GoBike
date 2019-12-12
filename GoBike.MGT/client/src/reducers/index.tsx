import { AnyAction } from "redux";

/**
 * 這邊可以放組合的 Reducer
 */
// export default combineReducers({});
const reducer = (state = {}, action: AnyAction): any => {
  switch (action.type) {
    case "AGENT_LOGIN":
      localStorage.setItem("login", "true");
      return state;
    case "AGENT_LOGOUT":
      localStorage.removeItem("login");
      return state;
    case "LOGIN_ACTION":
      return { isValid: action.isValid, isLoading: action.isLoading };
    case "CHANGE_MENU":
      return { menuKey: action.menuKey };
    case "LOAD_HOME_DATA":
      return { isLoading: action.isLoading, data: action.data };
    default:
      return state;
  }
};

export default reducer;
