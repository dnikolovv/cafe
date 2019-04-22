import * as apiClient from "./apiClient";

const baseUrl = "/cashier/";

export function getCashiers() {
  return apiClient.get(baseUrl);
}
