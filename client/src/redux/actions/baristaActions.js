import * as ActionTypes from "../actions/actionTypes";
import * as baristaApi from "../../api/baristaService";

export function loadBaristasSuccess(baristas) {
  return { type: ActionTypes.LOAD_BARISTAS_SUCCESS, baristas };
}

export function loadBaristas() {
  return function(dispatch) {
    return baristaApi.getBaristas().then(response => {
      dispatch(loadBaristasSuccess(response.items));
    });
  };
}
