import * as apiClient from "./apiClient";

const baseUrl = apiClient.BASE_URL + "/barista/";

export function getBaristas() {
  return apiClient.get(baseUrl);
}
