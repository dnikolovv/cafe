import * as apiClient from "./apiClient";

const baseUrl = "/auth/";

export function getUsers() {
  const url = baseUrl + "users";
  return apiClient.get(url);
}
