import * as ActionTypes from "../actions/actionTypes";
import * as managerApi from "../../api/managerService";

export function loadManagersSuccess(managers) {
  return { type: ActionTypes.LOAD_MANAGERS_SUCCESS, managers };
}

export function loadManagers() {
  return function(dispatch) {
    return managerApi.getManagers().then(response => {
      dispatch(loadManagersSuccess(response.items));
    });
  };
}
