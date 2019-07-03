import React from "react";
import {
  Image,
  Modal,
  Dimensions,
  TouchableOpacity,
  StyleSheet,
  CameraRoll,
  Platform,
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
import {
  AdMobBanner,
  AdMobInterstitial,
  PublisherBanner,
  AdMobRewarded
} from "expo-ads-admob";
import { connect } from "react-redux";
import Constants from "expo-constants";
import * as FileSystem from "expo-file-system";
import * as Permissions from "expo-permissions";

import { getCartoons, postCartoonLikes } from "../src/actions/cartoonService";

// AdMobInterstitial.setAdUnitID("ca-app-pub-2691089291450682/1466803456");
// AdMobInterstitial.setTestDeviceID("EMULATOR");
// await AdMobInterstitial.requestAdAsync();
// await AdMobInterstitial.showAdAsync();

const imageWebPageUrl = "http://karikatur-admin.antiquemedia.net";

const adMobBannerCode = "ca-app-pub-2691089291450682/2988656739";

const themeColor = "#ff487e";

const { width, height } = Dimensions.get("window");

class HomeScreen extends React.Component {
  state = {
    modalVisible: false,
    swiperPage: 1,
    page: 1,
    deckElement: null,
    cartoons: null,
    getLoader: false,
    likeButton: true,
    getPreview: true,
    spinnerDownloadAdMobRewarded: false,
    isWatchingVideo: false
  };
  static navigationOptions = {
    header: null
  };

  async componentWillMount() {
    let imageHeight = Math.round((width * 9) / 16);
    let imageWidth = width;
    this.setState({ imageHeight, imageWidth });
    this._handleSwipeCartoonItem(1);
    const storageGetPreview = await AsyncStorage.getItem("@getPreview");
    this.setState({
      getPreview: storageGetPreview
        ? storageGetPreview === "1"
          ? false
          : true
        : true
    });

    AdMobRewarded.addEventListener("rewardedVideoDidRewardUser", () =>
      this.setState({ isWatchingVideo: true })
    );
    AdMobRewarded.addEventListener("rewardedVideoDidLoad", () =>
      this.setState({ isWatchingVideo: false })
    );
    AdMobRewarded.addEventListener("rewardedVideoDidFailToLoad", () =>
      console.log("rewardedVideoDidFailToLoad")
    );
    AdMobRewarded.addEventListener("rewardedVideoDidOpen", () =>
      this.setState({ spinnerDownloadAdMobRewarded: false })
    );

    AdMobRewarded.addEventListener("rewardedVideoDidClose", () =>
      this._handleDownloadCartoon()
    );
    AdMobRewarded.addEventListener("rewardedVideoWillLeaveApplication", () =>
      console.log("rewardedVideoWillLeaveApplication")
    );
  }

  _handleDownloadCartoon = () => {
    const { isWatchingVideo } = this.state;
    try {
      const imageUrl =
        imageWebPageUrl + this.state.cartoons[0].CartoonImages[0].ImageSrc;
      const splitUrl = imageUrl.split("/");
      const fileName = splitUrl[splitUrl.length - 1];
      FileSystem.downloadAsync(
        imageUrl,
        FileSystem.documentDirectory + fileName
      )
        .then(({ uri }) => {
          Permissions.askAsync(Permissions.CAMERA_ROLL).then(({ status }) => {
            if (status === "granted") {
              CameraRoll.saveToCameraRoll(uri).then(uriGallery => {
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
    } catch (error) {
      alert("Karikatür kaydedilemedi. İnternet bağlantınızı kontrol ediniz.");
    }
  };

  _handleOnLoadBanner = () => {
    this.setState({ getLoader: false });
  };

  _handleSetInitialState = (p, v) => {
    this.setState({ [p]: v });
  };

  _handleOnImagePress = item => {
    this.setState({ modalVisible: true });
  };

  _handleOnErrorBanner = item => {
    this.setState({ getLoader: false });
    this._handleSwipeCartoonItem(1);
  };

  _handleSwipeCartoonItem = appendPage => {
    let { swiperPage, page } = this.state;
    let data = null;
    if (swiperPage % 6 == 0) {
      this.setState({ getLoader: true });
      data = (
        <Card style={{ elevation: 3 }}>
          <CardItem>
            <Left>
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
              testDeviceID="EMULATOR"
              onDidFailToReceiveAdWithError={this._handleOnErrorBanner}
              onAdViewDidReceiveAd={this._handleOnLoadBanner}
            />
          </CardItem>
        </Card>
      );
      this.setState({ deckElement: data, likeButton: false });
    } else {
      this.setState({
        deckElement: (
          <Card style={{ elevation: 3 }}>
            <CardItem>
              <Left>
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
        )
      });
      const { getCartoons } = this.props;
      var payloadItem = null;
      if (this.state.cartoons) {
        payloadItem = this.state.cartoons;
      }
      getCartoons(page, 1, Constants.deviceId).then(({ payload }) => {
        if (payload) {
          if (payload.data) {
            if (payload.data.length) {
              payloadItem = payload.data;
            }
          }
        }
        letCartoonItem = payloadItem[0];
        let _dataObject = (
          <Card style={{ elevation: 3 }}>
            <CardItem>
              <Left>
                {letCartoonItem.LogoSrc != null &&
                letCartoonItem.LogoSrc.length ? (
                  <Thumbnail
                    source={{
                      uri: imageWebPageUrl + letCartoonItem.LogoSrc
                    }}
                    circular
                  />
                ) : null}
                {letCartoonItem.Name != null && letCartoonItem.Name.length ? (
                  <Body>
                    <Text>{letCartoonItem.Name}</Text>
                  </Body>
                ) : null}
              </Left>
            </CardItem>
            <CardItem cardBody>
              <TouchableOpacity
                style={{ flex: 1, flexDirection: "row" }}
                onPress={() => this._handleOnImagePress()}
              >
                <Image
                  style={{ flex: 1, height: height * 0.7 }}
                  source={{
                    uri:
                      imageWebPageUrl + letCartoonItem.CartoonImages[0].ImageSrc
                  }}
                  resizeMode="center"
                />
              </TouchableOpacity>
            </CardItem>
          </Card>
        );
        let newPage = page + appendPage;
        this.setState({
          deckElement: _dataObject,
          cartoons: payloadItem,
          page: newPage,
          likeButton: true
        });
      });
    }
    this.setState({ swiperPage: swiperPage + 1 });
  };

  _handlePressLikeCartoon = () => {
    this.setState({ getLoader: true });
    const { postCartoonLikes } = this.props;
    let { cartoons } = this.state;
    var _data = {
      Id: cartoons[0].LikeId,
      CartoonId: cartoons[0].Id,
      UniqUserKey: Constants.deviceId
    };
    postCartoonLikes(_data).then(({ payload }) => {
      if (payload) {
        if (payload.data) {
          this.setState({ cartoons: payload.data });
        }
      }
    });
    this.setState({ getLoader: false });
  };

  _handlePressSaveCartoon = async () => {
    this.setState({ spinnerDownloadAdMobRewarded: true });
    AdMobRewarded.setAdUnitID("ca-app-pub-3940256099942544/5224354917"); // Test ID, Replace with your-admob-unit-id
    AdMobRewarded.setTestDeviceID("EMULATOR");
    await AdMobRewarded.requestAdAsync();
    await AdMobRewarded.showAdAsync();
  };

  _handleChangePreviewTab = page => {
    if (page == 1) {
      setTimeout(() => {
        this.setState({ getPreview: false });
        this._handleSetPreviewPageInStorage();
      }, 5000);
    }
  };

  _handleClosePreviewTab = () => {
    this.setState({ getPreview: false });
    this._handleSetPreviewPageInStorage();
  };

  _handleSetPreviewPageInStorage = () => {
    AsyncStorage.setItem("@getPreview", "1");
  };

  bannerError() {}

  adMobEvent() {}

  render() {
    const {
      modalVisible,
      deckElement,
      cartoons,
      getLoader,
      likeButton,
      getPreview,
      spinnerDownloadAdMobRewarded
    } = this.state;
    return (
      <Container>
        {getPreview ? (
          <Tabs
            renderTabBar={() => <View />}
            onChangeTab={({ i }) => this._handleChangePreviewTab(i)}
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
                onPress={() => this._handleClosePreviewTab()}
              >
                <Image
                  style={{ width: width, height: height }}
                  resizeMode="contain"
                  source={require("../assets/images/preview2.png")}
                />
              </TouchableOpacity>
            </Tab>
          </Tabs>
        ) : (
          <React.Fragment>
            <Modal
              animationType={"slide"}
              transparent={false}
              visible={modalVisible}
            >
              <Icon
                name="ios-close"
                style={style.closeIcon}
                size={35}
                onPress={() =>
                  this._handleSetInitialState("modalVisible", !modalVisible)
                }
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
                  {cartoons[0].CartoonImages.map((el, i) => (
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
                ref={c => (this._deckSwiper = c)}
                dataSource={[0]}
                renderEmpty={() => <Spinner />}
                renderItem={item => {
                  return deckElement;
                }}
                onSwipeRight={() => this._handleSwipeCartoonItem(1)}
                onSwipeLeft={() => this._handleSwipeCartoonItem(-1)}
              />
            </View>
            {cartoons != null && likeButton ? (
              <View style={style.buttonContainer}>
                <View style={style.buttons}>
                  <Button
                    rounded
                    light
                    onPress={() => this._handlePressLikeCartoon()}
                  >
                    <Icon
                      style={{
                        color: themeColor
                      }}
                      name={
                        cartoons[0].IsLiked ? "ios-star" : "ios-star-outline"
                      }
                    />
                  </Button>
                </View>
                <View style={style.buttons}>
                  <Button
                    rounded
                    light
                    onPress={() =>
                      spinnerDownloadAdMobRewarded
                        ? null
                        : this._handlePressSaveCartoon()
                    }
                  >
                    {spinnerDownloadAdMobRewarded ? (
                      <Spinner
                        color={themeColor}
                        style={{ color: themeColor }}
                      />
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
            ) : null}
            {likeButton ? null : (
              <View style={style.buttonContainer}>
                <View style={style.buttons}>
                  <AdMobBanner
                    bannerSize="banner"
                    adUnitID={adMobBannerCode} // Test ID, Replace with your-admob-unit-id
                    testDeviceID="EMULATOR"
                    onDidFailToReceiveAdWithError={this.bannerError}
                  />
                </View>
              </View>
            )}
          </React.Fragment>
        )}
      </Container>
    );
  }
}

const mapStateToProps = ({ cartoonServiceResponse }) => ({
  cartoonServiceResponse
});

const mapDispatchToProps = {
  getCartoons,
  postCartoonLikes
};

export default connect(
  mapStateToProps,
  mapDispatchToProps
)(HomeScreen);

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
