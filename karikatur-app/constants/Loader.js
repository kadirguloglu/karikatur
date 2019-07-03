import React from "react";
import AnimatedLoader from "react-native-animated-loader";
import { View } from "native-base";
import { Dimensions } from "react-native";

const { width, height } = Dimensions.get("window");

export default (Loader = props => {
  return (
    <View
      style={{
        width: width,
        height: height,
        position: "absolute",
        flex: 1,
        justifyContent: "center",
        alignItems: "center",
        zIndex: 999999
      }}
    >
      <AnimatedLoader
        visible={true}
        overlayColor="transparent"
        source={require("../lottie-files.json")}
        animationStyle={{
          width: width * 0.7,
          height: height
        }}
        speed={1}
      />
    </View>
  );
});
