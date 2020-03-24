import {
  POST_NOTIFICATION_TOKEN,
  POST_NOTIFICATION_TOKEN_URL
} from "../types/notificationToken";

export function postNotificationToken(device, token) {
  return {
    type: POST_NOTIFICATION_TOKEN,
    payload: {
      request: {
        url: `${POST_NOTIFICATION_TOKEN_URL}`,
        method: "POST",
        data: {
          token: token,
          device: device
        }
      }
    }
  };
}
