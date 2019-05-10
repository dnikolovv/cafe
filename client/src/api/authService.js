import * as apiClient from "./apiClient";

const baseUrl = apiClient.BASE_URL + "/auth/";

export function login(credentials) {
  const url = baseUrl + "login";
  return apiClient.post(url, credentials);
}

export function logout() {
  const url = baseUrl + "logout";
  return apiClient.httpDelete(url);
}

export function getCurrentUser() {
  return apiClient.get(baseUrl);
}
