import * as ActionTypes from "../../actions/actionTypes";
import initialState from "../../initialState";

const STATUS_PENDING = 0;
const STATUS_ISSUED = 1;
const STATUS_COMPLETED = 2;

export function orderReducer(state = initialState.orders, action) {
  switch (action.type) {
    case ActionTypes.LOAD_ORDERS_SUCCESS:
      return {
        pending: action.orders.filter(o => o.status === STATUS_PENDING),
        issued: action.orders.filter(o => o.status === STATUS_ISSUED),
        completed: action.orders.filter(o => o.status === STATUS_COMPLETED)
      };
    default:
      return state;
  }
}
