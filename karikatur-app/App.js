import { AppLoading } from "expo";
import { Asset } from "expo-asset";
import * as Font from "expo-font";
import React, { useState } from "react";
import { Platform, StatusBar, StyleSheet, View } from "react-native";
import { Ionicons } from "@expo/vector-icons";

import { createStore, applyMiddleware } from "redux";
import thunk from "redux-thunk";
import reducers from "./src/reducers";
import { Provider } from "react-redux";
import axios from "axios";
import axiosMiddleware from "redux-axios-middleware";
import logger from "redux-logger";
import Sentry from "sentry-expo";

import Main from "./screens/Main";

// Sentry.config(
//   "https://126f9bb1294345ffb7627dc5527e6fe0@sentry.io/1486156"
// ).install();

// Sentry.init({
//   dsn: "https://126f9bb1294345ffb7627dc5527e6fe0@sentry.io/1486156",
//   enableInExpoDevelopment: true,
//   debug: true
// });

const client = axios.create({
  baseURL: "http://karikatur-api.kadirguloglu.com",
  responseType: "json"
});

client.interceptors.request.use(request => {
  //alert("Request : " + JSON.stringify(request));
  //console.log("Request : ", JSON.stringify(request));
  return request;
});

client.interceptors.response.use(response => {
  //alert("Response:", JSON.stringify(response));
  //console.log("Response : ", JSON.stringify(response));
  return response;
});

const store = createStore(
  reducers,
  {},
  applyMiddleware(thunk, axiosMiddleware(client))
);

export default function App(props) {
  const [isLoadingComplete, setLoadingComplete] = useState(false);

  if (!isLoadingComplete && !props.skipLoadingScreen) {
    return (
      <AppLoading
        startAsync={loadResourcesAsync}
        onError={handleLoadingError}
        onFinish={() => handleFinishLoading(setLoadingComplete)}
      />
    );
  } else {
    return (
      <View style={styles.container}>
        {Platform.OS === "ios" && <StatusBar barStyle="default" />}
        <Provider store={store}>
          <Main />
        </Provider>
      </View>
    );
  }
}

async function loadResourcesAsync() {
  await Promise.all([
    Asset.loadAsync([]),
    Font.loadAsync({
      Roboto: require("native-base/Fonts/Roboto.ttf"),
      Roboto_medium: require("native-base/Fonts/Roboto_medium.ttf"),
      // This is the font that we are using for our tab bar
      ...Ionicons.font,
      // We include SpaceMono because we use it in HomeScreen.js. Feel free to
      // remove this if you are not using it in your app
      "space-mono": require("./assets/fonts/SpaceMono-Regular.ttf")
    })
  ]);
}

function handleLoadingError(error) {
  // In this case, you might want to report the error to your error reporting
  // service, for example Sentry
  console.warn(error);
}

function handleFinishLoading(setLoadingComplete) {
  setLoadingComplete(true);
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: "#fff"
  }
});
