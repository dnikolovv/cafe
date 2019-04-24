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

export function completeToGoOrder(orderId) {
  return function(dispatch) {
    return orderApi.completeToGoOrder(orderId).then(_ => {
      dispatch(loadOrders());
    });
  };
}

export function confirmToGoOrder(pricePaid, orderId) {
  return function(dispatch) {
    return orderApi.confirmToGoOrder(pricePaid, orderId).then(_ => {
      dispatch(loadOrders());
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
