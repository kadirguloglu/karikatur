import { GET_PROJECT_DETAIL, GET_PROJECT_DETAIL_URL } from "../types/settings";

export function getProjectDetail() {
  return {
    type: GET_PROJECT_DETAIL,
    payload: {
      request: {
        url: `${GET_PROJECT_DETAIL_URL}/KarikaturMadeni`
      }
    }
  };
}
