import * as ActionTypes from "../actions/actionTypes";
import * as tabApi from "../../api/tabService";

export function loadAllOpenTabsSuccess(tabs) {
  return { type: ActionTypes.LOAD_ALL_OPEN_TABS_SUCCESS, tabs };
}

export function loadPastTabHistorySuccess(tabs) {
  return { type: ActionTypes.LOAD_PAST_TABS_HISTORY_SUCCESS, tabs };
}

export function loadTabSuccess(tab) {
  return { type: ActionTypes.LOAD_TAB_SUCCESS, tab };
}

export function loadAllOpenTabs() {
  return function(dispatch) {
    return tabApi.loadAllOpenTabs().then(response => {
      dispatch(loadAllOpenTabsSuccess(response.items));
    });
  };
}

export function loadPastTabHistory() {
  return function(dispatch) {
    return tabApi.getPastTabsHistory().then(response => {
      dispatch(loadPastTabHistorySuccess(response.items));
    });
  };
}

export function serveMenuItems(tabId, itemNumbers) {
  return function(dispatch) {
    return tabApi.serveMenuItems(tabId, itemNumbers).then(_ => {
      dispatch(loadAllOpenTabs());
    });
  };
}

export function rejectMenuItems(tabId, itemNumbers) {
  return function(dispatch) {
    return tabApi.rejectMenuItems(tabId, itemNumbers).then(_ => {
      dispatch(loadAllOpenTabs());
    });
  };
}

export function orderMenuItems(tabId, itemNumbers) {
  return function(dispatch) {
    return tabApi.orderMenuItems(tabId, itemNumbers).then(_ => {
      dispatch(loadAllOpenTabs());
    });
  };
}

export function closeTab(tabId, amountPaid) {
  return function(dispatch) {
    return tabApi.closeTab(tabId, amountPaid).then(_ => {
      dispatch(loadAllOpenTabs());
    });
  };
}

export function openTab(tab) {
  return function(dispatch) {
    return tabApi.openTab(tab).then(_ => {
      dispatch(loadAllOpenTabs());
    });
  };
}

export function loadTab(tabId) {
  return function(dispatch) {
    return tabApi.loadTab(tabId).then(tab => {
      dispatch(loadTabSuccess(tab));
    });
  };
}
