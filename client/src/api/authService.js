import { handleResponse, handleError } from "./apiUtils";

const baseUrl = "/auth/";

export function login(credentials) {
  return fetch(baseUrl + "login", {
    method: "POST",
    headers: {
      Accept: "application/json",
      "Content-Type": "application/json"
    },
    body: JSON.stringify(credentials)
  })
    .then(handleResponse)
    .catch(handleError);
}
