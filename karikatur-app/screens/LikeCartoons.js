import React, { useEffect, useState } from "react";
import {
  Text,
  Container,
  Header,
  Left,
  Content,
  Body,
  Title,
  Icon,
  View,
  Spinner,
  Right,
  Button
} from "native-base";
import { Image, Dimensions, StyleSheet } from "react-native";
import Constants from "expo-constants";
import { useDispatch, useSelector } from "react-redux";
import { FlatGrid } from "react-native-super-grid";
import { AdMobRewarded } from "expo-ads-admob";

import {
  imageWebPageUrl,
  adMobBannerCode,
  adMobVideoAdsCode,
  adMobAwardAdsCode,
  themeColor
} from "../constants/variables";

import {
  getMyCartoonLikes,
  postCartoonLikes
} from "../src/actions/cartoonService";

function LikeCartoons({ navigation }) {
  const dispatch = useDispatch();
  const [selectedCartoon, setSelectedCartoon] = useState(null);
  const [isWatchingVideo, setIsWatchingVideo] = useState(false);
  const [
    spinnerDownloadAdMobRewarded,
    setSpinnerDownloadAdMobRewarded
  ] = useState(false);

  const {
    getMyCartoonLikesLoading,
    getMyCartoonLikesFail,
    getMyCartoonLikesResult
  } = useSelector(x => x.cartoonServiceResponse);

  useEffect(() => {
    dispatch(getMyCartoonLikes(Constants.deviceId));
    return () => {};
  }, []);

  useEffect(() => {
    AdMobRewarded.addEventListener("rewardedVideoDidRewardUser", () => {
      setIsWatchingVideo(true);
    });
    AdMobRewarded.addEventListener("rewardedVideoDidLoad", () => {
      setIsWatchingVideo(false);
    });
    AdMobRewarded.addEventListener("rewardedVideoDidFailToLoad", () => {
      setIsWatchingVideo(false);
    });
    AdMobRewarded.addEventListener("rewardedVideoDidComplete", () => {
      setIsWatchingVideo(true);
    });
    AdMobRewarded.addEventListener("rewardedVideoDidOpen", () =>
      setSpinnerDownloadAdMobRewarded(false)
    );

    AdMobRewarded.addEventListener("rewardedVideoDidClose", () =>
      _handleDownloadCartoon()
    );

    AdMobRewarded.addEventListener("rewardedVideoWillLeaveApplication", () =>
      setIsWatchingVideo(false)
    );
    return () => {
      try {
        AdMobRewarded.removeAllListeners();
        AdMobRewarded.removeEventListener("rewardedVideoDidRewardUser");
        AdMobRewarded.removeEventListener("rewardedVideoDidLoad");
        AdMobRewarded.removeEventListener("rewardedVideoDidFailToLoad");
        AdMobRewarded.removeEventListener("rewardedVideoDidOpen");
        AdMobRewarded.removeEventListener("rewardedVideoDidClose");
        AdMobRewarded.removeEventListener("rewardedVideoWillLeaveApplication");
        AdMobRewarded.removeEventListener("rewardedVideoDidComplete");
      } catch (error) {}
    };
  }, []);

  const _handlePressSaveCartoon = async cartoon => {
    setSelectedCartoon(cartoon);
    setIsWatchingVideo(false);
    setSpinnerDownloadAdMobRewarded(true);
    AdMobRewarded.setAdUnitID(adMobAwardAdsCode);
    await AdMobRewarded.requestAdAsync();
    await AdMobRewarded.showAdAsync();
  };

  function _handleDownloadCartoon() {
    try {
      const imageUrl =
        imageWebPageUrl + selectedCartoon.CartoonImages[0].ImageSrc;
      if (imageUrl) {
        const splitUrl = imageUrl.split("/");
        let rnd = parseInt(Math.random() * 1000000000000) + "";
        const fileName = rnd + splitUrl[splitUrl.length - 1];
        FileSystem.downloadAsync(
          imageUrl,
          FileSystem.documentDirectory + fileName
        )
          .then(({ uri }) => {
            Permissions.askAsync(Permissions.CAMERA_ROLL).then(({ status }) => {
              if (status === "granted") {
                MediaLibrary.saveToLibraryAsync(uri).then(uriGallery => {
                  let alertText = "Karikatür Kaydedildi.";
                  if (isWatchingVideo) {
                    alertText +=
                      " Videoyu izleyip bize destek olduğunuz için teşekkür ederiz.";
                  } else {
                    alertText +=
                      " Videoları izleyerek bize desktek olabilirisiniz.";
                  }
                  alert(alertText);
                });
              }
            });
          })
          .catch(error => {
            console.error(error);
          });
      } else {
        alert("Karikatür kaydedilemedi. İnternet bağlantınızı kontrol ediniz.");
      }
    } catch (error) {
      alert("Karikatür kaydedilemedi. İnternet bağlantınızı kontrol ediniz.");
    }
  }

  return (
    <Container>
      <Header transparent rounded={true}>
        <Left>
          <Icon
            name="image"
            onPress={() => navigation.navigate("Main")}
            style={{ color: themeColor }}
            color={themeColor}
          />
        </Left>
        <Body>
          <Title color="black" style={{ color: "black" }}>
            Beğendiklerim
          </Title>
        </Body>
        <Right />
      </Header>
      {getMyCartoonLikesLoading ? (
        <View style={styles.centerPage}>
          <Spinner />
        </View>
      ) : getMyCartoonLikesFail ? (
        <View style={styles.centerPage}>
          <Text>İşlem sırasında bir hata oluştu</Text>
        </View>
      ) : (
        <FlatGrid
          itemDimension={130}
          style={{ flex: 1 }}
          items={getMyCartoonLikesResult}
          renderItem={({ item, index }) => {
            return (
              <View key={"image-" + index} style={[styles.cartoonContainer]}>
                <Image
                  style={styles.image}
                  source={{
                    uri: imageWebPageUrl + item.CartoonImages[0].ImageSrc
                  }}
                  resizeMode="contain"
                />
                <View style={styles.buttons}>
                  <Button
                    rounded
                    style={styles.button}
                    onPress={() => {
                      dispatch(
                        postCartoonLikes({
                          Id: item.LikeId,
                          CartoonId: item.Id,
                          UniqUserKey: Constants.deviceId
                        })
                      ).then(() => {
                        dispatch(getMyCartoonLikes(Constants.deviceId));
                      });
                    }}
                  >
                    <Icon name="star" style={styles.icon} />
                  </Button>
                  <Button
                    rounded
                    style={styles.button}
                    onPress={() =>
                      spinnerDownloadAdMobRewarded
                        ? null
                        : _handlePressSaveCartoon(item)
                    }
                  >
                    {spinnerDownloadAdMobRewarded ? (
                      <Spinner
                        color={themeColor}
                        style={{ color: themeColor }}
                      />
                    ) : (
                      <Icon name="download" style={styles.icon} />
                    )}
                  </Button>
                </View>
              </View>
            );
          }}
        />
      )}
    </Container>
  );
}

export default LikeCartoons;

const styles = StyleSheet.create({
  centerPage: {
    flexDirection: "row",
    justifyContent: "center",
    alignItems: "center"
  },
  cartoonContainer: {
    borderWidth: 1,
    borderColor: "#DCDCDC",
    padding: 2,
    justifyContent: "flex-end"
  },
  image: { flex: 1, height: 150 },
  buttons: {
    flexDirection: "row",
    justifyContent: "center",
    alignItems: "center"
  },
  button: {
    backgroundColor: themeColor,
    justifyContent: "center",
    alignItems: "center",
    alignContent: "center",
    flexDirection: "row",
    margin: 2,
    width: 35,
    height: 35
  },
  icon: {
    fontSize: 20,
    width: 20,
    height: 20,
    textAlign: "center",
    alignItems: "center",
    alignSelf: "center"
  }
});
