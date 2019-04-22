import * as ActionTypes from "../../actions/actionTypes";
import initialState from "../../initialState";

export function waiterReducer(state = initialState.waiters, action) {
  switch (action.type) {
    case ActionTypes.LOAD_WAITERS_SUCCESS:
      return action.waiters;
    default:
      return state;
  }
}
