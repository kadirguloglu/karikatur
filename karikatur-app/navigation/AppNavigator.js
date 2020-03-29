import React from "react";
import { createAppContainer, createSwitchNavigator } from "react-navigation";
import { View, Text } from "react-native";

import HomeScreen from "../screens/HomeScreen";
import LikeCartoons from "../screens/LikeCartoons";

export default createAppContainer(
  createSwitchNavigator({
    // You could add another route here for authentication.
    // Read more at https://reactnavigation.org/docs/en/auth-flow.html
    //Main: MainTabNavigator,
    Main: HomeScreen,
    LikeCartoons: LikeCartoons
  })
);
