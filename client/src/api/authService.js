import * as apiClient from "./apiClient";

const baseUrl = "/auth/";

export function login(credentials) {
  const url = baseUrl + "login";
  return apiClient.post(url, credentials);
}

export function getCurrentUser() {
  return apiClient.get(baseUrl);
}
