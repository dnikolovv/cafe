import * as ActionTypes from "../../actions/actionTypes";
import initialState from "../../initialState";

export function authReducer(state = initialState.auth, action) {
  switch (action.type) {
    case ActionTypes.LOAD_CURRENT_USER_SUCCESS:
      return { ...state, user: action.user, isLoggedIn: true };
    case ActionTypes.LOGOUT_SUCCESS:
    case ActionTypes.LOAD_CURRENT_USER_FAILURE:
      return { ...state, user: {}, isLoggedIn: false };
    default:
      return state;
  }
}
