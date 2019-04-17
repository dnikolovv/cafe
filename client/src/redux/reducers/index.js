import { combineReducers } from "redux";
import { authReducer as auth } from "../reducers/auth/authReducer";
import { userReducer as users } from "../reducers/user/userReducer";

const rootReducer = combineReducers({
  auth,
  users
});

export default rootReducer;
