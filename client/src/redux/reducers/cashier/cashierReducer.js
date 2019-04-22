import * as ActionTypes from "../../actions/actionTypes";
import initialState from "../../initialState";

export function cashierReducer(state = initialState.cashiers, action) {
  switch (action.type) {
    case ActionTypes.LOAD_CASHIERS_SUCCESS:
      return action.cashiers;
    default:
      return state;
  }
}
