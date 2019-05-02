import * as ActionTypes from "../../actions/actionTypes";
import initialState from "../../initialState";

export function tabReducer(state = initialState.tabs, action) {
  switch (action.type) {
    case ActionTypes.LOAD_ALL_TABS_SUCCESS:
      return action.tabs;
    case ActionTypes.LOAD_TAB_SUCCESS:
      return [...state, action.tab];
    default:
      return state;
  }
}
