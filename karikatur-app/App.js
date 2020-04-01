import React, { useState, useRef, useEffect } from "react";
import {
  Platform,
  StatusBar,
  StyleSheet,
  View,
  AsyncStorage
} from "react-native";
import { AppLoading } from "expo";
import { Asset } from "expo-asset";
import * as Font from "expo-font";
import { Ionicons } from "@expo/vector-icons";
import { NavigationContainer } from "@react-navigation/native";
import { SplashScreen } from "expo";
import Constants from "expo-constants";
import { createStore, applyMiddleware } from "redux";
import thunk from "redux-thunk";
import reducers from "./src/reducers";
import { Provider } from "react-redux";
import axios from "axios";
import axiosMiddleware from "redux-axios-middleware";
import { changeUniqUserData } from "./constants/variables";

import Main from "./screens/Main";
import useLinking from "./navigation/useLinking";

const client = axios.create({
  baseURL: "http://karikatur-api.antiquemedia.xyz",
  responseType: "json"
});

client.interceptors.request.use(request => {
  //alert("Request : " + JSON.stringify(request));
  return request;
});

client.interceptors.response.use(response => {
  //alert("Response:", JSON.stringify(response));
  return response;
});

const store = createStore(
  reducers,
  {},
  applyMiddleware(thunk, axiosMiddleware(client))
);

export default function App(props) {
  const [isLoadingComplete, setLoadingComplete] = useState(false);
  const [initialNavigationState, setInitialNavigationState] = useState();
  const [uniqValueLoading, setUniqValueLoading] = useState(true);
  const containerRef = useRef();
  const { getInitialState } = useLinking(containerRef);

  useEffect(() => {
    async function loadResourcesAndDataAsync() {
      try {
        SplashScreen.preventAutoHide();
        const uniqUserValue = await AsyncStorage.getItem("@uniqUserValue");
        let uniqValue = uniqUserValue;
        if (uniqUserValue === null) {
          uniqValue =
            Constants.sessionId + "" + parseInt(Math.random() * 1000000) + "";
        }
        await AsyncStorage.setItem("@uniqUserValue", uniqValue);
        changeUniqUserData(uniqValue);
        setUniqValueLoading(false);

        // Load our initial navigation state
        setInitialNavigationState(await getInitialState());

        // Load fonts
        await Font.loadAsync({
          ...Ionicons.font,
          "space-mono": require("./assets/fonts/SpaceMono-Regular.ttf")
        });
      } catch (e) {
        // We might want to provide this error information to an error reporting service
        console.warn(e);
      } finally {
        setLoadingComplete(true);
        SplashScreen.hide();
      }
    }

    loadResourcesAndDataAsync();
  }, []);

  if (!isLoadingComplete && !props.skipLoadingScreen && uniqValueLoading) {
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
        <StatusBar hidden />
        <Provider store={store}>
          <NavigationContainer
            ref={containerRef}
            initialState={initialNavigationState}
          >
            <Main />
          </NavigationContainer>
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
    backgroundColor: "#fff",
    marginTop: Platform.OS === "ios" ? 0 : -StatusBar.currentHeight
  }
});
