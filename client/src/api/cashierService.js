import * as apiClient from "./apiClient";

const baseUrl = apiClient.BASE_URL + "/cashier/";

export function getCashiers() {
  return apiClient.get(baseUrl);
}
