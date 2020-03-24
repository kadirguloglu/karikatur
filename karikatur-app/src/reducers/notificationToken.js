import {
  POST_NOTIFICATION_TOKEN,
  POST_NOTIFICATION_TOKEN_SUCCESS,
  POST_NOTIFICATION_TOKEN_FAIL
} from "../types/notificationToken";

const INITIAL_STATE = {};

export default (state = INITIAL_STATE, action) => {
  switch (action.type) {
    // case POST_NOTIFICATION_TOKEN:
    //   return { ...state };
    // case POST_NOTIFICATION_TOKEN_SUCCESS:
    //   return { ...state };
    // case POST_NOTIFICATION_TOKEN_FAIL:
    //   return { ...state };

    default:
      return { ...state };
  }
};
