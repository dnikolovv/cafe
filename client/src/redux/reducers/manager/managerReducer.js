import * as ActionTypes from "../../actions/actionTypes";
import initialState from "../../initialState";

export function managerReducer(state = initialState.managers, action) {
  switch (action.type) {
    case ActionTypes.LOAD_MANAGERS_SUCCESS:
      return action.managers;
    default:
      return state;
  }
}
