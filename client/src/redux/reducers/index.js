import { combineReducers } from "redux";
import { authReducer as auth } from "../reducers/auth/authReducer";
import { userReducer as users } from "../reducers/user/userReducer";
import { waiterReducer as waiters } from "../reducers/waiter/waiterReducer";
import { baristaReducer as baristas } from "../reducers/barista/baristaReducer";
import { cashierReducer as cashiers } from "../reducers/cashier/cashierReducer";
import { managerReducer as managers } from "../reducers/manager/managerReducer";

const rootReducer = combineReducers({
  auth,
  users,
  waiters,
  baristas,
  cashiers,
  managers
});

export default rootReducer;
