import {
  GET_CARTOONS,
  GET_CARTOONS_SUCCESS,
  GET_CARTOONS_FAIL,
  POST_CARTOON_LIKES,
  POST_CARTOON_LIKES_SUCCESS,
  POST_CARTOON_LIKES_FAIL
} from "../types/cartoonService";
import Sentry from "sentry-expo";

const INITIAL_STATE = {};

export default (state = INITIAL_STATE, action) => {
  switch (action.type) {
    case GET_CARTOONS:
      return { ...state, getCartoonsLoading: true };
    case GET_CARTOONS_SUCCESS:
      return {
        ...state,
        getCartoonsLoading: false,
        getCartoonsData: action.payload.data
      };
    case GET_CARTOONS_FAIL:
      return { ...state, getCartoonsLoading: false };

    case POST_CARTOON_LIKES:
      return { ...state, postCartoonLikesLoading: true };
    case POST_CARTOON_LIKES_SUCCESS:
      return {
        ...state,
        postCartoonLikesLoading: false,
        postCartoonLikesData: action.payload.data
      };
    case POST_CARTOON_LIKES_FAIL:
      return { ...state, postCartoonLikesLoading: false };

    default:
      return { ...state };
  }
};
