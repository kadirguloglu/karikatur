import React, { useEffect } from "react";
import Constants from "expo-constants";
import * as Permissions from "expo-permissions";
import { Notifications } from "expo";
import { useDispatch } from "react-redux";

import { postNotificationToken } from "../src/actions/notificationToken";

import AppNavigator from "../navigation/AppNavigator";

function Main() {
  const dispatch = useDispatch();
  useEffect(() => {
    async function registerForPushNotificationsAsync() {
      try {
        const { status } = await Permissions.askAsync(
          Permissions.NOTIFICATIONS
        );
        if (status !== "granted") {
          // alert("No notification permissions!");
          return;
        }
        let token = await Notifications.getExpoPushTokenAsync();
        dispatch(postNotificationToken(Constants.installationId, token));
      } catch (error) {}
    }
    registerForPushNotificationsAsync();
    return () => {};
  }, []);

  return <AppNavigator />;
}

export default Main;
