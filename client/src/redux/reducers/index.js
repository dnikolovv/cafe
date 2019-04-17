import { combineReducers } from "redux";
import { authReducer as auth } from "../reducers/auth/authReducer";

const rootReducer = combineReducers({
  auth
});

export default rootReducer;
