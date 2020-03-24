import { combineReducers } from "redux";
import cartoonService from "./cartoonService";
import notificationToken from "./notificationToken";
import settings from "./settings";

export default combineReducers({
  cartoonServiceResponse: cartoonService,
  notificationTokenResponse: notificationToken,
  settingsResponse: settings
});
