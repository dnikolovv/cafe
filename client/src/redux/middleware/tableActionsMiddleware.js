import * as tablesApi from "../../api/tablesService";
import * as ActionTypes from "../actions/actionTypes";

const tableActionsMiddleware = store => next => action => {
  if (action.type === ActionTypes.LOAD_CURRENT_USER_SUCCESS) {
    if (action.user.isWaiter) {
      tablesApi.subscribeToTableActions();
    }
  }

  next(action);
};

export default tableActionsMiddleware;
