import React, { useEffect, useState } from "react";
import Constants from "expo-constants";
import * as Permissions from "expo-permissions";
import { Notifications } from "expo";
import { useDispatch } from "react-redux";
import { Spinner } from "native-base";
import { View, AsyncStorage } from "react-native";

import { postNotificationToken } from "../src/actions/notificationToken";
import { getProjectDetail } from "../src/actions/settings";

import AppNavigator from "../navigation/AppNavigator";

function Main() {
  const [getVersionLoading, setGetVersionLoading] = useState(true);
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
    async function checkVersion() {
      console.log("LOG: ----------------------------------------");
      console.log("LOG: checkVersion -> Constants", Constants);
      console.log("LOG: ----------------------------------------");
      return;
      try {
        let versionNumber = await AsyncStorage.getItem("@versionNumber");
        dispatch(getProjectDetail()).then(({ payload }) => {
          if (versionNumber) {
            AsyncStorage.setItem("@versionNumber", payload.data.VersionNumber);
            setGetVersionLoading(false);
          } else if (
            versionNumber &&
            payload.data.VersionNumber !== parseInt(versionNumber)
          ) {
            AsyncStorage.setItem("@versionNumber", payload.data.VersionNumber);
            if (payload.data.UpdateRequired) {
              /// uygulamayı güncelle
            }
          } else {
            setGetVersionLoading(false);
          }
        });
      } catch (error) {
        setGetVersionLoading(false);
      }
    }
    registerForPushNotificationsAsync();
    checkVersion();
    return () => {};
  }, []);

  if (getVersionLoading) {
    return (
      <View
        style={{
          flexDirection: "row",
          justifyContent: "center",
          alignItems: "center",
          flex: 1
        }}
      >
        <Spinner />
      </View>
    );
  }
  return <AppNavigator />;
}

export default Main;
