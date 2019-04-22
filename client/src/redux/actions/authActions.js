import * as ActionTypes from "./actionTypes";
import * as authApi from "../../api/authService";

export function logoutSuccess() {
  return { type: ActionTypes.LOGOUT_SUCCESS };
}

export function loadCurrentUserSuccess(user) {
  return { type: ActionTypes.LOAD_CURRENT_USER_SUCCESS, user };
}

export function loadCurrentUserFailure() {
  return { type: ActionTypes.LOAD_CURRENT_USER_FAILURE };
}

export function login(credentials) {
  return function(dispatch) {
    return authApi
      .login(credentials)
      .then(token => {
        localStorage.setItem("access_token", token.tokenString);
        dispatch(loadCurrentUser());
      })
      .catch(error => {
        dispatch(logout());
        throw error;
      });
  };
}

export function logout() {
  return function(dispatch) {
    localStorage.removeItem("access_token");
    dispatch(logoutSuccess());
  };
}

export function loadCurrentUser() {
  return function(dispatch) {
    return authApi
      .getCurrentUser()
      .then(user => {
        dispatch(loadCurrentUserSuccess(user));
      })
      .catch(() => {
        dispatch(loadCurrentUserFailure());
      });
  };
}
