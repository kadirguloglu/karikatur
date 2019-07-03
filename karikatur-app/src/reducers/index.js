import { combineReducers } from "redux";
import cartoonService from "./cartoonService";

export default combineReducers({
  cartoonServiceResponse: cartoonService
});
