import * as ActionTypes from "../actions/actionTypes";
import * as waitersApi from "../../api/waiterService";

export function loadWaitersSuccess(waiters) {
  return { type: ActionTypes.LOAD_WAITERS_SUCCESS, waiters };
}

export function loadWaiters() {
  return function(dispatch) {
    return waitersApi.getWaiters().then(response => {
      dispatch(loadWaitersSuccess(response.items));
    });
  };
}

export function hireWaiter(waiter) {
  return function(dispatch) {
    return waitersApi.hireWaiter(waiter).then(_ => {
      dispatch(loadWaiters());
    });
  };
}

export function assignTable(assignment) {
  return function(dispatch) {
    return waitersApi.assignTable(assignment).then(_ => {
      dispatch(loadWaiters());
    });
  };
}
