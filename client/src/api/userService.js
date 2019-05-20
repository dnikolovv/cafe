import * as apiClient from "./apiClient";
import { joinUrlWithRoute } from "../utils/urlUtils";

const baseUrl = joinUrlWithRoute(apiClient.BASE_URL, "/auth");

export function getUsers() {
  const url = joinUrlWithRoute(baseUrl, "users");
  return apiClient.get(url);
}

export function assignRole(userId, roleId, roleName) {
  const url = joinUrlWithRoute(baseUrl, "assign/" + roleName);
  const body = {
    accountId: userId,
    [`${roleName}Id`]: roleId
  };
  return apiClient.post(url, body);
}
