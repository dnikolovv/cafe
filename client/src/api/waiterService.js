import * as apiClient from "./apiClient";

const baseUrl = "/waiter/";

export function getWaiters() {
  return apiClient.get(baseUrl);
}
