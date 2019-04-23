import * as apiClient from "./apiClient";

const baseUrl = "/menu/items/";

export function getMenuItems() {
  return apiClient.get(baseUrl);
}
