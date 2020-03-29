import React, { useState, useEffect, useRef } from "react";
import { useDispatch } from "react-redux";
import {
  Image,
  Modal,
  Dimensions,
  TouchableOpacity,
  StyleSheet,
  CameraRoll,
  AsyncStorage
} from "react-native";
import {
  Container,
  View,
  DeckSwiper,
  Card,
  CardItem,
  Thumbnail,
  Text,
  Left,
  Body,
  Icon,
  Button,
  Spinner,
  Tab,
  Tabs
} from "native-base";
import { getStatusBarHeight } from "react-native-status-bar-height";
import Carousel from "react-native-looped-carousel";
import { AdMobBanner, AdMobRewarded } from "expo-ads-admob";
import Constants from "expo-constants";
import * as FileSystem from "expo-file-system";
import * as Permissions from "expo-permissions";
import * as MediaLibrary from "expo-media-library";

import { getCartoons, postCartoonLikes } from "../src/actions/cartoonService";
import {
  imageWebPageUrl,
  adMobBannerCode,
  adMobVideoAdsCode,
  adMobAwardAdsCode,
  themeColor
} from "../constants/variables";

const { width, height } = Dimensions.get("window");

function bannerError() {}

const BottomAds = ({}) => {
  return (
    <View style={style.buttonContainer}>
      <View style={style.buttons}>
        <AdMobBanner
          bannerSize="banner"
          adUnitID={adMobBannerCode} // Test ID, Replace with your-admob-unit-id
          onDidFailToReceiveAdWithError={bannerError}
        />
      </View>
    </View>
  );
};

const MenuIcon = React.memo(({ navigation }) => {
  return (
    <Icon
      name="list"
      color={themeColor}
      style={{ color: themeColor }}
      onPress={() => navigation.navigate("LikeCartoons")}
    />
  );
});

let currentCartoon = null;
function HomeScreen({ navigation }) {
  const dispatch = useDispatch();
  const _deckSwiper = useRef();
  const [modalVisible, setModalVisible] = useState(false);
  const [swiperPage, setSwiperPage] = useState(1);
  const [page, setPage] = useState(1);
  const [deckElement, setDeckElement] = useState(null);
  const [cartoons, setCartoons] = useState(null);
  const [likeButton, setLikeButton] = useState(true);
  const [getPreview, setGetPreview] = useState(true);
  const [
    spinnerDownloadAdMobRewarded,
    setSpinnerDownloadAdMobRewarded
  ] = useState(false);
  const [isWatchingVideo, setIsWatchingVideo] = useState(false);

  useEffect(() => {
    async function initPage() {
      let storePage = await AsyncStorage.getItem("@page");
      storePage = storePage == null ? 1 : storePage;
      setPage(parseInt(storePage));
      _handleSwipeCartoonItem(1);
      const storageGetPreview = await AsyncStorage.getItem("@getPreview");
      let storeData = storageGetPreview
        ? storageGetPreview === "1"
          ? false
          : true
        : true;
      setGetPreview(storeData);

      if (storeData) {
        setTimeout(() => {
          setGetPreview(false);
          AsyncStorage.setItem("@getPreview", "1");
        }, 5000);
      }

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
    }
    initPage();
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

  const _handleDownloadCartoon = () => {
    try {
      const imageUrl =
        imageWebPageUrl + currentCartoon[0].CartoonImages[0].ImageSrc;
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
  };

  const _handleSwipeCartoonItem = appendPage => {
    let data = null;
    if (swiperPage % 6 == 0) {
      data = (
        <Card style={{ elevation: 3 }}>
          <CardItem>
            <Left>
              <MenuIcon navigation={navigation} />
              <Thumbnail
                source={require("../assets/images/icon.png")}
                circular
              />
              <Body>
                <Text>Karikatür Madeni</Text>
              </Body>
            </Left>
          </CardItem>
          <CardItem cardBody style={style.bannerContainer}>
            <AdMobBanner
              bannerSize="mediumRectangle"
              adUnitID={adMobBannerCode}
            />
          </CardItem>
        </Card>
      );
      setDeckElement(data);
      setLikeButton(false);
    } else {
      setDeckElement(
        <Card style={{ elevation: 3 }}>
          <CardItem>
            <Left>
              <MenuIcon navigation={navigation} />
              <Thumbnail
                source={require("../assets/images/icon.png")}
                circular
              />
              <Body>
                <Text>Karikatür Madeni</Text>
              </Body>
            </Left>
          </CardItem>
          <CardItem cardBody style={style.bannerContainer}>
            <Spinner />
          </CardItem>
        </Card>
      );

      var payloadItem = null;
      if (cartoons) {
        payloadItem = cartoons;
      }
      AsyncStorage.setItem("@page", page + "");
      dispatch(getCartoons(page, 1, Constants.deviceId)).then(({ payload }) => {
        if (payload) {
          if (payload.data) {
            if (payload.data.length) {
              payloadItem = payload.data;
              currentCartoon = payloadItem;
              setCartoons(payloadItem);
              AsyncStorage.setItem("@page", page + "");
            }
          }
        }
        let letCartoonItem;
        if (payloadItem) {
          letCartoonItem = payloadItem[0];
        }
        let _dataObject = (
          <Card style={{ elevation: 3 }}>
            <CardItem>
              <Left>
                <MenuIcon navigation={navigation} />
                {letCartoonItem &&
                letCartoonItem.LogoSrc != null &&
                letCartoonItem.LogoSrc.length ? (
                  <Thumbnail
                    source={{
                      uri: imageWebPageUrl + letCartoonItem.LogoSrc
                    }}
                    circular
                  />
                ) : null}
                {letCartoonItem &&
                letCartoonItem.Name != null &&
                letCartoonItem.Name.length ? (
                  <Body>
                    <Text>{letCartoonItem.Name}</Text>
                  </Body>
                ) : null}
              </Left>
            </CardItem>
            <CardItem cardBody>
              <TouchableOpacity
                style={{
                  flex: 1,
                  flexDirection: "row",
                  flexDirection: "row",
                  justifyContent: "center",
                  alignItems: "center"
                }}
                onPress={() => setModalVisible(true)}
              >
                <Image
                  style={{ flex: 1, height: height * 0.7 }}
                  source={
                    letCartoonItem && {
                      uri:
                        imageWebPageUrl +
                        letCartoonItem?.CartoonImages[0].ImageSrc
                    }
                  }
                  resizeMode="contain"
                />
              </TouchableOpacity>
            </CardItem>
          </Card>
        );
        let newPage = page + appendPage;
        AsyncStorage.setItem("@page", page + "");
        setDeckElement(_dataObject);
        setPage(newPage);
        setLikeButton(true);
      });
    }
    setSwiperPage(swiperPage + 1);
  };

  const _handlePressLikeCartoon = () => {
    var _data = {
      Id: cartoons[0].LikeId,
      CartoonId: cartoons[0].Id,
      UniqUserKey: Constants.deviceId
    };
    dispatch(postCartoonLikes(_data)).then(({ payload }) => {
      if (payload) {
        if (payload.data) {
          setCartoons(payload.data);
        }
      }
    });
  };

  const _handlePressSaveCartoon = async () => {
    setSpinnerDownloadAdMobRewarded(true);
    AdMobRewarded.setAdUnitID(adMobAwardAdsCode);
    await AdMobRewarded.requestAdAsync();
    await AdMobRewarded.showAdAsync();
  };

  const _handleChangePreviewTab = page => {
    if (page === 1) {
      setTimeout(() => {
        setGetPreview(false);
        AsyncStorage.setItem("@getPreview", "1");
      }, 5000);
    }
  };

  function adMobEvent() {}

  const Buttons = ({}) => {
    return (
      <View style={style.buttonContainer}>
        <View style={style.buttons}>
          <Button rounded light onPress={() => _handlePressLikeCartoon()}>
            <Icon
              style={{
                color: themeColor
              }}
              name={cartoons[0]?.IsLiked ? "ios-star" : "ios-star-outline"}
            />
          </Button>
        </View>
        <View style={style.buttons}>
          <Button
            rounded
            light
            onPress={() =>
              spinnerDownloadAdMobRewarded ? null : _handlePressSaveCartoon()
            }
          >
            {spinnerDownloadAdMobRewarded ? (
              <Spinner color={themeColor} style={{ color: themeColor }} />
            ) : (
              <Icon
                style={{
                  color: themeColor
                }}
                name={"ios-download"}
              />
            )}
          </Button>
        </View>
      </View>
    );
  };

  if (getPreview) {
    return (
      <Tabs
        renderTabBar={() => <View />}
        onChangeTab={({ i }) => _handleChangePreviewTab(i)}
      >
        <Tab heading={<View />}>
          <Image
            style={{ width: width, height: height }}
            resizeMode="contain"
            source={require("../assets/images/preview1.png")}
          />
        </Tab>
        <Tab heading={<View />}>
          <TouchableOpacity
            style={{ flex: 1, flexDirection: "row" }}
            onPress={() => {
              setGetPreview(false);
              AsyncStorage.setItem("@getPreview", "1");
            }}
          >
            <Image
              style={{ width: width, height: height }}
              resizeMode="contain"
              source={require("../assets/images/preview2.png")}
            />
          </TouchableOpacity>
        </Tab>
      </Tabs>
    );
  }

  return (
    <Container>
      <Modal animationType={"slide"} transparent={false} visible={modalVisible}>
        <Icon
          name="ios-close"
          style={style.closeIcon}
          size={35}
          onPress={() => setModalVisible(!modalVisible)}
        />
        {cartoons != null ? (
          <Carousel
            delay={2000}
            style={{ flex: 1 }}
            autoplay={false}
            pageInfo
            currentPage={0}
            // onAnimateNextPage={p => console.log(p)}
          >
            {cartoons[0]?.CartoonImages.map((el, i) => (
              <View style={style.modalImageContainer} key={"image" + i}>
                <Image
                  style={{ height: height, width: width }}
                  source={{ uri: imageWebPageUrl + el.ImageSrc }}
                  resizeMode="contain"
                />
              </View>
            ))}
          </Carousel>
        ) : null}
      </Modal>
      <View padder style={{ marginTop: getStatusBarHeight() }}>
        <DeckSwiper
          ref={_deckSwiper}
          dataSource={[0]}
          renderEmpty={() => <Spinner />}
          renderItem={item => {
            return deckElement;
          }}
          onSwipeRight={() => _handleSwipeCartoonItem(1)}
          onSwipeLeft={() => _handleSwipeCartoonItem(-1)}
        />
      </View>
      {cartoons != null && likeButton ? <Buttons></Buttons> : null}
      {likeButton ? null : <BottomAds></BottomAds>}
    </Container>
  );
}

HomeScreen.navigationOptions = {
  header: null
};

export default HomeScreen;

const style = StyleSheet.create({
  buttonContainer: {
    flexDirection: "row",
    flex: 1,
    position: "absolute",
    bottom: 0,
    left: 0,
    right: 0,
    justifyContent: "space-around",
    padding: 15,
    alignItems: "center"
  },
  buttons: {
    flexDirection: "column",
    justifyContent: "center",
    alignItems: "center",
    textAlign: "center"
  },
  closeIcon: {
    position: "absolute",
    top: 0,
    right: 0,
    padding: 15,
    zIndex: 2
  },
  bannerContainer: {
    height: height * 0.7,
    justifyContent: "center",
    alignItems: "center"
  },
  modalImageContainer: {
    flex: 1,
    width: width,
    height: height,
    flexDirection: "row",
    justifyContent: "center",
    alignItems: "center"
  }
});
