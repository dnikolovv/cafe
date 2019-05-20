import * as apiClient from "./apiClient";
import { joinUrlWithRoute } from "../utils/urlUtils";

const baseUrl = joinUrlWithRoute(apiClient.BASE_URL, "/waiter");

export function getWaiters() {
  return apiClient.get(baseUrl);
}

export function hireWaiter(waiter) {
  const url = joinUrlWithRoute(baseUrl, "hire");
  return apiClient.post(url, waiter);
}

export function assignTable(assignment) {
  const url = joinUrlWithRoute(baseUrl, "table/assign");
  return apiClient.post(url, assignment);
}
