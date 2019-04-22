import * as ActionTypes from "../actions/actionTypes";
import * as usersApi from "../../api/userService";

export function loadUsersSuccess(users) {
  return { type: ActionTypes.LOAD_USERS_SUCCESS, users };
}

export function loadUsers() {
  return function(dispatch) {
    return usersApi.getUsers().then(users => {
      dispatch(loadUsersSuccess(users));
    });
  };
}
