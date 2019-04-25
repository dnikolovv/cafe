import * as apiClient from "./apiClient";

const baseUrl = apiClient.BASE_URL + "/menu/items/";

export function getMenuItems() {
  return apiClient.get(baseUrl);
}
