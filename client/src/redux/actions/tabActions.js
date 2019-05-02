import * as ActionTypes from "../actions/actionTypes";
import * as tabApi from "../../api/tabService";

export function loadAllTabsSuccess(tabs) {
  return { type: ActionTypes.LOAD_ALL_TABS_SUCCESS, tabs };
}

export function loadAllTabs() {
  return function(dispatch) {
    return tabApi.loadAllTabs().then(tabs => {
      dispatch(loadAllTabsSuccess(tabs));
    });
  };
}

export function rejectMenuItems(tabId, itemNumbers) {
  return function(dispatch) {
    return tabApi.rejectMenuItems(tabId, itemNumbers).then(_ => {
      dispatch(loadAllTabs());
    });
  };
}

export function orderMenuItems(tabId, itemNumbers) {
  return function(dispatch) {
    return tabApi.orderMenuItems(tabId, itemNumbers).then(_ => {
      dispatch(loadAllTabs());
    });
  };
}

export function closeTab(tabId, amountPaid) {
  return function(dispatch) {
    return tabApi.closeTab(tabId, amountPaid).then(_ => {
      dispatch(loadAllTabs());
    });
  };
}

export function openTab(tab) {
  return function(dispatch) {
    return tabApi.openTab(tab).then(_ => {
      dispatch(loadAllTabs());
    });
  };
}
