import * as apiClient from "./apiClient";

const baseUrl = "/manager/";

export function getManagers() {
  return apiClient.get(baseUrl);
}
