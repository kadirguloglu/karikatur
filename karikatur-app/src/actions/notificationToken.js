import {
  POST_NOTIFICATION_TOKEN,
  POST_NOTIFICATION_TOKEN_URL
} from "../types/notificationToken";
import { Platform } from "react-native";
import Constants from "expo-constants";

export function postNotificationToken(token) {
  return {
    type: POST_NOTIFICATION_TOKEN,
    payload: {
      request: {
        url: `${POST_NOTIFICATION_TOKEN_URL}`,
        method: "POST",
        data: {
          token: token,
          device: Constants.installationId,
          platform: Platform.OS
        }
      }
    }
  };
}
