import * as ActionTypes from "../actions/actionTypes";
import * as cashierApi from "../../api/cashierService";

export function loadCashiersSuccess(cashiers) {
  return { type: ActionTypes.LOAD_CASHIERS_SUCCESS, cashiers };
}

export function loadCashiers() {
  return function(dispatch) {
    return cashierApi.getCashiers().then(response => {
      dispatch(loadCashiersSuccess(response.items));
    });
  };
}
