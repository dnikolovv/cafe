import * as ActionTypes from "../../actions/actionTypes";
import initialState from "../../initialState";

export function tabReducer(state = initialState.tabs, action) {
  switch (action.type) {
    case ActionTypes.LOAD_ALL_OPEN_TABS_SUCCESS:
      return { ...state, open: action.tabs };
    case ActionTypes.LOAD_PAST_TABS_HISTORY_SUCCESS:
      return { ...state, closed: action.tabs };
    case ActionTypes.LOAD_TAB_SUCCESS:
      return action.tab.isOpen
        ? { ...state, open: [...state.open, action.tab] }
        : { ...state, closed: [...state.closed, action.tab] };
    default:
      return state;
  }
}
