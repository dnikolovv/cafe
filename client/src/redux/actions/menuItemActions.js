import * as ActionTypes from "../actions/actionTypes";
import * as menuItemsApi from "../../api/menuItemsService";

export function loadMenuItemsSuccess(menuItems) {
  return { type: ActionTypes.LOAD_MENU_ITEMS_SUCCESS, menuItems };
}

export function addMenuItem(item) {
  return function(dispatch) {
    return menuItemsApi.addMenuItem(item).then(_ => {
      dispatch(loadMenuItems());
    });
  };
}

export function loadMenuItems() {
  return function(dispatch) {
    return menuItemsApi.getMenuItems().then(response => {
      dispatch(loadMenuItemsSuccess(response.items));
    });
  };
}
