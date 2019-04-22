import * as apiClient from "./apiClient";

const baseUrl = "/barista/";

export function getBaristas() {
  return apiClient.get(baseUrl);
}
