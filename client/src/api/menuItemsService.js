import * as apiClient from "./apiClient";

const baseUrl = apiClient.BASE_URL + "/menu/items/";

export function addMenuItem(item) {
  return apiClient.post(baseUrl, { menuItems: [item] });
}

export function getMenuItems() {
  return apiClient.get(baseUrl);
}
