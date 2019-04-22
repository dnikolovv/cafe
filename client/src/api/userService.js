import * as apiClient from "./apiClient";

const baseUrl = "/auth/";

export function getUsers() {
  const url = baseUrl + "users";
  return apiClient.get(url);
}

export function assignRole(userId, roleId, roleName) {
  const url = baseUrl + "assign/" + roleName;
  const body = {
    accountId: userId,
    [`${roleName}Id`]: roleId
  };
  return apiClient.post(url, body);
}
