import * as apiClient from "./apiClient";
import { joinUrlWithRoute } from "../utils/urlUtils";

const uuidv4 = require("uuid/v4");

const baseUrl = joinUrlWithRoute(apiClient.BASE_URL, "/tab/");

export function loadAllTabs() {
  return apiClient.get(baseUrl);
}

export function loadTab(tabId) {
  const url = baseUrl + tabId;
  return apiClient.get(url);
}

export function openTab(tab) {
  const url = joinUrlWithRoute(baseUrl, "open");
  return apiClient.post(url, { ...tab, id: uuidv4() });
}

export function closeTab(tabId, amountPaid) {
  const url = joinUrlWithRoute(baseUrl, "close");
  return apiClient.put(url, { tabId, amountPaid });
}

export function orderMenuItems(tabId, itemNumbers) {
  const url = joinUrlWithRoute(baseUrl, "order");
  return apiClient.put(url, { tabId, itemNumbers });
}

export function rejectMenuItems(tabId, itemNumbers) {
  const url = joinUrlWithRoute(baseUrl, "reject");
  return apiClient.put(url, { tabId, itemNumbers });
}

export function serveMenuItems(tabId, itemNumbers) {
  const url = joinUrlWithRoute(baseUrl, "serve");
  return apiClient.put(url, { tabId, itemNumbers });
}
