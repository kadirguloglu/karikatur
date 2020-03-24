import { combineReducers } from "redux";
import cartoonService from "./cartoonService";
import notificationToken from "./notificationToken";

export default combineReducers({
  cartoonServiceResponse: cartoonService,
  notificationTokenResponse: notificationToken
});
