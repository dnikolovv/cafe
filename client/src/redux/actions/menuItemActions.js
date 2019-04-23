import * as ActionTypes from "../actions/actionTypes";
import * as menuItemsApi from "../../api/menuItemsService";

export function loadMenuItemsSuccess(menuItems) {
  return { type: ActionTypes.LOAD_MENU_ITEMS_SUCCESS, menuItems };
}

export function loadMenuItems() {
  return function(dispatch) {
    return menuItemsApi.getMenuItems().then(menuItems => {
      dispatch(loadMenuItemsSuccess(menuItems));
    });
  };
}
