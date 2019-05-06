import * as apiClient from "./apiClient";

const baseUrl = apiClient.BASE_URL + "/table";

export function addTable(table) {
  return apiClient.post(baseUrl, table);
}

export function loadTables() {
  return apiClient.get(baseUrl);
}
