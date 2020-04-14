import React, { useEffect, useState } from "react";
import * as Permissions from "expo-permissions";
import { Notifications, Linking } from "expo";
import { useDispatch } from "react-redux";
import { Spinner, Button } from "native-base";
import { View, Text, Dimensions, Image, Platform } from "react-native";
import * as Analytics from "expo-firebase-analytics";

import { postNotificationToken } from "../src/actions/notificationToken";
import { getProjectDetail } from "../src/actions/settings";
import { ProjectVersion, themeColor } from "../constants/variables";

import AppNavigator from "../navigation/AppNavigator";

const { width, height } = Dimensions.get("window");

function Main() {
  const [getVersionLoading, setGetVersionLoading] = useState(true);
  const [updateRequired, setUpdateRequired] = useState(true);
  const dispatch = useDispatch();
  useEffect(() => {
    async function configurationAppWithWeb() {
      await Analytics.setAnalyticsCollectionEnabled(false);
    }
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
        dispatch(postNotificationToken(token));
      } catch (error) {}
    }
    async function checkVersion() {
      try {
        dispatch(getProjectDetail()).then(({ payload }) => {
          if (
            payload &&
            payload.data &&
            payload.data.VersionNumber !== ProjectVersion
          ) {
            if (payload.data.UpdateRequired) {
              /// uygulamayı güncelle
              setGetVersionLoading(false);
              _handleUpdateApplication();
            } else {
              setGetVersionLoading(false);
              setUpdateRequired(false);
            }
          } else {
            setGetVersionLoading(false);
            setUpdateRequired(false);
          }
        });
      } catch (error) {
        setGetVersionLoading(false);
        setUpdateRequired(false);
      }
    }
    if (Platform.OS === "web") {
      configurationAppWithWeb();
    }
    registerForPushNotificationsAsync();
    checkVersion();
    return () => {};
  }, []);

  function _handleUpdateApplication() {
    if (Platform.OS === "android") {
      Linking.openURL(
        "https://play.google.com/store/apps/details?id=com.antiqmedia.karikaturmadeni"
      );
    } else {
      Linking.openURL(
        "https://play.google.com/store/apps/details?id=com.antiqmedia.karikaturmadeni"
      );
    }
  }

  if (getVersionLoading) {
    return (
      <View
        style={{
          flexDirection: "row",
          justifyContent: "center",
          alignItems: "center",
          flex: 1,
        }}
      >
        <Spinner />
      </View>
    );
  }
  if (updateRequired) {
    return (
      <View
        style={{
          flexDirection: "column",
          justifyContent: "center",
          alignItems: "center",
          flex: 1,
          margin: 10,
        }}
      >
        <Image
          source={require("../assets/images/icon.png")}
          style={{
            width: width * 0.2,
            height: height * 0.2,
            resizeMode: "cover",
          }}
        />
        <Text style={{ textAlign: "center", marginTop: 10, marginBottom: 10 }}>
          Yeni sürümde hatalar düzeltildi. Sizlere daha performanslı bir
          uygulama yapmak için çalışıyoruz. Devam etmek için uygulamayı
          güncelleyiniz.
        </Text>
        <Button
          block
          full
          small
          rounded
          light
          onPress={() => _handleUpdateApplication()}
        >
          <Text style={{ color: themeColor }}>Güncelle</Text>
        </Button>
      </View>
    );
  }
  return <AppNavigator />;
}

export default Main;
