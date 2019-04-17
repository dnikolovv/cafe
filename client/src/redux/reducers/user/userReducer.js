import * as ActionTypes from "../../actions/actionTypes";
import initialState from "../../initialState";

export function userReducer(state = initialState.users, action) {
  switch (action.type) {
    case ActionTypes.LOAD_USERS_SUCCESS:
      return action.users;
    default:
      return state;
  }
}
