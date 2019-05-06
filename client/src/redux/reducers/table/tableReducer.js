import * as ActionTypes from "../../actions/actionTypes";
import initialState from "../../initialState";

export function tableReducer(state = initialState.tables, action) {
  switch (action.type) {
    case ActionTypes.LOAD_TABLES_SUCCESS:
      return action.tables;
    default:
      return state;
  }
}
