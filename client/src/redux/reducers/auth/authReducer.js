import * as ActionTypes from "../../actions/actionTypes";
import initialState from "../../initialState";

export function authReducer(state = initialState.auth, action) {
  switch (action.type) {
    case ActionTypes.LOGIN_REQUEST_SUCCESS:
      return { ...state, isLoggedIn: true };
    case ActionTypes.LOGOUT_SUCCESS:
      return { ...state, isLoggedIn: false };
    default:
      return state;
  }
}
