import * as ActionTypes from "../actions/actionTypes";
import * as tablesApi from "../../api/tablesService";

export function loadTablesSuccess(tables) {
  return { type: ActionTypes.LOAD_TABLES_SUCCESS, tables };
}

export function callWaiterSuccess(tableNumber) {
  return { type: ActionTypes.CALL_WAITER_SUCCESS, tableNumber };
}

export function requestBillSuccess(tableNumber) {
  return { type: ActionTypes.REQUEST_BILL_SUCCESS, tableNumber };
}

export function loadTables() {
  return function(dispatch) {
    return tablesApi.loadTables().then(response => {
      dispatch(loadTablesSuccess(response.items));
    });
  };
}

export function addTable(table) {
  return function(dispatch) {
    return tablesApi.addTable(table).then(_ => {
      dispatch(loadTables());
    });
  };
}

export function callWaiter(tableNumber) {
  return function(dispatch) {
    return tablesApi.callWaiter(tableNumber).then(_ => {
      dispatch(callWaiterSuccess(tableNumber));
    });
  };
}

export function requestBill(tableNumber) {
  return function(dispatch) {
    return tablesApi.requestBill(tableNumber).then(_ => {
      dispatch(requestBillSuccess(tableNumber));
    });
  };
}
