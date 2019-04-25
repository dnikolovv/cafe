import * as apiClient from "./apiClient";

const baseUrl = apiClient.BASE_URL + "/manager/";

export function getManagers() {
  return apiClient.get(baseUrl);
}
