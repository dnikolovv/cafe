import * as ActionTypes from "../../actions/actionTypes";
import initialState from "../../initialState";

export function menuItemReducer(state = initialState.menuItems, action) {
  switch (action.type) {
    case ActionTypes.LOAD_MENU_ITEMS_SUCCESS:
      return action.menuItems;
    default:
      return state;
  }
}
