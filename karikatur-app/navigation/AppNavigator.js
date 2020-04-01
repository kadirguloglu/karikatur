import React from "react";
import { createStackNavigator } from "@react-navigation/stack";
// import { createAppContainer, createSwitchNavigator } from "react-navigation";
import { View, Text } from "react-native";

import HomeScreen from "../screens/HomeScreen";
import LikeCartoons from "../screens/LikeCartoons";

const Stack = createStackNavigator();

export default function MyStack() {
  return (
    <Stack.Navigator headerMode="none">
      <Stack.Screen name="Main" component={HomeScreen} />
      <Stack.Screen name="LikeCartoons" component={LikeCartoons} />
    </Stack.Navigator>
  );
}
