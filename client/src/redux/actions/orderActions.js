import * as ActionTypes from "../actions/actionTypes";
import * as orderApi from "../../api/orderService";

export function loadOrdersSuccess(orders) {
  return { type: ActionTypes.LOAD_ORDERS_SUCCESS, orders };
}

export function loadOrders() {
  return function(dispatch) {
    return orderApi.getOrders().then(orders => {
      dispatch(loadOrdersSuccess(orders));
    });
  };
}

export function issueToGoOrder(itemNumbers) {
  return function(dispatch) {
    return orderApi.issueToGoOrder(itemNumbers).then(_ => {
      dispatch(loadOrders());
    });
  };
}
