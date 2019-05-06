import * as ActionTypes from "../actions/actionTypes";
import * as tablesApi from "../../api/tablesService";

export function loadTablesSuccess(tables) {
  return { type: ActionTypes.LOAD_TABLES_SUCCESS, tables };
}

export function loadTables() {
  return function(dispatch) {
    return tablesApi.loadTables().then(tables => {
      dispatch(loadTablesSuccess(tables));
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
