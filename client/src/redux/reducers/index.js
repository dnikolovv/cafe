import { combineReducers } from "redux";
import { authReducer as auth } from "../reducers/auth/authReducer";
import { userReducer as users } from "../reducers/user/userReducer";
import { waiterReducer as waiters } from "../reducers/waiter/waiterReducer";
import { baristaReducer as baristas } from "../reducers/barista/baristaReducer";
import { cashierReducer as cashiers } from "../reducers/cashier/cashierReducer";
import { managerReducer as managers } from "../reducers/manager/managerReducer";
import { orderReducer as orders } from "../reducers/order/orderReducer";
import { tableReducer as tables } from "../reducers/table/tableReducer";
import { menuItemReducer as menuItems } from "../reducers/menuItem/menuItemReducer";
import { tabReducer as tabs } from "../reducers/tab/tabReducer";

const rootReducer = combineReducers({
  auth,
  users,
  waiters,
  baristas,
  cashiers,
  managers,
  orders,
  tables,
  tabs,
  menuItems
});

export default rootReducer;
