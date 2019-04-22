import * as ActionTypes from "../../actions/actionTypes";
import initialState from "../../initialState";

export function baristaReducer(state = initialState.baristas, action) {
  switch (action.type) {
    case ActionTypes.LOAD_BARISTAS_SUCCESS:
      return action.baristas;
    default:
      return state;
  }
}
