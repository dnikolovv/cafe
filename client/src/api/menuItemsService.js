import * as apiClient from "./apiClient";
import { joinUrlWithRoute } from "../utils/urlUtils";

const baseUrl = joinUrlWithRoute(apiClient.BASE_URL, "/menu/items");

export function addMenuItem(item) {
  return apiClient.post(baseUrl, { menuItems: [item] });
}

export function getMenuItems() {
  return apiClient.get(baseUrl);
}
