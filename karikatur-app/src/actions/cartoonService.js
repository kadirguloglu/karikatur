import {
  GET_CARTOONS,
  GET_CARTOONS_URL,
  POST_CARTOON_LIKES,
  POST_CARTOON_LIKES_URL,
  GET_MY_CARTOON_LIKES,
  GET_MY_CARTOON_LIKES_URL
} from "../types/cartoonService";

export function getCartoons(page, count, uniqUserKey) {
  return {
    type: GET_CARTOONS,
    payload: {
      request: {
        url: `${GET_CARTOONS_URL}/${page}/${count}/${uniqUserKey}`
      }
    }
  };
}

export function postCartoonLikes(likes) {
  return {
    type: POST_CARTOON_LIKES,
    payload: {
      request: {
        url: `${POST_CARTOON_LIKES_URL}`,
        method: "POST",
        data: likes
      }
    }
  };
}

export function getMyCartoonLikes(uniqUserKey) {
  return {
    type: GET_MY_CARTOON_LIKES,
    payload: {
      request: {
        url: `${GET_MY_CARTOON_LIKES_URL}/${uniqUserKey}`
      }
    }
  };
}
