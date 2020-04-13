import {
  GET_CARTOONS,
  GET_CARTOONS_SUCCESS,
  GET_CARTOONS_FAIL,
  POST_CARTOON_LIKES,
  POST_CARTOON_LIKES_SUCCESS,
  POST_CARTOON_LIKES_FAIL,
  GET_MY_CARTOON_LIKES,
  GET_MY_CARTOON_LIKES_SUCCESS,
  GET_MY_CARTOON_LIKES_FAIL,
  GET_CARTOON_GALLERY,
  GET_CARTOON_GALLERY_SUCCESS,
  GET_CARTOON_GALLERY_FAIL,
} from "../types/cartoonService";

const INITIAL_STATE = {
  getMyCartoonLikesLoading: true,
  getMyCartoonLikesFail: false,
  getMyCartoonLikesResult: [],
  getCartoonGalleryLoading: false,
  getCartoonGalleryFail: false,
  getCartoonGalleryResult: [],
};

export default (state = INITIAL_STATE, action) => {
  switch (action.type) {
    case GET_CARTOONS:
      return { ...state, getCartoonsLoading: true };
    case GET_CARTOONS_SUCCESS:
      return {
        ...state,
        getCartoonsLoading: false,
        getCartoonsData: action.payload.data,
      };
    case GET_CARTOONS_FAIL:
      return { ...state, getCartoonsLoading: false };

    case POST_CARTOON_LIKES:
      return { ...state, postCartoonLikesLoading: true };
    case POST_CARTOON_LIKES_SUCCESS:
      return {
        ...state,
        postCartoonLikesLoading: false,
        postCartoonLikesData: action.payload.data,
      };
    case POST_CARTOON_LIKES_FAIL:
      return { ...state, postCartoonLikesLoading: false };

    case GET_MY_CARTOON_LIKES:
      return {
        ...state,
        getMyCartoonLikesLoading: true,
        getMyCartoonLikesFail: false,
        getMyCartoonLikesResult: [],
      };
    case GET_MY_CARTOON_LIKES_SUCCESS:
      return {
        ...state,
        getMyCartoonLikesLoading: false,
        getMyCartoonLikesFail: false,
        getMyCartoonLikesResult: action.payload.data,
      };
    case GET_MY_CARTOON_LIKES_FAIL:
      return {
        ...state,
        getMyCartoonLikesLoading: false,
        getMyCartoonLikesFail: true,
        getMyCartoonLikesResult: [],
      };

    case GET_CARTOON_GALLERY:
      return {
        ...state,
        getCartoonGalleryLoading: true,
        getCartoonGalleryFail: false,
        getCartoonGalleryResult: [],
      };
    case GET_CARTOON_GALLERY_SUCCESS:
      return {
        ...state,
        getCartoonGalleryLoading: false,
        getCartoonGalleryFail: false,
        getCartoonGalleryResult: action.payload.data,
      };
    case GET_CARTOON_GALLERY_FAIL:
      return {
        ...state,
        getCartoonGalleryLoading: false,
        getCartoonGalleryFail: true,
        getCartoonGalleryResult: [],
      };

    default:
      return { ...state };
  }
};
