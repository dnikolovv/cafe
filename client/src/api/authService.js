import * as apiClient from "./apiClient";
import { joinUrlWithRoute } from "../utils/urlUtils";

const baseUrl = joinUrlWithRoute(apiClient.BASE_URL, "/auth/");

export function login(credentials) {
  const url = joinUrlWithRoute(baseUrl, "login");
  return apiClient.post(url, credentials);
}

export function logout() {
  const url = joinUrlWithRoute(baseUrl, "logout");
  return apiClient.httpDelete(url);
}

export function getCurrentUser() {
  return apiClient.get(baseUrl);
}
